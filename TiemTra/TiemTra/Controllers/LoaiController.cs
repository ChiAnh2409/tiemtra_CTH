using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiemTra.Models;
using TiemTra.ViewModel;

namespace TiemTra.Controllers
{
    public class LoaiController : Controller
    {
        private QLTTEntities1 db = new QLTTEntities1();
        // GET: NhanVien
        public ActionResult Loai()
        {
            var list = new MutipleData();
            list.loaiThucUongs = db.LoaiThucUong.ToList();
            list.phieuNhaps = db.PhieuNhap.ToList();
            list.khoNguyenLieus = db.KhoNguyenLieu.ToList();
            list.bill = db.Bill.ToList();

            return View(list);

        }

        public ActionResult ThemLoai()
        {
            return View();
        }

        [HttpPost]

        public ActionResult ThemLoai(LoaiThucUong nl)
        {
            db.LoaiThucUong.Add(nl);
            db.SaveChanges();
            return RedirectToAction("Loai");
        }

        public ActionResult SuaLoai(int id)
        {
            LoaiThucUong nl = db.LoaiThucUong.Find(id);
            return View(nl);
        }
        [HttpPost]
        public ActionResult SuaLoai(LoaiThucUong nl)
        {
            db.Entry(nl).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Loai");
        }
        [HttpPost]
        public ActionResult XoaLoai(int id)
        {
            LoaiThucUong nl = db.LoaiThucUong.Find(id);
            db.LoaiThucUong.Remove(nl);
            db.SaveChanges();
            return RedirectToAction("Loai");
        }
    }
}