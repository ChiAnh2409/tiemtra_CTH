using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiemTra.Models;
using TiemTra.ViewModel;

namespace TiemTra.Controllers
{
    public class KhoController : Controller
    {
        // GET: Kho
        private QLTTEntities1 db = new QLTTEntities1();
        public ActionResult Kho(int id)
        {
            var list = new MutipleData();
            list.phieuNhaps = db.PhieuNhap.ToList();
            list.nhaCungCap = db.NhaCungCap.ToList();
            list.nguyenLieus = db.NguyenLieu.ToList();
            list.bill = db.Bill.ToList();

            // Lấy thông tin của nguyên liệu có Id phù hợp
            list.khoNguyenLieus = db.KhoNguyenLieu
                                        .Include("NhaCungCap")
                                        .Include("NguyenLieu")
                                        .Where(c => c.TenNL == id)
                                        .ToList();

            return View(list);
        }


        public ActionResult Them()
        {
            var list = new MutipleData();
            list.khoNguyenLieus = db.KhoNguyenLieu.Include("NhaCungCap");
            list.khoNguyenLieus = db.KhoNguyenLieu.Include("NguyenLieu");
            list.nhaCungCap = db.NhaCungCap.ToList();
            list.nguyenLieus = db.NguyenLieu.ToList();
            return View(list);
        }
        [HttpPost]
        public ActionResult Them(KhoNguyenLieu nl)
        {

            db.KhoNguyenLieu.Add(nl);
            db.SaveChanges();
            return RedirectToAction("Kho/" + nl.TenNL);

        }

        public ActionResult Sua(int id)
        {
            var viewmodel = new MutipleData();
            viewmodel.khoNguyenLieus = db.KhoNguyenLieu.Where(nl => nl.IdNL == id).ToList();
            viewmodel.nhaCungCap = db.NhaCungCap.ToList();
            viewmodel.nguyenLieus = db.NguyenLieu.ToList();
            return View(viewmodel);
        }
        [HttpPost]
        public ActionResult Sua(KhoNguyenLieu ngl)
        {

            db.Entry(ngl).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Kho/" + ngl.TenNL);
        }
        [HttpPost]
        public ActionResult Xoa(int id)
        {

            KhoNguyenLieu ngl = db.KhoNguyenLieu.Find(id);
            db.KhoNguyenLieu.Remove(ngl);
            db.SaveChanges();
            return RedirectToAction("Kho");
        }
    }
}
