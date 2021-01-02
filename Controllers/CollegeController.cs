using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace College.Controllers
{
    public class CollegeController : Controller
    {
        // GET: College
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Faculty");
        }
    }
}