using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace ABC.Services
{
    public class AppServices
    {
        private ThakshilawaEntities2 db = new ThakshilawaEntities2();
        public void sendMail(String mailTo, String name, String subject,String body)
        {
            string emailbody = "Dear " + name + ", \r\n \r\n";
            emailbody += body+"\r\n \r\n";
            emailbody += "\r\n";
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("esofthndtharaka@gmail.com", "Pass#word2"),
                EnableSsl = true
            };
           // client.UseDefaultCredentials = true;
            try
            {
                client.Send(mailTo, mailTo, subject, emailbody);
                Console.WriteLine("Sent");
            }
            catch (Exception e)
            {
                Console.WriteLine("Email not sent " + e.Message);
            }
        }

         

        public PdfPTable PrintInvoice(int id)
        {
            //Create PDF Table with 5 columns
            PdfPTable tableLayout = new PdfPTable(4);
            float[] headers = { 40, 20, 20, 20};  //Header Widths
            tableLayout.SetWidths(headers);        //Set the pdf headers
            tableLayout.WidthPercentage = 100;       //Set the PDF File witdh percentage
            tableLayout.HeaderRows = 1;

            //Add Title to the PDF file at the top
            Enrollment enrollment = db.Enrollments.Find(id);
            SellingItemInvoice employees = db.SellingItemInvoices.Find(id);



            tableLayout.AddCell(new PdfPCell(new Phrase("Thakshilawa POS Invoice", new Font(Font.FontFamily.HELVETICA, 16, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 12, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER });
            tableLayout.AddCell(new PdfPCell(new Phrase("Invoice No: "+id, new Font(Font.FontFamily.HELVETICA, 10, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 12, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase("Date : " + DateTime.Now, new Font(Font.FontFamily.HELVETICA, 10, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 12, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT });



            ////Add header
            AddCellToHeader(tableLayout, "Item Name");
            AddCellToHeader(tableLayout, "Qty");
            AddCellToHeader(tableLayout, "Rate");
            AddCellToHeader(tableLayout, "Value");

            ////Add body

                 foreach (var emp in employees.SellingItemInvoiceDetails)
              {

             //  AddCellToBody(tableLayout, emp.SellingItem.itemname);
                tableLayout.AddCell(new PdfPCell(new Phrase(emp.SellingItem.itemname, new Font(Font.FontFamily.HELVETICA,10, 1, iTextSharp.text.BaseColor.BLACK))) { Border = 0, HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5, BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255) });
                AddCellToBody(tableLayout, emp.qty.ToString());
               AddCellToBody(tableLayout, emp.SellingItem.sellingrate.ToString());
               AddCellToBody(tableLayout, emp.rate.ToString());
               }
            AddCellToBody(tableLayout,"");
            AddCellToBody(tableLayout, "");
            AddCellToBody(tableLayout, "Total amount");
            AddCellToBody(tableLayout, employees.amount.ToString());

            tableLayout.AddCell(new PdfPCell(new Phrase("Paid by "+ employees.paytype, new Font(Font.FontFamily.HELVETICA, 10, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 12, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER });

            return tableLayout;
        }

        public PdfPTable PrintPayment(int id)
        {
            //Create PDF Table with 5 columns
            PdfPTable tableLayout = new PdfPTable(4);
            float[] headers = { 40, 20, 20, 20 };  //Header Widths
            tableLayout.SetWidths(headers);        //Set the pdf headers
            tableLayout.WidthPercentage = 100;       //Set the PDF File witdh percentage
            tableLayout.HeaderRows = 1;
            

            //Add Title to the PDF file at the top

            Payment payment = db.Payments.Find(id);
            Enrollment enrollment = db.Enrollments.Find(payment.enrolementid);
            ClassRoom classRoom = db.ClassRooms.Find(enrollment.classid);
            Student student = db.Students.Find(enrollment.studentid);



           
            tableLayout.AddCell(new PdfPCell(new Phrase("Payment No: " + id, new Font(Font.FontFamily.HELVETICA, 10, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 12, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase("Date : " + DateTime.Now, new Font(Font.FontFamily.HELVETICA, 10, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 12, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase("Reg ID : " + student.regid, new Font(Font.FontFamily.HELVETICA, 10, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 12, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase("Thakshilawa Payment Slip", new Font(Font.FontFamily.HELVETICA, 16, 1, iTextSharp.text.BaseColor.WHITE))) { Colspan = 12, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER, BackgroundColor = iTextSharp.text.BaseColor.BLACK });


            ////Add header
            // AddCellToHeader(tableLayout, " ");
            // AddCellToHeader(tableLayout, " ");
            //  AddCellToHeader(tableLayout, " ");
            // AddCellToHeader(tableLayout, " ");

            ////Add body

            tableLayout.AddCell(new PdfPCell(new Phrase("Class Name - " + classRoom.name, new Font(Font.FontFamily.HELVETICA, 10, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 12, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase("Student Name - " + student.name, new Font(Font.FontFamily.HELVETICA, 10, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 12, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase("Amount : " + payment.amount+" / type : "+payment.type, new Font(Font.FontFamily.HELVETICA, 10, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 12, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 10, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 12, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase("Collected by : " + payment.UserData.name, new Font(Font.FontFamily.HELVETICA, 10, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 12, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_RIGHT });
            tableLayout.AddCell(new PdfPCell(new Phrase("- Thank You -", new Font(Font.FontFamily.HELVETICA, 10, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 12, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER });

            return tableLayout;
        }

        private static void AddCellToHeader(PdfPTable tableLayout, string cellText)
        {

            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 10, 1, iTextSharp.text.BaseColor.WHITE))) { HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5, BackgroundColor = iTextSharp.text.BaseColor.BLACK });
        }

        // Method to add single cell to the body
        private static void AddCellToBody(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 10, 1, iTextSharp.text.BaseColor.BLACK))) { Border=0, HorizontalAlignment = Element.ALIGN_RIGHT, Padding = 5, BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255) });
        }
    }
}