﻿using System;
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
        public HttpResponseMessage GetAllActs()
        {
            IEnumerable<RandomActsModel> acts = nirvana_repo.GetAllActs();
            if (acts == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            return Request.CreateResponse(acts);
        }

        // GET: api/Acts/5
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
