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
    public class AccessLevelUsersController : Controller
    {
        private ThakshilawaEntities2 db = new ThakshilawaEntities2();

        // GET: AccessLevelUsers
        public ActionResult Index()
        {
            var accessLevelUsers = db.AccessLevelUsers.Include(a => a.AccessLevel).Include(a => a.UserData);
            return View(accessLevelUsers.ToList());
        }

        // GET: AccessLevelUsers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccessLevelUser accessLevelUser = db.AccessLevelUsers.Find(id);
            if (accessLevelUser == null)
            {
                return HttpNotFound();
            }
            return View(accessLevelUser);
        }

        // GET: AccessLevelUsers/Create
        public ActionResult Create()
        {
            ViewBag.accesslevelid = new SelectList(db.AccessLevels, "id", "access");
            ViewBag.userid = new SelectList(db.UserDatas, "id", "name");
            return View();
        }

        // POST: AccessLevelUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,accesslevelid,userid")] AccessLevelUser accessLevelUser)
        {
            if (ModelState.IsValid)
            {
                db.AccessLevelUsers.Add(accessLevelUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.accesslevelid = new SelectList(db.AccessLevels, "id", "access", accessLevelUser.accesslevelid);
            ViewBag.userid = new SelectList(db.UserDatas, "id", "name", accessLevelUser.userid);
            return View(accessLevelUser);
        }

        // GET: AccessLevelUsers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccessLevelUser accessLevelUser = db.AccessLevelUsers.Find(id);
            if (accessLevelUser == null)
            {
                return HttpNotFound();
            }
            ViewBag.accesslevelid = new SelectList(db.AccessLevels, "id", "access", accessLevelUser.accesslevelid);
            ViewBag.userid = new SelectList(db.UserDatas, "id", "name", accessLevelUser.userid);
            return View(accessLevelUser);
        }

        // POST: AccessLevelUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,accesslevelid,userid")] AccessLevelUser accessLevelUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(accessLevelUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.accesslevelid = new SelectList(db.AccessLevels, "id", "access", accessLevelUser.accesslevelid);
            ViewBag.userid = new SelectList(db.UserDatas, "id", "name", accessLevelUser.userid);
            return View(accessLevelUser);
        }

        // GET: AccessLevelUsers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccessLevelUser accessLevelUser = db.AccessLevelUsers.Find(id);
            if (accessLevelUser == null)
            {
                return HttpNotFound();
            }
            return View(accessLevelUser);
        }

        // POST: AccessLevelUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AccessLevelUser accessLevelUser = db.AccessLevelUsers.Find(id);
            db.AccessLevelUsers.Remove(accessLevelUser);
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
