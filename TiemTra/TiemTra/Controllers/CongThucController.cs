using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiemTra.Models;
using TiemTra.ViewModel;

namespace TiemTra.Controllers
{
    public class CongThucController : Controller
    {
        // GET: CongThuc
        private QLTTEntities1 db = new QLTTEntities1();
        public ActionResult CongThuc(int id)
        {
            var list = new MutipleData();
            list.phieuNhaps = db.PhieuNhap.ToList();
            list.congThucs = db.CongThuc.Include("ThucUong");
            list.congThucs = db.CongThuc.Include("NguyenLieu");
            list.khoNguyenLieus = db.KhoNguyenLieu.ToList();
            list.thucUongs = db.ThucUong.ToList();
            list.congThucs = db.CongThuc.Where(c => c.IdTU == id).ToList();
            list.nguyenLieus = db.NguyenLieu.ToList();
            list.bill = db.Bill.ToList();
            return View(list);
 
        }

        public ActionResult ThemCT(int id)
        {
            var list = new MutipleData();
            list.congThucs = db.CongThuc.Include("ThucUong");
            list.congThucs = db.CongThuc.Include("NguyenLieu");
            list.thucUongs = db.ThucUong.ToList();
            list.congThucs = db.CongThuc.Where(c => c.IdNLTU == id).ToList();
            list.nguyenLieus = db.NguyenLieu.ToList();
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            ViewBag.MultipleDataJson = JsonConvert.SerializeObject(list, settings);
            return View(list);
        }
        [HttpPost]
        public ActionResult ThemCT(List<CongThuc> listct)
        {
            // Lặp qua danh sách công thức mới thêm vào
            //foreach (var ct in listct)
            //{
            //    // Kiểm tra nếu IdSize là 2 hoặc Size là "Small"
            //    if (ct.ThucUong.IdSize == 2 || ct.ThucUong.Size.Size1 == "Small")
            //    {
            //        // Tạo mới 2 công thức với IdSize lần lượt là 3 và 4
            //        var newCT3 = new CongThuc
            //        {
            //            IdTU = ct.IdTU,
            //            IdNL = ct.IdNL,
            //            SoLuong = (int)(ct.SoLuong * 1.5), 
            //            DonVi = ct.DonVi,
            //                             // Tạo công thức dựa trên ct với IdSize = 3 và thực hiện tính toán số lượng
            //                             // Lưu ý: Bạn cần thay đổi cách tính toán số lượng dựa trên yêu cầu cụ thể
            //                             // Ở đây, ví dụ, tôi nhân số lượng của nguyên liệu với 150%
            //                             // Bạn cần thay đổi phần này cho phù hợp với logic thực của bạn
            //                             // Ví dụ: newCT3.SoLuong = ct.SoLuong * 1.5;
            //        };

            //        var newCT4 = new CongThuc
            //        {
            //            IdTU = ct.IdTU,
            //            IdNL = ct.IdNL,
            //            SoLuong = (int)(ct.SoLuong * 2),
            //            DonVi = ct.DonVi,
            //            // Tạo công thức dựa trên ct với IdSize = 4 và thực hiện tính toán số lượng
            //            // Lưu ý: Bạn cần thay đổi cách tính toán số lượng dựa trên yêu cầu cụ thể
            //            // Ở đây, ví dụ, tôi nhân số lượng của nguyên liệu với 200%
            //            // Bạn cần thay đổi phần này cho phù hợp với logic thực của bạn
            //            // Ví dụ: newCT4.SoLuong = ct.SoLuong * 2;
            //        };

            //        // Thêm 2 công thức mới vào danh sách
            //        db.CongThuc.Add(newCT3);
            //        db.CongThuc.Add(newCT4);
            //    }
            //}

            // Lưu các thay đổi vào cơ sở dữ liệu
            db.CongThuc.AddRange(listct);
            db.SaveChanges();

            bool isSuccess = true; // Giả sử giá trị thành công là true

            return Json(new { success = isSuccess, redirectUrl = "/Menu/Menu", message = "Thêm công thức thành công" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SuaCT(int id)
        {
            var list = new MutipleData();
            list.congThucs = db.CongThuc.Include("ThucUong");
            list.congThucs = db.CongThuc.Include("NguyenLieu");

            list.thucUongs = db.ThucUong.ToList();
            list.congThucs = db.CongThuc.Where(c => c.IdNLTU == id).ToList();
            list.nguyenLieus = db.NguyenLieu.ToList();
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            ViewBag.MultipleDataJson = JsonConvert.SerializeObject(list, settings);
            return View(list);
        }
        [HttpPost]
        public ActionResult SuaCT(CongThuc ct)
        {
            db.Entry(ct).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("CongThuc/" + ct.IdTU);
        }
        public ActionResult XoaCT(int id)
        {
            CongThuc ct = db.CongThuc.Find(id);
            db.CongThuc.Remove(ct);
            db.SaveChanges();
            return RedirectToAction("Menu/Menu");
        }
    }
}