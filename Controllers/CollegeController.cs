﻿using College.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace College.Controllers
{
    public class CollegeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admitere
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult AddFaculty()
        {
            Faculty faculty = new Faculty
            {
                Students = new List<Student>()
            };
            return View(faculty);
        }

        [NonAction]
        private IEnumerable<SelectListItem> GetAllFaculties()
        {
            var selectList = new List<SelectListItem>();
            foreach (var faculty in db.Faculties.ToList())
            {
                selectList.Add(new SelectListItem
                {
                    Value = faculty.FacultyId.ToString(),
                    Text = faculty.Name
                });
            }
            return selectList;
        }

        [NonAction]
        private bool AlreadyApplied(ApplicationUser user)
        {
            Student student = db.Students.FirstOrDefault(predicate => predicate.Email.Equals(user.UserName));
            return student != null;
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Faculties()
        {
            List<Faculty> faculties = db.Faculties.ToList();

            var userId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == userId);

            Student student = db.Students.FirstOrDefault(x => x.Email == currentUser.UserName);
            if (student != null)
            {
                ViewBag.StudentId = student.StudentId;
            }

            ViewBag.Faculties = faculties;
            ViewBag.Role = User.IsInRole("User") == true ? "User" : "Admin";
            ViewBag.AllowApply = User.IsInRole("User") == true && AlreadyApplied(currentUser) == false ? true : false;
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult EditFaculty(int? id)
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
        public ActionResult EditFaculty(Faculty faculty)
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

                    return RedirectToAction("Details", new { id = oldFaculty.FacultyId });
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

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Dean(int? id)
        {
            Dean dean = db.Deans.Find(id);
            ViewBag.Dean = dean;
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult EditDean(int? id)
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
        public ActionResult EditDean(Dean dean)
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

                    return RedirectToAction("Details", new { id = oldDean.Faculty.FacultyId });
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
            return View(dean);
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
        public ActionResult EditStudent(int? id)
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
        public ActionResult EditStudent(Student student)
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

                    return RedirectToAction("Candidates", new { id = oldStudent.Faculty.FacultyId });
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

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Candidates(int? id)
        {
            List<Student> students = db.Faculties.Find(id).Students.ToList();
            ViewBag.Students = students;
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Exams(int? id)
        {
            Faculty faculty = db.Faculties.Find(id);
            ViewBag.Faculty = faculty;

            List<Exam> exams = faculty.Exam.ToList();
            ViewBag.Exams = exams;

            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult ExamDetails(int? id)
        {
            if (id.HasValue)
            {
                Exam exam = db.Exams.Find(id);
                if (exam != null)
                {
                    return View(exam);
                }
                return HttpNotFound("We couldn't find an exam with this id!");
            }
            return HttpNotFound("Please introduce an id for the exam!");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult AddExam(int id)
        {
            Exam exam = new Exam {
                Faculty = db.Faculties.Find(id)
            };
            return View(exam);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult AddExam(int id, Exam exam)
        {
            try
            {
                if (ModelState.IsValid)
                { 
                    Faculty faculty = db.Faculties.Find(id);

                    faculty.Exam.Add(new Exam
                    {
                        Subject = exam.Subject,
                        Date = exam.Date,
                        Type = exam.Type
                    });
                    db.SaveChanges();

                    return RedirectToAction("Exams", "College", new { id = faculty.FacultyId});
                }
                else
                {
                    var errors = ModelState.Select(x => x.Value.Errors)
                               .Where(y => y.Count > 0)
                               .ToList();
                    Console.WriteLine(errors);
                }
                return View(exam);
            }
            catch (Exception e)
            {
                return View(exam);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult AddDean()
        {
            Dean dean = new Dean { };
            return View(dean);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult AddStep_1(Faculty faculty)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Faculties.Add(faculty);
                    db.SaveChanges();
                    return RedirectToAction("AddDean");
                }
                return View(faculty);
            }
            catch (Exception e)
            {
                return View(faculty);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult AddStep_2(Dean dean)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Faculties.OrderByDescending(f => f.FacultyId).FirstOrDefault().Dean = dean;
                    db.SaveChanges();

                    return RedirectToAction("Faculties");
                }
                return View(dean);
            }
            catch (Exception e)
            {
                return View(dean);
            }
        }

        [Authorize(Roles="User")]
        [HttpGet]
        public ActionResult AddStudent()
        {
            Student student = new Student { };
            student.FacultiesList = GetAllFaculties();
            return View(student);
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public ActionResult Apply(Student student)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userId = User.Identity.GetUserId();
                    ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == userId);

                    int id = student.Faculty.FacultyId;
                    Faculty faculty = db.Faculties.FirstOrDefault(predicate => predicate.FacultyId.Equals(id));
                    faculty.Students.Add(new Student
                    {
                        Name = student.Name,
                        Cnp = student.Cnp,
                        Frequency = student.Frequency,
                        Email = currentUser.UserName,
                        Sat = student.Sat,
                        Badge = student.Badge
                    });
                    db.SaveChanges();

                    return RedirectToAction("Faculties");
                }
                return View(student);
            }
            catch (Exception e)
            {
                return View(student);
            }
        }
    }
}