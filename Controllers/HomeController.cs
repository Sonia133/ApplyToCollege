using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace College.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("Index", "College");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Apply to faculty!";

            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}