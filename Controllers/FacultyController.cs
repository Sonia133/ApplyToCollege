using College.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace College.Controllers
{
    public class FacultyController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admitere
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Index(string city)
        {
            List<Faculty> faculties = new List<Faculty>();

            if (city != null)
            {
                faculties = db.Faculties.Where(fac => fac.City == city).ToList();
            }
            else faculties = db.Faculties.ToList();

            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == userId);

                Student student = db.Students.FirstOrDefault(x => x.Email == currentUser.UserName);
                if (student != null)
                {
                    ViewBag.StudentId = student.StudentId;
                }

                if (User.IsInRole("User"))
                {
                    ViewBag.Apply = db.Students.FirstOrDefault(x => x.Email.Equals(currentUser.UserName));
                }
            }

            ViewBag.Faculties = faculties;

            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id.HasValue)
            {
                Faculty faculty = db.Faculties.Find(id);
                if (faculty != null)
                {
                    return View(faculty);
                }
                return HttpNotFound("We couldn't find a faculty with this id!");
            }
            return HttpNotFound("Please introduce an id for the faculty!");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult New()
        {
            FacultyDean faculty = new FacultyDean { };
            return View(faculty);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult New(FacultyDean faculty)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Dean dean = new Dean
                    {
                        Name = faculty.DeanName,
                        Email = faculty.Email
                    };

                    db.Deans.Add(dean);

                    db.Faculties.Add(new Faculty
                    {
                        Name = faculty.Name,
                        City = faculty.City,
                        Places = faculty.Places,
                        Description = faculty.Description,
                        Dean = dean
                    });

                    db.SaveChanges();
                    return RedirectToAction("Index", "Faculty");
                }
                else
                {
                    var errors = ModelState.Select(x => x.Value.Errors)
                               .Where(y => y.Count > 0)
                               .ToList();

                    ViewBag.Message = errors;
                    return View(faculty);
                }
            }
            catch (Exception e)
            {
                return View(faculty);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                Faculty faculty = db.Faculties.Find(id);
                if (faculty != null)
                {
                    return View(faculty);
                }
                return HttpNotFound("We couldn't find a faculty with this id!");
            }
            return HttpNotFound("Please introduce an id for the faculty!");
        }

        [HttpPost]
        public ActionResult Edit(Faculty faculty)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var oldFaculty = db.Faculties.Find(faculty.FacultyId);

                    if (oldFaculty == null)
                    {
                        return HttpNotFound();
                    }

                    oldFaculty.Name = faculty.Name;
                    oldFaculty.City = faculty.City;
                    oldFaculty.Places = faculty.Places;
                    oldFaculty.Description = faculty.Description;

                    TryUpdateModel(oldFaculty);

                    db.SaveChanges();

                    return RedirectToAction("Details", "Faculty", new { id = oldFaculty.FacultyId });
                }
                else
                {
                    var errors = ModelState.Select(x => x.Value.Errors)
                               .Where(y => y.Count > 0)
                               .ToList();
                    Console.WriteLine(errors);
                }
            }
            catch (Exception e)
            {
                return HttpNotFound();
            }
            return View(faculty);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Delete(int id)
        {
            Faculty faculty = db.Faculties.Find(id);
            Dean dean = faculty.Dean;

            if (faculty != null)
            {
                List<Teacher> teachers = db.Teachers.Where(t => t.Faculty.FacultyId == faculty.FacultyId).ToList();
                teachers.ForEach((teacher) =>
                {
                    db.Teachers.Remove(teacher);
                });

                List<Exam> exams = db.Exams.Where(e => e.Faculty.FacultyId == faculty.FacultyId).ToList();
                exams.ForEach((exam) =>
                {
                    db.Exams.Remove(exam);
                });

                faculty.Dean = null;
                dean.Faculty = null;

                db.Faculties.Remove(faculty);
                db.Deans.Remove(dean);

                db.SaveChanges();

                return RedirectToAction("Index", "Faculty");
            }
            return HttpNotFound("I couldn't find a faculty with this id!");
        }
    }
}