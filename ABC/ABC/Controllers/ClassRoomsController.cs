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
using ABC.Models;

namespace ABC.Controllers
{
    [CustomAuthenticationFilter]
    public class ClassRoomsController : Controller
    {
        private ThakshilawaEntities2 db = new ThakshilawaEntities2();

        // GET: ClassRooms
        [CustomAuthorize("ClassRoom")]
        public ActionResult Index()
        {
            var classRooms = db.ClassRooms.Include(c => c.Lecture).Include(c => c.UserData);
            return View(classRooms.ToList());
        }

        // GET: ClassRooms/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClassRoom classRoom = db.ClassRooms.Find(id);
            if (classRoom == null)
            {
                return HttpNotFound();
            }
            return View(classRoom);
        }

        // GET: ClassRooms/Create
        public ActionResult Create()
        {

            ViewBag.lectureid = new SelectList(getAllLectures(), "id", "name");          
            return View();
        }

        // POST: ClassRooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,commencementdate,maxstudent,coursefee,lectureid")] ClassRoom classRoom)
        {
            var userId = Convert.ToString(Session["UserId"]);
            classRoom.createdby = int.Parse(userId);
            if (ModelState.IsValid)
            {
                db.ClassRooms.Add(classRoom);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.lectureid = new SelectList(getAllLectures(), "id", "name");
            ViewBag.createdby = new SelectList(db.UserDatas, "id", "name", classRoom.createdby);
            return View(classRoom);
        }

        // GET: ClassRooms/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClassRoom classRoom = db.ClassRooms.Find(id);
            if (classRoom == null)
            {
                return HttpNotFound();
            }
            ViewBag.lectureid = new SelectList(db.Lectures, "id", "id", classRoom.lectureid);
            ViewBag.createdby = new SelectList(db.UserDatas, "id", "name", classRoom.createdby);
            return View(classRoom);
        }

        // POST: ClassRooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,commencementdate,maxstudent,coursefee,createdby,lectureid")] ClassRoom classRoom)
        {
            if (ModelState.IsValid)
            {
                db.Entry(classRoom).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.lectureid = new SelectList(db.Lectures, "id", "id", classRoom.lectureid);
            ViewBag.createdby = new SelectList(db.UserDatas, "id", "name", classRoom.createdby);
            return View(classRoom);
        }

        // GET: ClassRooms/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClassRoom classRoom = db.ClassRooms.Find(id);
            if (classRoom == null)
            {
                return HttpNotFound();
            }
            return View(classRoom);
        }

        // POST: ClassRooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ClassRoom classRoom = db.ClassRooms.Find(id);
            db.ClassRooms.Remove(classRoom);
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
       List<DataDTO>  getAllLectures() {
            var list = (from pp in db.Lectures
                        select new DataDTO
                        {
                            id = pp.id,
                            name = pp.UserData.name
                        }).ToList();
            return list;
        }
    }
}
