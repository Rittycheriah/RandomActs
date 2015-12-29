﻿using System;
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
using Newtonsoft.Json.Linq;
using System.Security.Principal;

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

        [Route("api/Acts/GetCurrentUser")]
        [HttpGet]
        public string GetUser()
        {
            return RequestContext.Principal.Identity.Name.ToString();
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
        public RandomActsModel Post([FromBody]RandomActsModel new_act)
        {
            string act_title = new_act.RandomActTitle;
            string act_description = new_act.RandomActDescription;

            string user_id = User.Identity.GetUserId();

            ApplicationUser owner = nirvana_repo.context.Users.FirstOrDefault(u => u.Id == user_id);

            RandomActsModel current = nirvana_repo.CreateAct(act_title, act_description, owner);

            if (current == null)
            {
                throw new ArgumentNullException();
            }

            return current;
        }

        [Route("api/Acts/{id}")]
        [HttpPost]
        public void Post(int id, [FromBody]Comment NewComment)
        {

            var userID = User.Identity.GetUserId();
            ApplicationUser owner = nirvana_repo.Users.FirstOrDefault(u => u.Id == userID);

            Comment new_comment = new Comment { UserComment = NewComment.UserComment, ActId = id, Date = DateTime.Now, User = owner };

            try
            {
                nirvana_repo.CreateComment(new_comment, id);
            }
            catch
            {
                throw new ArgumentException();
            }
        }

        [Route("api/Acts/GetComms/{id}")]
        [HttpGet]
        public IEnumerable<Comment> GetCommentsForAct(int id)
        {
            return nirvana_repo.GetAllComments(id);
        }

        [Route("api/Acts/DeleteComm/{id}")]
        [HttpDelete]
        public void Delete(int id)
        {

            try
            {
                nirvana_repo.DeleteComment(id);
            }
            catch
            {
                throw new ArgumentException();
            }
        }

        [Route("api/Acts/EditComm/{id}")]
        [HttpPut]
        public void Edit(int id, [FromBody]Comment comment)
        {
            string the_change = comment.UserComment;

            try
            {
                nirvana_repo.UpdateComment(id, the_change);
            }
            catch
            {
                throw new ArgumentException();
            }
        }

    }
}
