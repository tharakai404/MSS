using ABC.Authentication;
using ABC.Models;
using ABC.Services;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ABC.Controllers
{
    [CustomAuthenticationFilter]
    public class ReportController : Controller
    {
        ReportService reportService = new ReportServiceImpl();

        private ThakshilawaEntities2 db = new ThakshilawaEntities2();

        // GET: Report
        [CustomAuthorize("Report")]
        public ActionResult Index()
        {
            return View();
        }
        [CustomAuthorize("Report")]
        public ActionResult PrintPDF()
        {

            ViewBag.name = new SelectList(reportService.GetReportType("normal"), "name", "name");
            return View();
        }
        [CustomAuthorize("Report")]
        public ActionResult PrintPDFDayWise()
        {
            ViewBag.name = new SelectList(reportService.GetReportType("daywise"), "name", "name");
            return View();
        }

        [CustomAuthorize("Report")]
        public ActionResult PrintPDFDayWiseAttendence()
        {
            ViewBag.name = new SelectList(db.ClassRooms.ToList(), "id", "name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize("Report")]
        public ActionResult PrintPDF(ReportDTO reportDTO)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.name = new SelectList(reportService.GetReportType("normal"), "name", "name");
                return View(reportDTO);
            }
          

            MemoryStream workStream = new MemoryStream();
            StringBuilder status = new StringBuilder("");
            DateTime dTime = DateTime.Now;
            //file name to be created 
            string strPDFFileName = string.Format("SamplePdf" + dTime.ToString("yyyyMMdd") + "-" + ".pdf");
            Document doc = new Document();
            doc.SetMargins(0f, 0f, 0f, 0f);
          
            doc.SetMargins(10f, 10f, 10f, 10f);
            //Create PDF Table

            //file will created in this path
            string strAttachment = Server.MapPath("~/Downloadss/" + strPDFFileName);


            PdfWriter.GetInstance(doc, workStream).CloseStream = false;
            doc.Open();

            //Add Content to PDF 
            if (reportDTO.name== "Student Details")
            {
                doc.Add(reportService.getStudentDetails());
            }
            else if (reportDTO.name == "Class Details")
            {
               doc.Add(reportService.getClasDetails());
            }            
            else
            {
                return null;
            }

            // Closing the document
            doc.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            Response.ContentType = "application/pdf";
            Response.BinaryWrite(workStream.ToArray());
            return null;

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize("Report")]
        public ActionResult PrintPDFDayWise(ReportDTO reportDTO)
        {
            if (!ModelState.IsValid)
            {
              ViewBag.name = new SelectList(reportService.GetReportType("daywise"), "name", "name");
                return View(reportDTO);
            }


            MemoryStream workStream = new MemoryStream();
            StringBuilder status = new StringBuilder("");
            DateTime dTime = DateTime.Now;
            //file name to be created 
            string strPDFFileName = string.Format("SamplePdf" + dTime.ToString("yyyyMMdd") + "-" + ".pdf");
            Document doc = new Document();
            doc.SetMargins(10f, 10f, 10f, 10f);

            //doc.SetMargins(0f, 0f, 0f, 0f);
            //Create PDF Table

            //file will created in this path
            string strAttachment = Server.MapPath("~/Downloadss/" + strPDFFileName);


            PdfWriter.GetInstance(doc, workStream).CloseStream = false;
            doc.Open();
            // doc.Add(reportService.TenantDetails());
            //Add Content to PDF 
            if (reportDTO.name == "Student Attendance Details")
            {
                doc.Add(reportService.StudentAttendanceDetails(reportDTO));
            }
            else if (reportDTO.name == "Staff Attendance Details")
            {
                doc.Add(reportService.UserAttendancesDetails(reportDTO));
            }
            else if (reportDTO.name == "POS Revenue Details")
            {
                doc.Add(reportService.PosRevenueDetails(reportDTO));
            }
            else if (reportDTO.name == "Payment Details")
            {
                doc.Add(reportService.PaymentDetails(reportDTO));
            }
            
            else
            {
                return null;
            }
            
            // Closing the document
            doc.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            Response.ContentType = "application/pdf";
            Response.BinaryWrite(workStream.ToArray());
            return null;

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize("Report")]
        public ActionResult PrintPDFDayWiseAttendence(ReportDTO reportDTO)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.name = new SelectList(db.ClassRooms.ToList(), "id", "name");
                return View(reportDTO);
            }


            MemoryStream workStream = new MemoryStream();
            StringBuilder status = new StringBuilder("");
            DateTime dTime = DateTime.Now;
            //file name to be created 
            string strPDFFileName = string.Format("SamplePdf" + dTime.ToString("yyyyMMdd") + "-" + ".pdf");
            Document doc = new Document();
            doc.SetMargins(10f, 10f, 10f, 10f);

            //doc.SetMargins(0f, 0f, 0f, 0f);
            //Create PDF Table

            //file will created in this path
            string strAttachment = Server.MapPath("~/Downloadss/" + strPDFFileName);


            PdfWriter.GetInstance(doc, workStream).CloseStream = false;
            doc.Open();
            // doc.Add(reportService.TenantDetails());
            //Add Content to PDF 
 
           doc.Add(reportService.StudentAttendanceDetails(reportDTO));


            // Closing the document
            doc.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            Response.ContentType = "application/pdf";
            Response.BinaryWrite(workStream.ToArray());
            return null;

        }

        //   [CustomAuthorize("Report")]
        public ActionResult GetDashbordData()
        {
           //  ViewBag.green = dashboardService.getTenantsData();
        //    ViewBag.red = dashboardService.getPendingOrderData();
         //   ViewBag.blue = dashboardService.getMonthlyIncome();
            return PartialView();
        }

        
    }

}