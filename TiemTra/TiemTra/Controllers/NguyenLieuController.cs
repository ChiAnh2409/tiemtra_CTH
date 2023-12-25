using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiemTra.Models;
using TiemTra.ViewModel;

namespace TiemTra.Controllers
{
    public class NguyenLieuController : Controller
    {
        private QLTTEntities1 db = new QLTTEntities1();
        // GET: NguyenLieu
        public ActionResult NguyenLieu()
        {
            var list = new MutipleData();
            list.nguyenLieus = db.NguyenLieu.ToList();
            list.phieuNhaps = db.PhieuNhap.ToList();
            list.khoNguyenLieus = db.KhoNguyenLieu.ToList();
            list.bill = db.Bill.ToList();

            return View(list);
        }
        public ActionResult ThemNL()
        {
            
            return View();
        }
        [HttpPost]
        public ActionResult ThemNL(NguyenLieu nl)
        {

            db.NguyenLieu.Add(nl);
            db.SaveChanges();
            return RedirectToAction("NguyenLieu");

        }

        public ActionResult SuaNL(int id)
        {
            NguyenLieu ngl = db.NguyenLieu.Find(id);
            return View(ngl);
        }
        [HttpPost]
        public ActionResult SuaNL(NguyenLieu ngl)
        {

            db.Entry(ngl).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("NguyenLieu");
        }
        [HttpPost]
        public ActionResult XoaNL(int id)
        {

            NguyenLieu ngl = db.NguyenLieu.Find(id);
            db.NguyenLieu.Remove(ngl);
            db.SaveChanges();
            return RedirectToAction("NguyenLieu");
        }
    }
}