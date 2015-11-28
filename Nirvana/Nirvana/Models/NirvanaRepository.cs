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
            RandomActsModel _act = new RandomActsModel { RandomActTitle = ActTitle, RandomActDescription = ActDescription, Date = ActDate, Owner = owner };
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
            throw new NotImplementedException();
        }

        public bool DeleteComment(int comment_id)
        {
            throw new NotImplementedException();
        }

        public Comment CreateComment(string curr_comment, ApplicationUser user, DateTime date)
        {
            throw new NotImplementedException();
        }

        public bool UpdateComment(int comment_id)
        {
            throw new NotImplementedException();
        }

        public bool GetLikes(int act_id)
        {
            throw new NotImplementedException();
        }

        public int GetLikeCount(int act_id)
        {
            throw new NotImplementedException();
        }
    }

}