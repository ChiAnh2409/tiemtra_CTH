using OfficeOpenXml;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using TiemTra.Models;

namespace TiemTra.Controllers
{
    public class ExportController : Controller
    {
        QLTTEntities1 db = new QLTTEntities1();

        public class BillStatistic
        {
            public int Thang { get; set; }
            public int Nam { get; set; }
            public decimal TongTienThang { get; set; }
        }

        public class PNStatistic
        {
            public int Thang { get; set; }
            public int Nam { get; set; }
            public decimal TongTienThang { get; set; }
        }

        public ActionResult ExportBillStatisticsToExcel()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Hoặc LicenseContext.Commercial tùy thuộc vào giấy phép bạn sử dụng

            var query = @"
                SELECT 
                    MONTH(NgayBan) AS Thang,
                    YEAR(NgayBan) AS Nam,
                    SUM(TongTien) AS TongTienThang
                FROM 
                    Bill
                GROUP BY 
                    YEAR(NgayBan), MONTH(NgayBan)
                ORDER BY 
                    Nam, Thang";

            var query1 = @"
                SELECT 
                    MONTH(NgayNhap) AS Thang,
                    YEAR(NgayNhap) AS Nam,
                    SUM(TongTien) AS TongTienThang
                FROM 
                    PhieuNhap
                GROUP BY 
                    YEAR(NgayNhap), MONTH(NgayNhap)
                ORDER BY 
                    Nam, Thang";

            var billStatistics = db.Database.SqlQuery<BillStatistic>(query).ToList();
            var pnStatistics = db.Database.SqlQuery<PNStatistic>(query1).ToList();

            using (ExcelPackage excel = new ExcelPackage())
            {
                var billSheet = excel.Workbook.Worksheets.Add("DoanhThu_Bill");
                var pnSheet = excel.Workbook.Worksheets.Add("ChiTieu_PhieuNhap");

                int row = 1;

                // Fill data for Bill sheet
                billSheet.Cells[row, 1].Value = "Tháng";
                billSheet.Cells[row, 2].Value = "Năm";
                billSheet.Cells[row, 3].Value = "Doanh Thu";
                row++;

                foreach (var stat in billStatistics)
                {
                    billSheet.Cells[row, 1].Value = stat.Thang;
                    billSheet.Cells[row, 2].Value = stat.Nam;
                    billSheet.Cells[row, 3].Value = stat.TongTienThang;
                    row++;
                }

                // Reset row for PhieuNhap sheet
                row = 1;

                // Fill data for PhieuNhap sheet
                pnSheet.Cells[row, 1].Value = "Tháng";
                pnSheet.Cells[row, 2].Value = "Năm";
                pnSheet.Cells[row, 3].Value = "Chi Tiêu";
                row++;

                foreach (var stat in pnStatistics)
                {
                    pnSheet.Cells[row, 1].Value = stat.Thang;
                    pnSheet.Cells[row, 2].Value = stat.Nam;
                    pnSheet.Cells[row, 3].Value = stat.TongTienThang;
                    row++;
                }

                MemoryStream stream = new MemoryStream();
                excel.SaveAs(stream);
                stream.Position = 0;

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ThuChi.xlsx");
            }

        }
        public ActionResult ExportBillStatisticsToPDF()
        {
            var query = @"
        SELECT 
            MONTH(NgayBan) AS Thang,
            YEAR(NgayBan) AS Nam,
            SUM(TongTien) AS TongTienThang
        FROM 
            Bill
        GROUP BY 
            YEAR(NgayBan), MONTH(NgayBan)
        ORDER BY 
            Nam, Thang";

            var query1 = @"
        SELECT 
            MONTH(NgayNhap) AS Thang,
            YEAR(NgayNhap) AS Nam,
            SUM(TongTien) AS TongTienThang
        FROM 
            PhieuNhap
        GROUP BY 
            YEAR(NgayNhap), MONTH(NgayNhap)
        ORDER BY 
            Nam, Thang";

            var billStatistics = db.Database.SqlQuery<BillStatistic>(query).ToList();
            var pnStatistics = db.Database.SqlQuery<PNStatistic>(query1).ToList();

            var model = new Tuple<List<BillStatistic>, List<PNStatistic>>(billStatistics, pnStatistics);

            var pdfResult = new ViewAsPdf("BillStatisticsToPDF", model)
            {
                FileName = "DoanhThu.pdf",
                CustomSwitches = "--header-html \"\" --footer-html \"\"",
            };

            // Trả về file PDF
            var byteArray = pdfResult.BuildFile(this.ControllerContext);
            return File(byteArray, "application/pdf", "ThuChi.pdf");
        }



        public ActionResult BillStatisticsToPDF(List<BillStatistic> billStatistics, List<PNStatistic> pnStatistics)
        {
            return View(new Tuple<List<BillStatistic>, List<PNStatistic>>(billStatistics, pnStatistics));
        }
    }
}
