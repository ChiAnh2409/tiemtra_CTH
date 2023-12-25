using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiemTra.Models;
using TiemTra.ViewModel;

namespace TiemTra.Controllers
{
    public class SizeController : Controller
    {
        private QLTTEntities1 db = new QLTTEntities1();
        // GET: Size
        public ActionResult Size()
        {
            var list = new MutipleData();
            list.sizes = db.Size.ToList();
            list.phieuNhaps = db.PhieuNhap.ToList();
            list.khoNguyenLieus = db.KhoNguyenLieu.ToList();
            list.bill = db.Bill.ToList();

            return View(list);
        }

        public ActionResult ThemSize()
        {
            return View();
        }
        [HttpPost]

        public ActionResult ThemSize(Size s)
        {
            db.Size.Add(s);
            db.SaveChanges();
            return RedirectToAction("Size");
        }

        public ActionResult SuaSize(int id)
        {
            Size s = db.Size.Find(id);
            return View(s);
        }
        [HttpPost]
        public ActionResult SuaSize(Size s)
        {
            db.Entry(s).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Size");
        }
        [HttpPost]

        public ActionResult XoaSize(int id)
        {
            Size s = db.Size.Find(id);
            db.Size.Remove(s);
            db.SaveChanges();
            return RedirectToAction("Size");
        }
    }
}