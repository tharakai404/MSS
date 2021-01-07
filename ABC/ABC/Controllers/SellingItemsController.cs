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
    public class SellingItemsController : Controller
    {
        private ThakshilawaEntities2 db = new ThakshilawaEntities2();

        // GET: SellingItems
        public ActionResult Index()
        {
            var sellingItems = db.SellingItems.Include(s => s.UserData);
            return View(sellingItems.ToList());
        }

        // GET: SellingItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SellingItem sellingItem = db.SellingItems.Find(id);
            if (sellingItem == null)
            {
                return HttpNotFound();
            }
            return View(sellingItem);
        }

        // GET: SellingItems/Create
        public ActionResult Create()
        {
            ViewBag.createdby = new SelectList(db.UserDatas, "id", "name");
            return View();
        }

        // POST: SellingItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,itemname,sellingrate,buyingrate,qty")] SellingItem sellingItem)
        {
            var userId = Convert.ToString(Session["UserId"]);
            sellingItem.createdby = int.Parse(userId);
            if (ModelState.IsValid)
            {
                db.SellingItems.Add(sellingItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.createdby = new SelectList(db.UserDatas, "id", "name", sellingItem.createdby);
            return View(sellingItem);
        }

        // GET: SellingItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SellingItem sellingItem = db.SellingItems.Find(id);
            if (sellingItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.createdby = new SelectList(db.UserDatas, "id", "name", sellingItem.createdby);
            return View(sellingItem);
        }

        // POST: SellingItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,itemname,sellingrate,buyingrate,qty,createdby")] SellingItem sellingItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sellingItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.createdby = new SelectList(db.UserDatas, "id", "name", sellingItem.createdby);
            return View(sellingItem);
        }

        // GET: SellingItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SellingItem sellingItem = db.SellingItems.Find(id);
            if (sellingItem == null)
            {
                return HttpNotFound();
            }
            return View(sellingItem);
        }

        // POST: SellingItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SellingItem sellingItem = db.SellingItems.Find(id);
            db.SellingItems.Remove(sellingItem);
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
