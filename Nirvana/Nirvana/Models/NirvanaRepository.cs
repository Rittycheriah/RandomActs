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
            //var query = from b in context.Ranks where b.User.Id == user.Id select b;
            //return query.Single<Rank>();
            throw new NotImplementedException();
        }

        public List<Rank> GetAllUsersRanks()
        {
            throw new NotImplementedException();
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
    }

}