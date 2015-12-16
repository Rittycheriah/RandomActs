using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Nirvana.Models;
using System.Web.Http;
using System.Web.Mvc;

namespace Nirvana.Controllers
{
    public class ActsController : ApiController
    {
        private NirvanaRepository nirvana_repo;

        public ActsController()
        {
            this.nirvana_repo = new NirvanaRepository();
        }

        public ActsController(NirvanaRepository nirvanarepo)
        {
            this.nirvana_repo = nirvanarepo;
        }

        // just end points to retrieve data. 
        // GET: api/Acts
        public HttpResponseMessage GetAllActs()
        {
            List<RandomActsModel> acts = nirvana_repo.GetAllActs();
            if (acts == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            return Request.CreateResponse(acts);
        }

        // GET: api/Acts/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Acts
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Acts/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Acts/5
        public void Delete(int id)
        {
        }
    }
}
