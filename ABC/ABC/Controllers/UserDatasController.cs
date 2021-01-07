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

namespace ABC.Controllers
{
    [CustomAuthenticationFilter]
    public class UserDatasController : Controller
    {
        private ThakshilawaEntities2 db = new ThakshilawaEntities2();

        // GET: UserDatas
        [CustomAuthorize("UserData")]
        public ActionResult Index()
        {
            return View(db.UserDatas.ToList());
        }

        // GET: UserDatas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserData userData = db.UserDatas.Find(id);
            if (userData == null)
            {
                return HttpNotFound();
            }
            return View(userData);
        }

        // GET: UserDatas/Create
        public ActionResult Create()
        {
            ViewBag.type = new SelectList(getUsertypes());
            return View();
        }

        // POST: UserDatas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,userid,password,mobile,email,type")] UserData userData)
        {
            if (ModelState.IsValid)
            {
                db.UserDatas.Add(userData);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userData);
        }

        // GET: UserDatas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserData userData = db.UserDatas.Find(id);
            if (userData == null)
            {
                return HttpNotFound();
            }
            return View(userData);
        }

        // POST: UserDatas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,userid,password,mobile,email,type")] UserData userData)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userData).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userData);
        }

        // GET: UserDatas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserData userData = db.UserDatas.Find(id);
            if (userData == null)
            {
                return HttpNotFound();
            }
            return View(userData);
        }

        // POST: UserDatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserData userData = db.UserDatas.Find(id);
            db.UserDatas.Remove(userData);
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
        List<String> getUsertypes() {
            List<String> list = new List<String>();
            list.Add("admin");
            list.Add("clerk ");
            list.Add("lecturer");
            list.Add("cafeteria");
            return list;
        }
    }
}
