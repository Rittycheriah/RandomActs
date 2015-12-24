using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nirvana.Models;

namespace Nirvana.Controllers
{
    public class HomeController : Controller
    {
        private NirvanaRepository nirvana_repo;

        public HomeController()
        {
            nirvana_repo = new NirvanaRepository();
        }

        public HomeController (NirvanaRepository nirvana_Repo)
        {
            nirvana_repo = nirvana_Repo;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult add_Act()
        {
            var act = new RandomActsModel();
            return View(act);
        }

        public ActionResult UserProfile()
        {
            var acts_now = nirvana_repo.GetAllActs().ToList();

            return View(acts_now);
        }
    }
}