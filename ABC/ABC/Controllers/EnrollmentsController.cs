using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ABC;
using ABC.Authentication;
using ABC.Services;

namespace ABC.Controllers
{
    [CustomAuthenticationFilter]
    public class EnrollmentsController : Controller
    {
        private ThakshilawaEntities2 db = new ThakshilawaEntities2();
        private AppServices appService = new AppServices();

        // GET: Enrollments
        [CustomAuthorize("Enrollment")]
        public ActionResult Index()
        {
            var enrollments = db.Enrollments.Include(e => e.ClassRoom).Include(e => e.Student).Include(e => e.UserData);
            return View(enrollments.ToList());
        }

        // GET: Enrollments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrollment enrollment = db.Enrollments.Find(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            return View(enrollment);
        }

        // GET: Enrollments/Create
        public ActionResult Create()
        {
            ViewBag.classid = new SelectList(db.ClassRooms, "id", "name");
            ViewBag.studentid = new SelectList(db.Students, "id", "name");
            ViewBag.createdby = new SelectList(db.UserDatas, "id", "name");
            return View();
        }

        // POST: Enrollments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,studentid,classid,date,createdby,paymentdone")] Enrollment enrollment)
        {
            ViewBag.classid = new SelectList(db.ClassRooms, "id", "name", enrollment.classid);
            ViewBag.studentid = new SelectList(db.Students, "id", "name", enrollment.studentid);
            ViewBag.createdby = new SelectList(db.UserDatas, "id", "name", enrollment.createdby);

            var userId = Convert.ToString(Session["UserId"]);
            enrollment.createdby = int.Parse(userId);
            ClassRoom clz = db.ClassRooms.Find(enrollment.classid);
            if (enrollment.date> clz.commencementdate) {
                ModelState.AddModelError("date", "Check your enrollment date");
            }
            if (ModelState.IsValid)
            {
                db.Enrollments.Add(enrollment);
                db.SaveChanges();
                enrollment = db.Enrollments.Find(enrollment.id);
                Lecture lec = db.Lectures.Find(clz.id);
                UserData user = db.UserDatas.Find(lec.userid);
                appService.sendMail(user.email, user.name, "New student assigned", "reg date " + enrollment.date);

                return RedirectToAction("Index");
            }


            return View(enrollment);
        }

        // GET: Enrollments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrollment enrollment = db.Enrollments.Find(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            ViewBag.classid = new SelectList(db.ClassRooms, "id", "name", enrollment.classid);
            ViewBag.studentid = new SelectList(db.Students, "id", "name", enrollment.studentid);
            ViewBag.createdby = new SelectList(db.UserDatas, "id", "name", enrollment.createdby);
            return View(enrollment);
        }

        // POST: Enrollments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,studentid,classid,date,createdby,paymentdone")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(enrollment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.classid = new SelectList(db.ClassRooms, "id", "name", enrollment.classid);
            ViewBag.studentid = new SelectList(db.Students, "id", "name", enrollment.studentid);
            ViewBag.createdby = new SelectList(db.UserDatas, "id", "name", enrollment.createdby);
            return View(enrollment);
        }

        // GET: Enrollments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrollment enrollment = db.Enrollments.Find(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            return View(enrollment);
        }

        // POST: Enrollments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Enrollment enrollment = db.Enrollments.Find(id);
            db.Enrollments.Remove(enrollment);
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
    }
}
