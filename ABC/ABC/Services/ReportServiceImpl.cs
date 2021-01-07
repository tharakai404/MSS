using ABC.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABC.Services
{
    public class ReportServiceImpl:ReportService
    {
      private ThakshilawaEntities2 db = new ThakshilawaEntities2();
      
        public PdfPTable getClasDetails()
        {
            //Create PDF Table with 5 columns
            PdfPTable tableLayout = new PdfPTable(6);
            float[] headers = {20,20,20,20,10,10};  //Header Widths
            tableLayout.SetWidths(headers);        //Set the pdf headers
            tableLayout.WidthPercentage = 100;       //Set the PDF File witdh percentage
            tableLayout.HeaderRows = 1;
            //Add Title to the PDF file at the top

            List<ClassRoom> employees = db.ClassRooms.ToList();



            tableLayout.AddCell(new PdfPCell(new Phrase("Classes Details", new Font(Font.FontFamily.HELVETICA, 14, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 12, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER });


            ////Add header
            AddCellToHeader(tableLayout, "Class Name");
            AddCellToHeader(tableLayout, "Lecture Name");           
            AddCellToHeader(tableLayout, "commencement date");
            AddCellToHeader(tableLayout, "course fee");
            AddCellToHeader(tableLayout, "Max Student");
            AddCellToHeader(tableLayout, "Enrolled Student");

            ////Add body

            foreach (var emp in employees)
           {

            AddCellToBody(tableLayout, emp.name);
            Lecture lec =  emp.Lecture;
            AddCellToBody(tableLayout, lec.UserData.name);           
            AddCellToBody(tableLayout, emp.commencementdate.ToShortDateString());
                AddCellToBody(tableLayout, emp.coursefee.ToString());
                AddCellToBody(tableLayout, emp.maxstudent.ToString());


                List<Enrollment> enroll = db.Enrollments
               .Where(a => a.classid== emp.id).ToList();

                AddCellToBody(tableLayout,enroll.Count.ToString());

            }
        
            return tableLayout;
        }

        public PdfPTable getStudentDetails()
        {
            //Create PDF Table with 5 columns
            PdfPTable tableLayout = new PdfPTable(6);
            float[] headers = { 10, 20, 10 ,10,20 ,30 };  //Header Widths
            tableLayout.SetWidths(headers);        //Set the pdf headers
            tableLayout.WidthPercentage = 100;       //Set the PDF File witdh percentage
            tableLayout.HeaderRows = 1;
            //Add Title to the PDF file at the top

            List<Student> employees = db.Students.ToList();



            tableLayout.AddCell(new PdfPCell(new Phrase("Student Details", new Font(Font.FontFamily.HELVETICA, 14, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 12, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER });


            ////Add header
            AddCellToHeader(tableLayout, "Reg ID");
            AddCellToHeader(tableLayout, "Student Name");
            AddCellToHeader(tableLayout, "Mobile");
            AddCellToHeader(tableLayout, "NIC");
            AddCellToHeader(tableLayout, "Course");
            AddCellToHeader(tableLayout, "Address");

            ////Add body

            foreach (var emp in employees)
            {

                AddCellToBody(tableLayout, emp.regid);
                AddCellToBody(tableLayout, emp.name);
                AddCellToBody(tableLayout, emp.mobile);
                AddCellToBody(tableLayout, emp.nic);
                Enrollment enr = db.Enrollments.Where(a => a.studentid == emp.id).FirstOrDefault();
                if (enr != null)
                {
                    AddCellToBody(tableLayout, enr.ClassRoom.name);
                }
                else {
                    AddCellToBody(tableLayout, "Not Enrolled");
                }
               
                AddCellToBody(tableLayout, emp.address);


                


            }

            return tableLayout;
        }

        /* 
              public PdfPTable SecurityOfficers()
              {
                  //Create PDF Table with 5 columns
                  PdfPTable tableLayout = new PdfPTable(4);
                  float[] headers = { 30, 45, 45, 45 };  //Header Widths
                  tableLayout.SetWidths(headers);        //Set the pdf headers
                  tableLayout.WidthPercentage = 100;       //Set the PDF File witdh percentage
                  tableLayout.HeaderRows = 1;
                  //Add Title to the PDF file at the top

                  List<SecurityOfficer> employees = db.SecurityOfficer.ToList();



                  tableLayout.AddCell(new PdfPCell(new Phrase("Security Officers Details", new Font(Font.FontFamily.HELVETICA, 14, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 12, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER });


                  ////Add header
                  AddCellToHeader(tableLayout, "Residential unit Name");
                  AddCellToHeader(tableLayout, "Officer Name");
                  AddCellToHeader(tableLayout, "Nic");
                  AddCellToHeader(tableLayout, "Telephone");

                  ////Add body

                  foreach (var emp in employees)
                  {

                      AddCellToBody(tableLayout, emp.ResidentialUnits.Name);
                      AddCellToBody(tableLayout, emp.OfficerName);
                      AddCellToBody(tableLayout, emp.NIC);
                      AddCellToBody(tableLayout, emp.Contact_no);

                  }

                  return tableLayout;
              }
              public PdfPTable ResidentialUnits()
              {
                  //Create PDF Table with 5 columns
                  PdfPTable tableLayout = new PdfPTable(4);
                  float[] headers = { 10, 45, 45, 60 };  //Header Widths
                  tableLayout.SetWidths(headers);        //Set the pdf headers
                  tableLayout.WidthPercentage = 100;       //Set the PDF File witdh percentage
                  tableLayout.HeaderRows = 1;
                  //Add Title to the PDF file at the top

                  List<ResidentialUnit> employees = db.ResidentialUnits.ToList();



                  tableLayout.AddCell(new PdfPCell(new Phrase("Residential Units Details", new Font(Font.FontFamily.HELVETICA, 14, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 12, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER });


                  ////Add header
                  AddCellToHeader(tableLayout, "id");
                  AddCellToHeader(tableLayout, "Residential unit Name");
                  AddCellToHeader(tableLayout, "Owner Name");
                  AddCellToHeader(tableLayout, "Address");

                  ////Add body

                  foreach (var emp in employees)
                  {
                      AddCellToBody(tableLayout, emp.id.ToString());
                      AddCellToBody(tableLayout, emp.Name);
                      AddCellToBody(tableLayout, emp.Owner.Name);
                      AddCellToBody(tableLayout, emp.Address);


                  }

                  return tableLayout;
              }
              public PdfPTable MonthlyPayment()
              {
                  //Create PDF Table with 5 columns
                  PdfPTable tableLayout = new PdfPTable(4);
                  float[] headers = { 50, 24, 45, 35 };  //Header Widths
                  tableLayout.SetWidths(headers);        //Set the pdf headers
                  tableLayout.WidthPercentage = 100;       //Set the PDF File witdh percentage
                  tableLayout.HeaderRows = 1;
                  //Add Title to the PDF file at the top

                  List<Rent> employees = db.Rents.ToList();
               //   List<Tenant> employeess = db..ToList();



                  tableLayout.AddCell(new PdfPCell(new Phrase("Tenant Details", new Font(Font.FontFamily.HELVETICA, 14, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 12, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER });


                  ////Add header           
                  AddCellToHeader(tableLayout, "ResidentialUnit Name");
                  AddCellToHeader(tableLayout, "Tenant Name");
                  AddCellToHeader(tableLayout, "Key Money");
                  AddCellToHeader(tableLayout, "Monthly Rent Fee");

                  ////Add body
                  foreach (var emp in employees)
                  {               
                      AddCellToBody(tableLayout, emp.ResidentialUnit.Name);
                      AddCellToBody(tableLayout, emp.Tenant.Name);
                      AddCellToBody(tableLayout, emp.KeyMoney.ToString());
                      AddCellToBody(tableLayout, emp.monthlyRentFee.ToString());
                  }

                  return tableLayout;
              }
                    */
        public PdfPTable StudentAttendanceDetails(ReportDTO dto)
          {
              //Create PDF Table with 5 columns
              PdfPTable tableLayout = new PdfPTable(6);
              float[] headers = { 15, 15, 15, 15, 25, 15 };  //Header Widths
              tableLayout.SetWidths(headers);        //Set the pdf headers
              tableLayout.WidthPercentage = 100;       //Set the PDF File witdh percentage
              tableLayout.HeaderRows = 1;

            int classid = Convert.ToInt32(dto.name);
            ClassRoom clz= db.ClassRooms.Find(classid);

            var list = (from dd in db.Enrollments
                        join pp in db.Students on dd.studentid equals pp.id
                        join ddd in db.StudentAttendances on pp.id equals ddd.studentid
                        where ddd.date >= dto.fromDate && ddd.date <= dto.toDate && dd.classid== classid 
                        select new 
                        {
                           ddd
                        }).Distinct().OrderBy(c => c.ddd.inout).ToList();



            List<StudentAttendance> employees = db.StudentAttendances.Where(a => a.date >= dto.fromDate && a.date <= dto.toDate).ToList();


              tableLayout.AddCell(new PdfPCell(new Phrase("Student Attendance Details for " + dto.fromDate.ToShortDateString()+" - "+ dto.toDate.ToShortDateString(), new Font(Font.FontFamily.HELVETICA, 14, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 12, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER });


              ////Add header           
            
            AddCellToHeader(tableLayout, "IN / OUT");
            AddCellToHeader(tableLayout, "Date");
            AddCellToHeader(tableLayout, "Time");
            AddCellToHeader(tableLayout, "Student ID");
            AddCellToHeader(tableLayout, "Student Name");
            AddCellToHeader(tableLayout, "Class Name");
          

            ////Add body
                foreach (var emp in list)
                {
                   // emp.ResidentialUnit.
                    AddCellToBody(tableLayout, emp.ddd.inout);
                    AddCellToBody(tableLayout, emp.ddd.date.ToShortDateString());
                AddCellToBody(tableLayout, emp.ddd.date.ToShortTimeString());
                AddCellToBody(tableLayout, emp.ddd.Student.regid);
                AddCellToBody(tableLayout, emp.ddd.Student.name);
                Enrollment enrollment = db.Enrollments.Where(a => a.studentid == emp.ddd.studentid).FirstOrDefault();
                AddCellToBody(tableLayout,enrollment.ClassRoom.name);
                }

            return tableLayout;
          }
        public PdfPTable UserAttendancesDetails(ReportDTO dto)
        {
            //Create PDF Table with 5 columns
            PdfPTable tableLayout = new PdfPTable(5);
            float[] headers = { 15, 15, 15, 15, 35 };  //Header Widths
            tableLayout.SetWidths(headers);        //Set the pdf headers
            tableLayout.WidthPercentage = 100;       //Set the PDF File witdh percentage
            tableLayout.HeaderRows = 1;
            //Add Title to the PDF file at the top
            //   string quary = "SELECT *   FROM[Apartment management].[dbo].[Rent]   where id not in( SELECT[rent_id]   FROM [Apartment management].[dbo].[RentPaymentDetails])";

            //    List<Rent> dataList = db.Database.SqlQuery<Rent>(quary).ToList(); ;

            //   List<Rent> employees = db.Rents.Contains;
            //   List<Tenant> employeess = db..ToList();

            //List<customerOrder> employees = db.customerOrders
            //                 .Where(a => a.date >= dto.fromDate && a.date <= dto.toDate).ToList();
            // .Where(a => a.Date > dto.fromDate)
            // .Where(a => a.Date.Month == dto.fromDate.Month)
            //   .Select(x => x.rent_id).ToArray();

            //   List<Rent> employees = db.Rents.Where(p => !paymentDoneids.Contains(p.id)).ToList();

            List<UserAttendance> employees = db.UserAttendances
                .Where(a => a.date >= dto.fromDate && a.date <= dto.toDate).ToList();


            tableLayout.AddCell(new PdfPCell(new Phrase("User Attendances Details for " + dto.fromDate.ToShortDateString() + " - " + dto.toDate.ToShortDateString(), new Font(Font.FontFamily.HELVETICA, 14, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 12, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER });


            ////Add header           

            AddCellToHeader(tableLayout, "IN / OUT");
            AddCellToHeader(tableLayout, "Date");
            AddCellToHeader(tableLayout, "Time");
            AddCellToHeader(tableLayout, "Staff Name");
            AddCellToHeader(tableLayout, "Type");


            ////Add body
            foreach (var emp in employees)
            {
                // emp.ResidentialUnit.
                AddCellToBody(tableLayout, emp.inout);
                AddCellToBody(tableLayout, emp.date.ToShortDateString());
                AddCellToBody(tableLayout, emp.date.ToShortTimeString());
                AddCellToBody(tableLayout, emp.UserData.name);
                AddCellToBody(tableLayout, emp.UserData.type);
            }

            return tableLayout;
        }

        public PdfPTable PaymentDetails(ReportDTO dto)
        {
            //Create PDF Table with 5 columns
            PdfPTable tableLayout = new PdfPTable(7);
            float[] headers = { 10, 10, 15, 20, 10, 15, 15 };  //Header Widths
            tableLayout.SetWidths(headers);        //Set the pdf headers
            tableLayout.WidthPercentage = 100;       //Set the PDF File witdh percentage
            tableLayout.HeaderRows = 1;
           

            List<Payment> employees = db.Payments
                .Where(a => a.date >= dto.fromDate && a.date <= dto.toDate).ToList();


            tableLayout.AddCell(new PdfPCell(new Phrase("Payment Details for " + dto.fromDate.ToShortDateString() + " - " + dto.toDate.ToShortDateString(), new Font(Font.FontFamily.HELVETICA, 14, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 12, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER });


            ////Add header           
            AddCellToHeader(tableLayout, "Date");
            AddCellToHeader(tableLayout, "Reg ID");
            AddCellToHeader(tableLayout, "Course name");
            AddCellToHeader(tableLayout, "Student Name");           
            AddCellToHeader(tableLayout, "Type");
            AddCellToHeader(tableLayout, "Amount");
            AddCellToHeader(tableLayout, "Collected by");

            decimal total = 0;
            ////Add body
            foreach (var emp in employees)
            {

                AddCellToBody(tableLayout, emp.date.ToShortDateString());
                Enrollment enr = emp.Enrollment;
                AddCellToBody(tableLayout, enr.Student.regid);
                AddCellToBody(tableLayout, enr.ClassRoom.name);
                AddCellToBody(tableLayout, enr.Student.name);
                AddCellToBody(tableLayout, emp.type);
                AddCellToBody(tableLayout, emp.amount.ToString());
                AddCellToBody(tableLayout, emp.UserData.name);
                total += emp.amount;
            }

            AddCellToBody(tableLayout, "-");
            AddCellToBody(tableLayout,"-");
            AddCellToBody(tableLayout, "-");
            AddCellToBody(tableLayout, "-");
            AddCellToBody(tableLayout, "Total :");
            AddCellToBody(tableLayout, total.ToString());
            AddCellToBody(tableLayout, "-");

            return tableLayout;
        }

        public PdfPTable PosRevenueDetails(ReportDTO dto)
        {
            //Create PDF Table with 5 columns
            PdfPTable tableLayout = new PdfPTable(10);
            float[] headers = { 10,10, 10, 20, 10, 10, 5,10, 10, 10};  //Header Widths
            tableLayout.SetWidths(headers);        //Set the pdf headers
            tableLayout.WidthPercentage = 100;       //Set the PDF File witdh percentage
            tableLayout.HeaderRows = 1;
            //Add Title to the PDF file at the top
            //   string quary = "SELECT *   FROM[Apartment management].[dbo].[Rent]   where id not in( SELECT[rent_id]   FROM [Apartment management].[dbo].[RentPaymentDetails])";

            //    List<Rent> dataList = db.Database.SqlQuery<Rent>(quary).ToList(); ;

            //   List<Rent> employees = db.Rents.Contains;
            //   List<Tenant> employeess = db..ToList();

            //List<customerOrder> employees = db.customerOrders
            //                 .Where(a => a.date >= dto.fromDate && a.date <= dto.toDate).ToList();
            // .Where(a => a.Date > dto.fromDate)
            // .Where(a => a.Date.Month == dto.fromDate.Month)
            //   .Select(x => x.rent_id).ToArray();

            //   List<Rent> employees = db.Rents.Where(p => !paymentDoneids.Contains(p.id)).ToList();

            List<SellingItemInvoice> employees = db.SellingItemInvoices
                .Where(a => a.date >= dto.fromDate && a.date <= dto.toDate).ToList();


            tableLayout.AddCell(new PdfPCell(new Phrase("POS Revenue Details for " + dto.fromDate.ToShortDateString() + " - " + dto.toDate.ToShortDateString(), new Font(Font.FontFamily.HELVETICA, 14, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 12, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER });


            ////Add header           

            AddCellToHeader(tableLayout, "Date");
            AddCellToHeader(tableLayout, "Bill ID");
            AddCellToHeader(tableLayout, "Pay by");
            AddCellToHeader(tableLayout, "Item Name");
            AddCellToHeader(tableLayout, "Selling Price");
            AddCellToHeader(tableLayout, "Cost Price");
            AddCellToHeader(tableLayout, "QTY");
            AddCellToHeader(tableLayout, "Selling Value");
            AddCellToHeader(tableLayout, "Cost Value");
            AddCellToHeader(tableLayout, "Loss / Profit");

            decimal sellinttot = 0;
            decimal costtot = 0;
            ////Add body
            foreach (var emp in employees)
            {
                foreach (var item in emp.SellingItemInvoiceDetails)
                {
                    AddCellToBody(tableLayout, emp.date.ToShortDateString());
                    AddCellToBody(tableLayout, emp.id.ToString());
                    AddCellToBody(tableLayout, emp.paytype);
                    SellingItem sellitem = item.SellingItem;
                    AddCellToBody(tableLayout,sellitem.itemname);
                    AddCellToBody(tableLayout, sellitem.sellingrate.ToString());
                    AddCellToBody(tableLayout, sellitem.buyingrate.ToString());
                    AddCellToBody(tableLayout, item.qty.ToString());
                    AddCellToBody(tableLayout, (item.qty* sellitem.sellingrate).ToString());
                    AddCellToBody(tableLayout, (item.qty * sellitem.buyingrate).ToString());
                    AddCellToBody(tableLayout, ((item.qty * sellitem.sellingrate) -(item.qty * sellitem.buyingrate)).ToString());
                    sellinttot += (item.qty * sellitem.sellingrate);
                    costtot += (item.qty * sellitem.buyingrate);
                }
                
               
            }
            AddCellToBody(tableLayout, "-");
            AddCellToBody(tableLayout, "-");
            AddCellToBody(tableLayout, "-");
            AddCellToBody(tableLayout, "-");
            AddCellToBody(tableLayout,"-");
            AddCellToBody(tableLayout, "-");
            AddCellToBody(tableLayout, "-");
            AddCellToBody(tableLayout, sellinttot.ToString());
            AddCellToBody(tableLayout, costtot.ToString());
            AddCellToBody(tableLayout, (sellinttot-costtot).ToString());

            return tableLayout;
        }

        public PdfPTable LaundryDetails(ReportDTO dto)
        {
            //Create PDF Table with 5 columns
            PdfPTable tableLayout = new PdfPTable(8);
            float[] headers = { 20, 30, 30, 40, 10, 10 ,10,10};  //Header Widths
            tableLayout.SetWidths(headers);        //Set the pdf headers
            tableLayout.WidthPercentage = 100;       //Set the PDF File witdh percentage
            tableLayout.HeaderRows = 1;
            //Add Title to the PDF file at the top
            //   string quary = "SELECT *   FROM[Apartment management].[dbo].[Rent]   where id not in( SELECT[rent_id]   FROM [Apartment management].[dbo].[RentPaymentDetails])";

            //    List<Rent> dataList = db.Database.SqlQuery<Rent>(quary).ToList(); ;

            //   List<Rent> employees = db.Rents.Contains;
            //   List<Tenant> employeess = db..ToList();

        //    List<customerOrderDTL> employees = db.customerOrderDTLs
          //                     .Where(a => a.customerOrder.date >= dto.fromDate && a.customerOrder.date <= dto.toDate).ToList();
            // .Where(a => a.Date > dto.fromDate)
            // .Where(a => a.Date.Month == dto.fromDate.Month)
            //   .Select(x => x.rent_id).ToArray();

            //   List<Rent> employees = db.Rents.Where(p => !paymentDoneids.Contains(p.id)).ToList();



            tableLayout.AddCell(new PdfPCell(new Phrase("Laundry Details for " + dto.fromDate.ToShortDateString() + " - " + dto.toDate.ToShortDateString(), new Font(Font.FontFamily.HELVETICA, 14, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 12, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER });


            ////Add header           
            AddCellToHeader(tableLayout, "Order ID");
            AddCellToHeader(tableLayout, "Date");            
            AddCellToHeader(tableLayout, "Deliver Date");
            AddCellToHeader(tableLayout, "Item Name");
            AddCellToHeader(tableLayout, "Qty");
            AddCellToHeader(tableLayout, "Wash");
            AddCellToHeader(tableLayout, "Dry clean");
            AddCellToHeader(tableLayout, "Iron");

            ////Add body
            /*
            foreach (var emp in employees)
            {
                // emp.ResidentialUnit.
                AddCellToBody(tableLayout, emp.id.ToString());
                AddCellToBody(tableLayout, emp.customerOrder.date.ToShortDateString());               
                AddCellToBody(tableLayout, emp.customerOrder.deliverDate.ToShortDateString());
                AddCellToBody(tableLayout, emp.service.service1);
                AddCellToBody(tableLayout, emp.qty.ToString());
                AddCellToBody(tableLayout, emp.wash > 0 ? "Yes" : "No");
                AddCellToBody(tableLayout, emp.dryclean > 0 ? "Yes" : "No");
                AddCellToBody(tableLayout, emp.Iron > 0 ? "Yes" : "No");
            }*/

            return tableLayout;
        }

        // Method to add single cell to the Header
        private static void AddCellToHeader(PdfPTable tableLayout, string cellText)
        {

            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.YELLOW))) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = new iTextSharp.text.BaseColor(128, 0, 0) });
        }

        // Method to add single cell to the body
        private static void AddCellToBody(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.BLACK))) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255) });
        }

        public List<ReportDTO>  GetReportType(string type) {
            List<ReportDTO> list = new List<ReportDTO>();
            ReportDTO obj = new ReportDTO();
            if ("daywise"==type)
            {
                obj = new ReportDTO();
                obj.name = "Student Attendance Details";
             //   list.Add(obj);
                obj = new ReportDTO();
                obj.name = "Staff Attendance Details";
                list.Add(obj);

                obj = new ReportDTO();
                obj.name = "POS Revenue Details";
                list.Add(obj);

                obj = new ReportDTO();
                obj.name = "Payment Details";
                list.Add(obj);
            }
            else
            {
                obj = new ReportDTO();
                obj.name = "Student Details";
                list.Add(obj);
                obj = new ReportDTO();
                obj.name = "Class Details";
                list.Add(obj);
                obj = new ReportDTO();
                obj.name = "Residential Units";
             //   list.Add(obj);
                obj = new ReportDTO();
                obj.name = "Monthly Payment";
            //    list.Add(obj);

            }
            return list;
        }

    }

}