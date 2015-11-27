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
    }
}