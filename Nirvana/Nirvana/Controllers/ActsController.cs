using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Nirvana.Models;
using System.Web.Http;
using System.Web.Http.Results;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;


namespace Nirvana.Controllers
{
    public class ActsController : ApiController
    {
        private INirvanaRepository nirvana_repo;

        public ActsController()
        {
            this.nirvana_repo = new NirvanaRepository();
        }
        
        public ActsController(INirvanaRepository int_nirvanarepo)
        {
            this.nirvana_repo = int_nirvanarepo;
        }

        // just end points to retrieve data. 
        // GET: api/Acts
        [Route("api/GetAllActs")]
        [HttpGet]
        public IEnumerable<RandomActsModel> GetAllActs()
        {
            return nirvana_repo.GetAllActs();
        }

        // GET: api/Acts/5
        [Route("api/GetActs")]
        [HttpGet]
        public HttpResponseMessage GetUserAct(ApplicationUser user1)
        {
            IEnumerable<RandomActsModel> acts = nirvana_repo.GetAllActs(user1);
            if (acts == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            return Request.CreateResponse(acts);
        }

        // POST: api/Acts
        [Route("api/Acts")]
        [HttpPost]
        public RandomActsModel Post(RandomActsModel new_act)
        {
            string act_title = new_act.RandomActTitle;
            string act_description = new_act.RandomActDescription;

            string user_id = User.Identity.GetUserId();

            ApplicationUser owner = nirvana_repo.Users.FirstOrDefault(u => u.Id == user_id);

            RandomActsModel current = nirvana_repo.CreateAct(act_title, act_description, owner);

            if (current == null)
            {
                throw new ArgumentNullException();
            }

            return current;
        }

    }
}
