using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using TiemTra.Models;
using TiemTra.ViewModel;

namespace TiemTra.Controllers
{
    public class NhanVienController : Controller
    {
        private QLTTEntities1 db = new QLTTEntities1();
        // GET: NhanVien
        public ActionResult NhanVien(int id)
        {
            var list = new MutipleData();
            list.Staff = db.Staff.ToList();
            list.Staff = db.Staff.Where(c => c.IdStaff == id).ToList();
            list.khoNguyenLieus = db.KhoNguyenLieu.ToList();
            list.bill = db.Bill.ToList();
            list.phieuNhaps = db.PhieuNhap.ToList();
            return View(list);
        }

        public ActionResult AllNhanVien()
        {
            var list = new MutipleData();
            list.Staff = db.Staff.ToList();
            list.phieuNhaps = db.PhieuNhap.ToList();
            list.khoNguyenLieus = db.KhoNguyenLieu.ToList();
            list.bill = db.Bill.ToList();
            return View(list);
        }
        public ActionResult ThemNV()
        {
            return View();
        }

        [HttpPost]

        public ActionResult ThemNV(Staff nv)
        {
            db.Staff.Add(nv);
            db.SaveChanges();
            return RedirectToAction("NhanVien");
        }

        public ActionResult SuaNV(int id)
        {
             Staff nv = db.Staff.Find(id);
            return View(nv);
        }
        [HttpPost]
        public ActionResult SuaNV(Staff nv)
        {
            db.Entry(nv).State = System.Data.Entity.EntityState.Modified;
            
            db.SaveChanges();
            return RedirectToAction("NhanVien");
        }
        [HttpPost]
        public ActionResult XoaNV(int id)
        {
            Staff nv = db.Staff.Find(id);
            db.Staff.Remove(nv);
            db.SaveChanges();
            return RedirectToAction("NhanVien");
        }
    }
}