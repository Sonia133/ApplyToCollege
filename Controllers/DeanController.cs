using College.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace College.Controllers
{
    public class DeanController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Dean
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Index(int? id)
        {
            Dean dean = db.Deans.Find(id);
            ViewBag.Dean = dean;

            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                Dean dean = db.Deans.Find(id);
                if (dean != null)
                {
                    return View(dean);
                }
                return HttpNotFound("We couldn't find a dean with this id!");
            }
            return HttpNotFound("Please introduce an id for the dean!");
        }

        [HttpPost]
        public ActionResult Edit(Dean dean)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var oldDean = db.Deans.Find(dean.DeanId);

                    if (oldDean == null)
                    {
                        return HttpNotFound();
                    }

                    oldDean.Name = dean.Name;
                    oldDean.Email = dean.Email;

                    TryUpdateModel(oldDean);

                    db.SaveChanges();

                    return RedirectToAction("Details", "Faculty", new { id = oldDean.Faculty.FacultyId });
                }
                else
                {
                    var errors = ModelState.Select(x => x.Value.Errors)
                               .Where(y => y.Count > 0)
                               .ToList();
                }
            }
            catch (Exception e)
            {
                return HttpNotFound();
            }
            return View(dean);
        }
    }
}