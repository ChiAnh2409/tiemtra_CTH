using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TiemTra.Models;
using TiemTra.ViewModel;

namespace TiemTra.Controllers
{
    public class AccountController : Controller
    {
        private QLTTEntities1 db = new QLTTEntities1();
        // GET: Account
        public ActionResult Index()
        {
            MutipleData data = new MutipleData();
            data.phieuNhaps = db.PhieuNhap.ToList();
            data.khoNguyenLieus = db.KhoNguyenLieu.ToList();
            data.bill = db.Bill.ToList();
            var homnay = DateTime.Now;
            var tongSoLuongHanSuDungCon30Ngay = db.KhoNguyenLieu.Count(knl => DbFunctions.DiffDays(homnay, knl.HanSuDung) <= 30);
            ViewBag.TongSoLuongHanSuDungCon30Ngay = tongSoLuongHanSuDungCon30Ngay;

            return View(data);
        }
        
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(Account acc)
        {
            var usr = db.Account.Where(a => a.Email.Equals(acc.Email) && a.Pass.Equals(acc.Pass)).FirstOrDefault();
            if (usr != null)
            {
                var role = (from ru in db.RoleUser
                            join s in db.Staff on ru.IdStaff equals s.IdStaff
                            join r in db.Roles on ru.IdRoleUser equals r.Id_Role
                            where ru.IdStaff == usr.IdStaff // Lọc theo IdStaff tương ứng với người dùng đã đăng nhập
                            select ru.Id_Role.ToString()).ToList();
                if (role.Count > 0)
                {
                    // Lấy Id_Role đầu tiên từ danh sách roles
                    Session["Id_Role"] = role[0].ToString();
                }
                Session["Email"] = acc.Email.ToString();
                Session["Pass"] = acc.Pass.ToString();

                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", " Tên tài khoản hoặc mật khẩu sai! ");
            }
            return View();
        }
        public ActionResult Logout()
        {
            // Xóa session đăng nhập
            Session.Abandon();

            // Chuyển hướng đến trang chủ
            return RedirectToAction("DangNhap", "Account");
        }

        public ActionResult LoadBarChart(int? selectedYear)
        {
            var mymodel = new MutipleData();

            // Filter ThanhToans based on the selected year
            var invoices = db.Bill
                .Where(t => selectedYear == null || t.NgayBan.Year == selectedYear)
                .ToList();

            // Group invoices by month and calculate the rounded sum of TongTien for each month
            var monthlyRevenue = invoices
                .GroupBy(t => t.NgayBan.Month)
                .Select(group => new
                {
                    Month = group.Key,
                    TotalRevenue = Math.Round((double)(group.Sum(t => t.TongTien) ?? 0), 3) // Rounded to 3 decimal places
                })
                .OrderBy(entry => entry.Month) // Keep ascending order of months
                .ToList();

            return Json(monthlyRevenue, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ThuChi(int? selectedYear)
        {
            var mymodel = new MutipleData();

            // Filter ThanhToans based on the selected year
            var invoices = db.PhieuNhap
                .Where(t => selectedYear == null || t.NgayNhap.Year == selectedYear)
                .ToList();

            // Group invoices by month and calculate the rounded sum of TongTien for each month
            var monthlyRevenue = invoices
                .GroupBy(t => t.NgayNhap.Month)
                .Select(group => new
                {
                    Month = group.Key,
                    TotalRevenue = Math.Round((group.Sum(t => t.TongTien) ?? 0), 3) // Rounded to 3 decimal places
                })
                .OrderBy(entry => entry.Month) // Keep ascending order of months
                .ToList();

            return Json(monthlyRevenue, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Export()
        {
            // Chuyển hướng yêu cầu tới hành động ExportBillStatisticsToExcel trong ExcelExportController
            return RedirectToAction("ExportBillStatisticsToExcel", "Export");
        }
    }
}