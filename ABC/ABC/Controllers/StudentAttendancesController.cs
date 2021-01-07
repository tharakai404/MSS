using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ABC;
using ABC.Models;

namespace ABC.Controllers
{
    public class StudentAttendancesController : Controller
    {
        private ThakshilawaEntities2 db = new ThakshilawaEntities2();

        // GET: StudentAttendances
        public ActionResult Index()
        {   DateTime today = DateTime.Now;
            today = today.AddDays(-1);
            var studentAttendances = db.StudentAttendances.Include(s => s.Student).Include(s => s.UserData)
                                      .Where(a => a.date > today);
            return View(studentAttendances.ToList());
        }

        // GET: StudentAttendances/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            Enrollment env = db.Enrollments.Where(a => a.studentid ==id).FirstOrDefault();
            if (env == null)
            {
                return HttpNotFound();
            }
            return PartialView(env);
        }

        // GET: StudentAttendances/Create
        public ActionResult Create()
        {
            ViewBag.studentid = new SelectList(getAllStudents(), "id", "name");
            ViewBag.inout = new SelectList(getAttendenceTypes());
            return View();
        }

        // POST: StudentAttendances/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,studentid,inout")] StudentAttendance studentAttendance)
        {
            var userId = Convert.ToString(Session["UserId"]);
            studentAttendance.createdby = int.Parse(userId);
            studentAttendance.date = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.StudentAttendances.Add(studentAttendance);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.studentid = new SelectList(db.Students, "id", "name", studentAttendance.studentid);
            ViewBag.inout = new SelectList(getAttendenceTypes());
            return View(studentAttendance);
        }

        // GET: StudentAttendances/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentAttendance studentAttendance = db.StudentAttendances.Find(id);
            if (studentAttendance == null)
            {
                return HttpNotFound();
            }
            ViewBag.studentid = new SelectList(db.Students, "id", "name", studentAttendance.studentid);
            ViewBag.inout = new SelectList(getAttendenceTypes());
            return View(studentAttendance);
        }

        // POST: StudentAttendances/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,date,studentid,inout")] StudentAttendance studentAttendance)
        {
            var userId = Convert.ToString(Session["UserId"]);
            studentAttendance.createdby = int.Parse(userId);
            if (ModelState.IsValid)
            {
                db.Entry(studentAttendance).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.studentid = new SelectList(db.Students, "id", "name", studentAttendance.studentid);
            ViewBag.inout = new SelectList(getAttendenceTypes());
            return View(studentAttendance);
        }

        // GET: StudentAttendances/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentAttendance studentAttendance = db.StudentAttendances.Find(id);
            if (studentAttendance == null)
            {
                return HttpNotFound();
            }
            return View(studentAttendance);
        }

        // POST: StudentAttendances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StudentAttendance studentAttendance = db.StudentAttendances.Find(id);
            db.StudentAttendances.Remove(studentAttendance);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        List<String> getAttendenceTypes()
        {
            List<String> list = new List<String>();
            list.Add("IN");
            list.Add("OUT");
            return list;
        }

        List<DataDTO> getAllStudents()
        {
            var role = (from dd in db.Students
                        join pp in db.Enrollments on dd.id equals pp.studentid
                        select new DataDTO
                        {
                            id = dd.id,
                            name = dd.name + " - " + dd.regid
                        }).Distinct().ToList();

            return role;
        }
    }
    }
