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
    public class UserAttendancesController : Controller
    {
        private ThakshilawaEntities2 db = new ThakshilawaEntities2();

        // GET: UserAttendances
        public ActionResult Index()
        {
            DateTime today = DateTime.Now;
            today = today.AddDays(-1);
            var userAttendances = db.UserAttendances.Include(s => s.UserData)
                                      .Where(a => a.date > today).ToList();
            return View(userAttendances);
        }

        // GET: UserAttendances/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserAttendance userAttendance = db.UserAttendances.Find(id);
            if (userAttendance == null)
            {
                return HttpNotFound();
            }
            return View(userAttendance);
        }

        // GET: UserAttendances/Create
        public ActionResult Create()
        {
            ViewBag.inout = new SelectList(getAttendenceTypes());
            ViewBag.userid = new SelectList(getAllUsers(), "id", "name");
            return View();
        }

        // POST: UserAttendances/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,date,inout,userid")] UserAttendance userAttendance)
        {
            var userId = Convert.ToString(Session["UserId"]);
            userAttendance.createdby = int.Parse(userId);
            userAttendance.date = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.UserAttendances.Add(userAttendance);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.inout = new SelectList(getAttendenceTypes());
            ViewBag.userid = new SelectList(getAllUsers(), "id", "name", userAttendance.userid);
            return View(userAttendance);
        }

        // GET: UserAttendances/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserAttendance userAttendance = db.UserAttendances.Find(id);
            if (userAttendance == null)
            {
                return HttpNotFound();
            }
             ViewBag.inout = new SelectList(getAttendenceTypes());
            ViewBag.userid = new SelectList(getAllUsers(), "id", "name", userAttendance.userid);
            return View(userAttendance);
        }

        // POST: UserAttendances/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,date,inout,userid")] UserAttendance userAttendance)
        {
            var userId = Convert.ToString(Session["UserId"]);
            userAttendance.createdby = int.Parse(userId);
            userAttendance.date = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Entry(userAttendance).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.inout = new SelectList(getAttendenceTypes());
            ViewBag.userid = new SelectList(getAllUsers(), "id", "name", userAttendance.userid);
            return View(userAttendance);
        }

        // GET: UserAttendances/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserAttendance userAttendance = db.UserAttendances.Find(id);
            if (userAttendance == null)
            {
                return HttpNotFound();
            }
            return View(userAttendance);
        }

        // POST: UserAttendances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserAttendance userAttendance = db.UserAttendances.Find(id);
            db.UserAttendances.Remove(userAttendance);
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

        List<DataDTO> getAllUsers()
        {
            var role = (from dd in db.UserDatas                       
                        select new DataDTO
                        {
                            id = dd.id,
                            name = dd.name + " - " + dd.type
                        }).Distinct().ToList();

            return role;
        }
    }
}
