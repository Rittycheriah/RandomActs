﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Data.Entity;
using System.Threading;

namespace Nirvana.Models
{
    public class NirvanaRepository : INirvanaRepository
    {
        public NirvanaContext context { set; get; }

        public IDbSet<ApplicationUser> Users { get { return context.Users; } }

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
            return context.Acts.OrderByDescending(a => a.Date).ToList();
        }

        public List<RandomActsModel> GetProfileActs(ApplicationUser _user)
        {
            return context.Acts.Where(acts => acts.Owner.Id != _user.Id).OrderByDescending(a => a.Date).ToList();
        }

        public List<RandomActsModel> GetAllActs(ApplicationUser _user)
        {
            return context.Acts.Where(a => a.Owner.Id == _user.Id).OrderByDescending(a => a.Date).ToList();
        }

        public RandomActsModel GetActById(int act_id)
        {
            var ActIdQuery = from act in context.Acts where act.RandomActId == act_id select act;
            return ActIdQuery.Single();
        }

        public RandomActsModel CreateAct(string ActTitle, string ActDescription, ApplicationUser owner)
        {
            RandomActsModel _act = new RandomActsModel { RandomActTitle = ActTitle, RandomActDescription = ActDescription, Date = DateTime.Now, Owner = owner, PointsEarned = 3 };
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

            if (TotalPts <= 10 && TotalPts >= 0)
            {
                var query = context.Definitions.Where(n => n.RankingCode == 1);
                this_rank = query.First();
            }
            else if (TotalPts > 10 && TotalPts <= 20)
            {
                var query = context.Definitions.Where(n => n.RankingCode == 2);
                this_rank = query.First();
            }
            else if (TotalPts > 20 && TotalPts <= 30)
            {
                var query = context.Definitions.Where(n => n.RankingCode == 3);
                this_rank = query.First();
            }
            else if (TotalPts > 30 && TotalPts <= 40)
            {
                var query = context.Definitions.Where(n => n.RankingCode == 4);
                this_rank = query.First();
            }
            else if (TotalPts > 40 && TotalPts <= 60)
            {
                var query = context.Definitions.Where(n => n.RankingCode == 5);
                this_rank = query.First();
            }
            else if (TotalPts > 60 && TotalPts <= 75)
            {
                var query = context.Definitions.Where(n => n.RankingCode == 6);
                this_rank = query.First();
            }
            else if (TotalPts > 75 && TotalPts <= 100)
            {
                var query = context.Definitions.Where(n => n.RankingCode == 7);
                this_rank = query.First();
            }
            else if (TotalPts > 100 && TotalPts <= 200)
            {
                var query = context.Definitions.Where(n => n.RankingCode == 8);
                this_rank = query.First();
            }

            UserRank.Name = this_rank.RankingName;
            UserRank.BasePtsAllowance = this_rank.RankingBasePts;
            UserRank.CommentFeat = this_rank.RankingComments;
            UserRank.MinimumPtReq = this_rank.RankingMinPt;
            UserRank.SocialMedia = this_rank.RankingSocial;
            context.SaveChanges();

            return UserRank;

        }

        public IOrderedEnumerable<KeyValuePair<string, int>> GetAllUsersRanks()
        {
            List<ApplicationUser> AllUsers = context.Users.ToList();
            Dictionary<string, int> Leaderboard = new Dictionary<string, int>();

            foreach (ApplicationUser user in AllUsers)
            {
                int TotalPoints = GetTotalPoints(user);

                Leaderboard.Add(user.Email.ToString(), TotalPoints);
            }

            var sortLeaderboard = from v in Leaderboard orderby v.Value descending select v;

            return sortLeaderboard;
        }

        public List<Comment> GetAllComments()
        {
            var query = from a in context.Acts select a;
            return query.SelectMany(acts => acts.Comments).ToList();
        }

        public List<Comment> GetAllComments(int ActId)
        {
            var query = from a in context.Acts where a.RandomActId == ActId select a;
            return query.SelectMany(acts => acts.Comments).OrderByDescending(c => c.Date).ThenBy(c => c.Date.Minute).ToList();
        }

        public bool DeleteComment(int comment_id)
        {
            var query = from a in context.Comments where a.CommentId == comment_id select a;
            Comment target_comment = null;
            bool result = true;

            try
            {
                target_comment = query.Single<Comment>();
                context.Comments.Remove(target_comment);
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
            bool result = true;

            try
            {
                var target_comment = context.Comments.Single(c => c.CommentId == comment_id);
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

        public bool CheckLikes(RandomActsModel act, ApplicationUser User)
        {
            bool has_liked = false;



            List<Likes> found = context.Likes.Where(l => l.Act.RandomActId == act.RandomActId && l.User.Id == User.Id).ToList();

            if (found.Count >= 1)
            {
                has_liked = false;
            }
            else if (found.Count == 0)
            {
                has_liked = true;
            }

            return has_liked;
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
            catch (Exception e)
            {
                System.Console.WriteLine(e);
            }
            return selected_act.PointsEarned;
        }

        public List<string> SearchActs(string description)
        {
            try
            {
                var query = from acts in context.Acts select acts.RandomActTitle;
                List<string> found_acts = query.Where(a => a.Contains(description)).ToList();
                found_acts.Sort();
                return found_acts;
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException("Sorry nothing was found for your term");
            }
        }
    }

}