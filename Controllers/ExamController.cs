using College.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace College.Controllers
{
    public class ExamController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Exam
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Index(int? id)
        {
            Faculty faculty = db.Faculties.Find(id);
            ViewBag.Faculty = faculty;

            List<Exam> exams = faculty.Exam.ToList();
            ViewBag.Exams = exams;

            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Details(int? id)
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
        public ActionResult New(int id)
        {
            Exam exam = new Exam
            {
                Faculty = db.Faculties.Find(id)
            };
            return View(exam);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult New(int id, Exam exam)
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

                    return RedirectToAction("Index", "Exam", new { id = faculty.FacultyId });
                }
                else
                {
                    var errors = ModelState.Select(x => x.Value.Errors)
                               .Where(y => y.Count > 0)
                               .ToList();
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
        public ActionResult Edit(int? id)
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

        [HttpPost]
        public ActionResult Edit(Exam exam)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var oldExam = db.Exams.Find(exam.ExamId);

                    if (oldExam == null)
                    {
                        return HttpNotFound();
                    }

                    oldExam.Subject = exam.Subject;
                    oldExam.Date = exam.Date;
                    oldExam.Type = exam.Type;

                    TryUpdateModel(oldExam);

                    db.SaveChanges();

                    return RedirectToAction("Details", "Exam", new { id = oldExam.ExamId });
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
            return View(exam);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Delete(int id)
        {
            Exam exam = db.Exams.Find(id);

            List<Faculty> faculties = db.Faculties.Include("Exam").ToList();
            int idFaculty = -1;

            faculties.ForEach((faculty) =>
            {
                List<Exam> exams = faculty.Exam.ToList();
                if (exams.FirstOrDefault(x => x.ExamId.Equals(id)) != null)
                {
                    idFaculty = faculty.FacultyId;
                }
            });

            if (exam != null)
            {
                db.Exams.Remove(exam);
                db.SaveChanges();
                return RedirectToAction("Index", "Exam", new { id = idFaculty });
            }
            return HttpNotFound("I couldn't find an exam with this id!");
        }
    }
}