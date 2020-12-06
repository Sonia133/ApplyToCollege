using College.Models;
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

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Faculties()
        {
            List<Faculty> faculties = db.Faculties.ToList();
            ViewBag.Faculties = faculties;
            ViewBag.User = User.IsInRole("User");
            ViewBag.Admin = User.IsInRole("Admin");
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

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Candidates(int? id)
        {
            List<Student> students = db.Faculties.Find(id).Students.ToList();
            ViewBag.Students = students;
            return View();
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
                    int id = student.Faculty.FacultyId;
                    Faculty faculty = db.Faculties.FirstOrDefault(predicate => predicate.FacultyId.Equals(id));
                    faculty.Students.Add(new Student
                    {
                        Name = student.Name,
                        Cnp = student.Cnp,
                        Frequency = student.Frequency,
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