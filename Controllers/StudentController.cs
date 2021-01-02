using College.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace College.Controllers
{
    public class StudentController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Student
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Index(int? id)
        {
            Faculty faculty = db.Faculties.Find(id);
            ViewBag.Faculty = faculty;

            List<Student> students = faculty.Students.ToList();
            ViewBag.Students = students;

            return View();
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id.HasValue)
            {
                Student student = db.Students.Find(id);
                if (student != null)
                {
                    return View(student);
                }
                return HttpNotFound("I couldn't find your profile!");
            }
            return HttpNotFound("Please introduce an id for your profile!");
        }

        [NonAction]
        private List<Checkbox> GetAllFaculties()
        {
            var checkBoxList = new List<Checkbox>();
            foreach (var faculty in db.Faculties.ToList())
            {
                checkBoxList.Add(new Checkbox
                {
                    Id = faculty.FacultyId,
                    Name = faculty.Name,
                    Checked = false
                });
            }
            return checkBoxList;
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        public ActionResult New()
        {
            Student student = new Student { };
            student.FacultiesList = GetAllFaculties();
            student.Faculties = new List<Faculty>();

            return View(student);
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public ActionResult New(Student student)
        {
            try
            {
                var selectedFaculties = student.FacultiesList.Where(f => f.Checked).ToList();

                if (ModelState.IsValid)
                {
                    var userId = User.Identity.GetUserId();
                    ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == userId);

                    student.Faculties = new List<Faculty>();

                    for (int i = 0; i < selectedFaculties.Count(); i++)
                    {
                        Faculty faculty = db.Faculties.Find(selectedFaculties[i].Id);

                        student.Faculties.Add(faculty);
                    }

                    db.Students.Add(new Student
                    {
                        Name = student.Name,
                        Cnp = student.Cnp,
                        Frequency = student.Frequency,
                        Email = currentUser.UserName,
                        Sat = student.Sat,
                        Badge = student.Badge,
                        Faculties = student.Faculties
                    });

                    db.SaveChanges();

                    return RedirectToAction("Index", "Faculty");
                }
                else
                {
                    var errors = ModelState.Select(x => x.Value.Errors)
                               .Where(y => y.Count > 0)
                               .ToList();
                    Console.WriteLine(errors);
                }
                return View(student);
            }
            catch (Exception e)
            {
                return View(student);
            }
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                Student student = db.Students.Find(id);
                if (student != null)
                {
                    return View(student);
                }
                return HttpNotFound("I couldn't find your profile!");
            }
            return HttpNotFound("Please introduce an id for your profile!");
        }

        [HttpPost]
        public ActionResult Edit(Student student)
        {
            try
            {
                ModelState.Remove("Faculty");

                if (ModelState.IsValid)
                {
                    var oldStudent = db.Students.Find(student.StudentId);

                    if (oldStudent == null)
                    {
                        return HttpNotFound();
                    }

                    oldStudent.Name = student.Name;
                    oldStudent.Sat = student.Sat;
                    oldStudent.Cnp = student.Cnp;
                    oldStudent.Frequency = student.Frequency;
                    oldStudent.Badge = student.Badge;

                    TryUpdateModel(oldStudent);

                    db.SaveChanges();

                    return RedirectToAction("Index", "Manage");
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
            return View(student);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            Student student = db.Students.Find(id);

            if (student != null)
            {
                db.Students.Remove(student);
                db.SaveChanges();
                return RedirectToAction("Index", "Manage");
            }
            return HttpNotFound("I couldn't find a student with this id!");
        }
    }
}