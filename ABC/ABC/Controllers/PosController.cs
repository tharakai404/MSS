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
using ABC.Services;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace ABC.Controllers
{
    [CustomAuthenticationFilter]
    public class PosController : Controller
    {
        private ThakshilawaEntities2 db = new ThakshilawaEntities2();
        AppServices appServices = new AppServices();

        // GET: Pos

        [CustomAuthorize("Pos")]
        public ActionResult Index()
        {
            var sellingItems = db.SellingItems.Include(s => s.UserData);
            return View(sellingItems.ToList());
        }

       // [HttpPost]
        public String Bill(SellingItemInvoice inv)
        {

            foreach (var item in inv.SellingItemInvoiceDetails)
            {
                SellingItem sellingItem = db.SellingItems.Find(item.sellingitemid);
                item.rate = item.qty * sellingItem.sellingrate;
                inv.amount += item.rate;
            }
            var userId = Convert.ToString(Session["UserId"]);
            inv.createdby = int.Parse(userId);
            inv.date = DateTime.Now;
            db.SellingItemInvoices.Add(inv);
            db.SaveChanges();


            return inv.id.ToString();
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

           doc.Add(appServices.PrintInvoice(id));
            
           

            // Closing the document
            doc.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            Response.ContentType = "application/pdf";
            Response.BinaryWrite(workStream.ToArray());
            return null;

        }

        // GET: Pos/Details/5
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

        // GET: Pos/Create
        public ActionResult Create()
        {
            ViewBag.createdby = new SelectList(db.UserDatas, "id", "name");
            return View();
        }

        // POST: Pos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,itemname,sellingrate,buyingrate,qty,createdby")] SellingItem sellingItem)
        {
            if (ModelState.IsValid)
            {
                db.SellingItems.Add(sellingItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.createdby = new SelectList(db.UserDatas, "id", "name", sellingItem.createdby);
            return View(sellingItem);
        }

        // GET: Pos/Edit/5
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

        // POST: Pos/Edit/5
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

        // GET: Pos/Delete/5
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

        // POST: Pos/Delete/5
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
