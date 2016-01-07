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
using Newtonsoft.Json.Linq;
using System.Security.Principal;
using System.Threading;
using Newtonsoft.Json;

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
        public List<RandomActsModel> GetAllActs()
        {
            string user_id = User.Identity.GetUserId();

            ApplicationUser owner = nirvana_repo.context.Users.FirstOrDefault(u => u.Id == user_id);

            List<RandomActsModel> the_acts = new List<RandomActsModel>();

            the_acts = nirvana_repo.GetProfileActs(owner);

            return the_acts;
  
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
            Thread.Sleep(2000);

            string the_change = comment.UserComment;

            nirvana_repo.UpdateComment(id, the_change);
        }

        [Route("api/Acts/CurrentUserRank")]
        [HttpGet]
        public Rank GetRank()
        {
            string user_id = User.Identity.GetUserId();

            ApplicationUser logged_in_user = nirvana_repo.context.Users.FirstOrDefault(u => u.Id == user_id);

            Rank logged_in_user_rank = nirvana_repo.GetUserRank(logged_in_user);

            return logged_in_user_rank;
        }

        [Route("api/Acts/TotalUserPts")]
        [HttpGet]
        public int GetTotal()
        {
            string user_id = User.Identity.GetUserId();

            ApplicationUser logged_in_user = nirvana_repo.context.Users.FirstOrDefault(u => u.Id == user_id);

            int UserTotal = nirvana_repo.GetTotalPoints(logged_in_user);

            return UserTotal;
        }

        [Route("api/Acts/PostLike/{id}")]
        [HttpPost]
        public HttpStatusCode PostLike(int id)
        {
            string user_id = User.Identity.GetUserId();

            ApplicationUser logged_in = nirvana_repo.context.Users.FirstOrDefault(u => u.Id == user_id);
            RandomActsModel act = nirvana_repo.context.Acts.FirstOrDefault(a => a.RandomActId == id);

            if (nirvana_repo.CheckLikes(act, logged_in) == true)
            {
                Likes the_like = nirvana_repo.CreateLike(act, logged_in);
                return HttpStatusCode.OK;
            }

            return HttpStatusCode.BadRequest;
        }

        [Route("api/Acts/Leaderboard")]
        [HttpGet]
        public string Leaderboard()
        {
            IOrderedEnumerable<KeyValuePair<string, int>> leaderboard = nirvana_repo.GetAllUsersRanks();

            string json = JsonConvert.SerializeObject(leaderboard, Formatting.Indented);

            return json;
        }

        [Route("api/Acts/GetCurrentUserActs")]
        [HttpGet]
        public IEnumerable<RandomActsModel> MyActs()
        {
            string user_id = User.Identity.GetUserId();

            ApplicationUser owner = nirvana_repo.context.Users.FirstOrDefault(u => u.Id == user_id);

            List<RandomActsModel> the_acts = nirvana_repo.GetAllActs(owner);

            return the_acts;
        }

        [Route("api/Acts/Search/")]
        [HttpGet]
        public List<RandomActsModel> GetSearchActs()
        {
            return nirvana_repo.GetAllActs();
        }
    }
}
