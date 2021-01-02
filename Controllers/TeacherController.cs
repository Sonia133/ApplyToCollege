using College.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace College.Controllers
{
    public class TeacherController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Teacher
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Index(int? id)
        {
            Faculty faculty = db.Faculties.Find(id);
            ViewBag.Faculty = faculty;

            List<Teacher> teachers = faculty.Teachers.ToList();
            ViewBag.Teachers = teachers;

            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id.HasValue)
            {
                Teacher teacher = db.Teachers.Find(id);
                if (teacher != null)
                {
                    return View(teacher);
                }
                return HttpNotFound("We couldn't find a teacher with this id!");
            }
            return HttpNotFound("Please introduce an id for the teacher!");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult New(int id)
        {
            Teacher teacher = new Teacher
            {
                Faculty = db.Faculties.Find(id)
            };
            return View(teacher);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult New(int id, Teacher teacher)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Faculty faculty = db.Faculties.Find(id);

                    faculty.Teachers.Add(new Teacher
                    {
                        Name = teacher.Name,
                        Subject = teacher.Subject,
                        Email = teacher.Email
                    });
                    db.SaveChanges();

                    return RedirectToAction("Index", "Teacher", new { id = faculty.FacultyId });
                }
                else
                {
                    var errors = ModelState.Select(x => x.Value.Errors)
                               .Where(y => y.Count > 0)
                               .ToList();
                }
                return View(teacher);
            }
            catch (Exception e)
            {
                return View(teacher);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                Teacher teacher = db.Teachers.Find(id);
                if (teacher != null)
                {
                    return View(teacher);
                }
                return HttpNotFound("We couldn't find a teacher with this id!");
            }
            return HttpNotFound("Please introduce an id for the teacher!");
        }

        [HttpPost]
        public ActionResult Edit(Teacher teacher)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var oldTeacher = db.Teachers.Find(teacher.TeacherId);

                    if (oldTeacher == null)
                    {
                        return HttpNotFound();
                    }

                    oldTeacher.Name = teacher.Name;
                    oldTeacher.Subject = teacher.Subject;
                    oldTeacher.Email = teacher.Email;

                    TryUpdateModel(oldTeacher);

                    db.SaveChanges();

                    return RedirectToAction("Details", "Teacher", new { id = oldTeacher.TeacherId });
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
            return View(teacher);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Delete(int id)
        {
            Teacher teacher = db.Teachers.Find(id);

            List<Faculty> faculties = db.Faculties.Include("Teachers").ToList();
            int idFaculty = -1;

            faculties.ForEach((faculty) =>
            {
                List<Teacher> teachers = faculty.Teachers.ToList();
                if (teachers.FirstOrDefault(x => x.TeacherId.Equals(id)) != null)
                {
                    idFaculty = faculty.FacultyId;
                }
            });

            if (teacher != null)
            {
                db.Teachers.Remove(teacher);
                db.SaveChanges();
                return RedirectToAction("Index", "Teacher", new { id = idFaculty });
            }
            return HttpNotFound("I couldn't find an exam with this id!");
        }
    }
}