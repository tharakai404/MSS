using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ABC;

namespace ABC.Controllers
{
    public class StudentsController : Controller
    {
        private ThakshilawaEntities2 db = new ThakshilawaEntities2();



        public ActionResult Index(string name)
        {
            //var customerOrders = db.customerOrders.Include(c => c.customer);


            if (name == null) name = "";
            var userRoles = db.Students.Where(u => u.name.Contains(name));
           return PartialView(userRoles.ToList());
           // var students = db.Students.Include(s => s.UserData);
         //   return PartialView(students.ToList());
        }


        public ActionResult Search()
        {
            return View();
        }

  
        // GET: Students/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            ViewBag.createdby = new SelectList(db.UserDatas, "id", "name");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( [Bind(Include = "id,name,nic,address,regdate,regid,createdby,mobile")] Student student)
        {
            var userId = Convert.ToString(Session["UserId"]);
            student.createdby = int.Parse(userId);
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();
                 student = db.Students.Find(student.id);
                student.regid = "STD-" + student.id;
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Search");
            }

            ViewBag.createdby = new SelectList(db.UserDatas, "id", "name", student.createdby);
            return View(student);
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            ViewBag.createdby = new SelectList(db.UserDatas, "id", "name", student.createdby);
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,nic,address,regdate,regid,createdby,mobile")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.createdby = new SelectList(db.UserDatas, "id", "name", student.createdby);
            return View(student);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
            db.SaveChanges();
            return RedirectToAction("Search");
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
