using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ABC;
using ABC.Authentication;
using ABC.Models;
using ABC.Services;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace ABC.Controllers
{
    [CustomAuthenticationFilter]
    public class PaymentsController : Controller
    {
        private ThakshilawaEntities2 db = new ThakshilawaEntities2();
        AppServices appServices = new AppServices();

        // GET: Payments
        [CustomAuthorize("Payment")]
        public ActionResult Index()
        {
            var payments = db.Payments.Include(p => p.Enrollment).Include(p => p.UserData);
            return View(payments.ToList());
        }

        // GET: Payments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = db.Payments.Find(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            return View(payment);
        }

        // GET: Payments/Create
        public ActionResult Create()
        {
            ViewBag.enrolementid = new SelectList(getAllLectures(), "id", "name");
            ViewBag.createdby = new SelectList(db.UserDatas, "id", "name");
            ViewBag.type = new SelectList(getPaymentTypes());
            return View();
        }

        // POST: Payments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,enrolementid,amount,type,date,createdby")] Payment payment)
        {
            var userId = Convert.ToString(Session["UserId"]);
            payment.createdby = int.Parse(userId);
            if (ModelState.IsValid)
            {
                db.Payments.Add(payment);
                db.SaveChanges();
                return Redirect("Print/" + payment.id);
            }

            ViewBag.enrolementid = new SelectList(db.Enrollments, "id", "paymentdone", payment.enrolementid);
            ViewBag.createdby = new SelectList(db.UserDatas, "id", "name", payment.createdby);
            ViewBag.type = new SelectList(getPaymentTypes());
            return View(payment);
        }

        // GET: Payments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = db.Payments.Find(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            ViewBag.enrolementid = new SelectList(db.Enrollments, "id", "paymentdone", payment.enrolementid);
            ViewBag.createdby = new SelectList(db.UserDatas, "id", "name", payment.createdby);
            ViewBag.type = new SelectList(getPaymentTypes());
            return View(payment);
        }

        // POST: Payments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,enrolementid,amount,type,date,createdby")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(payment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.enrolementid = new SelectList(db.Enrollments, "id", "paymentdone", payment.enrolementid);
            ViewBag.createdby = new SelectList(db.UserDatas, "id", "name", payment.createdby);
            ViewBag.type = new SelectList(getPaymentTypes());
            return View(payment);
        }

        // GET: Payments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = db.Payments.Find(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            return View(payment);
        }

        // POST: Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Payment payment = db.Payments.Find(id);
            db.Payments.Remove(payment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult GetDuePayment()
        {
            List<DuePayDTO> list = new List<DuePayDTO>();
            DateTime now = DateTime.Now;
            var startDate = new DateTime(now.Year, now.Month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            var paymentDoneids = db.Payments.Where(a => a.date >= startDate && a.date <= endDate).Select(x => x.enrolementid).ToArray();

            List<Enrollment> employees = db.Enrollments.Where(p => !paymentDoneids.Contains(p.id)).ToList();
            list = (from pp in db.Enrollments.Where(p => !paymentDoneids.Contains(p.id))
                    select new DuePayDTO
                    {
                        id = pp.id,
                        name = pp.Student.name,
                        amount = pp.ClassRoom.coursefee,
                        classname = pp.ClassRoom.name
                    }).ToList();

           return View(list);
        }

        public ActionResult Print(int id)
        {

            MemoryStream workStream = new MemoryStream();
            StringBuilder status = new StringBuilder("");
            DateTime dTime = DateTime.Now;
            //file name to be created 
            string strPDFFileName = string.Format("SamplePdf" + dTime.ToString("yyyyMMdd") + "-" + ".pdf");
            Document doc = new Document();
            doc.SetMargins(0f, 0f, 0f, 0f);

            doc.SetMargins(100f, 100f, 100f, 100f);

            //Create PDF Table

            //file will created in this path
            string strAttachment = Server.MapPath("~/Downloadss/" + strPDFFileName);

            PdfWriter.GetInstance(doc, workStream).CloseStream = false;
            doc.Open();

            //Add Content to PDF 

            doc.Add(appServices.PrintPayment(id));

            // Closing the document
            doc.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            Response.ContentType = "application/pdf";
            Response.BinaryWrite(workStream.ToArray());
            return null;

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        List<String> getPaymentTypes()
        {
            List<String> list = new List<String>();
            list.Add("Card");
            list.Add("Cash");
            return list;
        }
        List<DataDTO> getAllLectures()
        {
            var role = (from dd in db.Enrollments
                        join pp in db.Students on dd.studentid equals pp.id
                      //  where dd.appuserid == userid

                        select new DataDTO
                        {
                            id = dd.id,
                            name = pp.name + " "+pp.regid +" "+dd.ClassRoom.name
                        }).Distinct().ToList();
       
            return role;
        }
    }
}
