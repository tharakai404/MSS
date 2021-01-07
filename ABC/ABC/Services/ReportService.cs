using ABC.Models;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.Services
{
    public interface ReportService
    {
        /*  PdfPTable TenantDetails();
          PdfPTable SecurityOfficers();
          PdfPTable MonthlyPayment();
          PdfPTable ResidentialUnits();
          PdfPTable MonthlyDelayPayment(ReportDTO dto);

          PdfPTable PaymentDetails(ReportDTO dto);
          List<ReportDTO> GetReportType(string type);
        */
        PdfPTable getClasDetails();
        List<ReportDTO> GetReportType(string type);
        PdfPTable StudentAttendanceDetails(ReportDTO dto);

        PdfPTable UserAttendancesDetails(ReportDTO dto);

        PdfPTable PosRevenueDetails(ReportDTO dto);

        PdfPTable PaymentDetails(ReportDTO dto);
        PdfPTable getStudentDetails();
        PdfPTable LaundryDetails(ReportDTO dto);

    }
}
