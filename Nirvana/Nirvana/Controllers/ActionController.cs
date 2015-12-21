using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nirvana.Models;
using Microsoft.AspNet.Identity;

namespace Nirvana.Controllers
{
    public class ActionController : Controller
    {
        NirvanaRepository nirvana_repo = new NirvanaRepository();

        // GET: Action
        public ActionResult Index()
        {
            return View();
        }

        // GET: Action/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Action/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Action/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                string title = collection["ActTitle"].ToString();
                string description = collection["ActDescription"].ToString();
                string user_id = User.Identity.GetUserId();
                ApplicationUser current_user = nirvana_repo.Users.FirstOrDefault(u => u.Id == user_id);

                nirvana_repo.CreateAct(title, description, current_user);

                // action name, controller name for overload
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return View();
            }
        }

        // GET: Action/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Action/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Action/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Action/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
