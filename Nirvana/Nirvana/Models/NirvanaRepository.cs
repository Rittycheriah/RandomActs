using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nirvana.Models
{
    public class NirvanaRepository
    {
        private NirvanaContext context;

        public NirvanaRepository()
        {
            context = new NirvanaContext();
        }

        public NirvanaRepository(NirvanaContext this_context)
        {
            context = this_context;
        }

        public List<RandomActsModel> GetAllActs()
        {
            return context.Acts.ToList();
        }

        public List<RandomActsModel> GetAllActs(ApplicationUser _user)
        {
            var UserActsQuery = from acts in context.Acts where acts.Owner == _user select acts;
            return UserActsQuery.ToList<RandomActsModel>();
        }

        public RandomActsModel CreateAct(string ActTitle, string ActDescription, DateTime ActDate, ApplicationUser owner)
        {
            RandomActsModel _act = new RandomActsModel { RandomActTitle = ActTitle, RandomActDescription = ActDescription, Date = ActDate, Owner = owner, PointsEarned = 3 };
            context.Acts.Add(_act);
            context.SaveChanges();

            return _act;
        }

        public int GetActCount()
        {
            var query = from acts in context.Acts select acts;
            return query.Count();
        }

        public int GetActCount(ApplicationUser user)
        {
            var query = from acts in context.Acts where acts.Owner == user select acts;

            return query.Count();
        }

        public Rank GetUserRank(ApplicationUser user)
        {
            int TotalPts = GetTotalPoints(user);
            Rank UserRank = new Rank();
            RankDefinitions this_rank = new RankDefinitions();

            if (TotalPts < 10 && TotalPts > 0)
            {
                var query = context.Definitions.Where(n => n.RankingCode == 1);
                this_rank = query.First();
            }
            else if (TotalPts > 10 && TotalPts < 20)
            {
                var query = context.Definitions.Where(n => n.RankingCode == 2);
                this_rank = query.First();
            }
            else if (TotalPts > 20 && TotalPts < 30)
            {
                var query = context.Definitions.Where(n => n.RankingCode == 3);
                this_rank = query.First();
            }
            else if (TotalPts > 30 && TotalPts < 40)
            {
                var query = context.Definitions.Where(n => n.RankingCode == 4);
                this_rank = query.First();
            }
            else if (TotalPts > 40 && TotalPts < 60)
            {
                var query = context.Definitions.Where(n => n.RankingCode == 5);
                this_rank = query.First();
            }
            else if (TotalPts > 60 && TotalPts < 75)
            {
                var query = context.Definitions.Where(n => n.RankingCode == 6);
                this_rank = query.First();
            }
            else if (TotalPts > 75 && TotalPts < 100)
            {
                var query = context.Definitions.Where(n => n.RankingCode == 7);
                this_rank = query.First();
            }
            else if (TotalPts > 100 && TotalPts < 200)
            {
                var query = context.Definitions.Where(n => n.RankingCode == 8);
                this_rank = query.First();
            }

            UserRank.Name = this_rank.RankingName;
            UserRank.BasePtsAllowance = this_rank.RankingBasePts;
            UserRank.CommentFeat = this_rank.RankingComments;
            UserRank.MinimumPtReq = this_rank.RankingMinPt;
            UserRank.SocialMedia = this_rank.RankingSocial;

            return UserRank;

        }

        public Dictionary<string, int> GetAllUsersRanks()
        {
            List<ApplicationUser> AllUsers = context.Users.ToList();
            Dictionary<string, int> Leaderboard = new Dictionary<string, int>();

            foreach (ApplicationUser user in AllUsers)
            {
                int TotalPoints = GetTotalPoints(user);

                Leaderboard.Add(user.Email.ToString(), TotalPoints);
            }

            Leaderboard.OrderByDescending(n => n.Value);

            return Leaderboard;
        }

        public List<Comment> GetAllComments()
        {
            var query = from a in context.Acts select a;
            return query.SelectMany(acts => acts.Comments).ToList();
        }

        public List<Comment> GetAllComments(int ActId)
        {
            var query = from a in context.Acts where a.RandomActId == ActId select a;
            return query.SelectMany(acts => acts.Comments).ToList();
        }

        public bool DeleteComment(int ActId, int comment_id)
        {
            var query = from a in context.Acts where a.RandomActId == ActId select a;
            RandomActsModel target_act = null;
            bool result = true;

            try
            {
                target_act = query.SingleOrDefault<RandomActsModel>();
                target_act.Comments.RemoveAll(t => t.CommentId == comment_id);
                context.SaveChanges();
                result = true;
            }
            catch (ArgumentException)
            {
                result = false;
            }

            return result;
        }

        public bool CreateComment(Comment comm_2_add, int ActId)
        {
            var query = from r in context.Acts where r.RandomActId == ActId select r;
            RandomActsModel found_act = null;
            bool result = true;

            try
            {
                found_act = query.Single();
                found_act.Comments.Add(comm_2_add);
                context.SaveChanges();
            }
            catch (InvalidOperationException)
            {
                result = false;
            }
            catch (ArgumentNullException)
            {
                result = false;
            }

            return result;
        }

        public bool UpdateComment(int comment_id, string new_text)
        {
            var query = from a in context.Comments where a.CommentId == comment_id select a;
            Comment target_comment = null;
            bool result = true;

            try
            {
                target_comment = query.SingleOrDefault<Comment>();
                target_comment.UserComment = new_text;
                context.SaveChanges();
                result = true;
            }
            catch (ArgumentNullException)
            {
                result = false;
            }
            catch (InvalidOperationException)
            {
                result = false;
            }

            return result;
        }

        public int GetLikeCount(int act_id)
        {
            var query = from a in context.Likes where a.Act.RandomActId == act_id select a;
            int result = 0;
            List<Likes> current_num_of_likes = new List<Likes>();

            try
            {
                current_num_of_likes = query.ToList();
                result = current_num_of_likes.Count;
            }
            catch (ArgumentException)
            {
                result = -1;
            }

            return result;
        }

        public Likes CreateLike(RandomActsModel act, ApplicationUser UserWhoLiked)
        {
            Likes create_me = new Likes { Act = act, User = UserWhoLiked };
            context.Likes.Add(create_me);
            context.SaveChanges();

            int add_to_points = AddLikePts(act, UserWhoLiked);
            return create_me;
        }

        public bool DeleteLike(int like_id)
        {
            var query = from a in context.Likes where a.LikeId == like_id select a;
            Likes target_like = null;
            bool result = true;

            try
            {
                target_like = query.Single<Likes>();
                context.Likes.Remove(target_like);
                context.SaveChanges();
                result = true;
            }
            catch (ArgumentException)
            {
                result = false;
            }

            return result;
        }

        public int GetTotalPoints(ApplicationUser user1)
        {
            var query = from a in context.Acts where a.Owner.Id == user1.Id select a;

            List<int> UserActPoints = new List<int>();
            UserActPoints = query.Select(acts => acts.PointsEarned).ToList();

            return UserActPoints.Sum();
        }

        public int AddLikePts(RandomActsModel act, ApplicationUser who_liked)
        {
            var query = from a in context.Acts where a.RandomActId == act.RandomActId select a;
            Rank GetRank = GetUserRank(who_liked);
            int points_to_add = GetRank.BasePtsAllowance;
            RandomActsModel selected_act = null;

            try
            {
                selected_act = query.SingleOrDefault<RandomActsModel>();
                selected_act.PointsEarned = selected_act.PointsEarned + points_to_add;
                context.SaveChanges();
            }
            catch (ArgumentException)
            {
                return -1;
            }

            return selected_act.PointsEarned;
        }
    }

}