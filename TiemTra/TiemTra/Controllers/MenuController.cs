using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiemTra.Models;
using TiemTra.ViewModel;

namespace TiemTra.Controllers
{
    public class MenuController : Controller
    {
        // GET: Menu
        private QLTTEntities1 db = new QLTTEntities1();
        public ActionResult Menu()
        {
            var list = new MutipleData();
            list.thucUongs = db.ThucUong.Include("LoaiThucUong");
            list.thucUongs = db.ThucUong.Include("Size");
            list.loaiThucUongs = db.LoaiThucUong.ToList();
            list.sizes = db.Size.ToList();
            list.phieuNhaps = db.PhieuNhap.ToList();
            list.khoNguyenLieus = db.KhoNguyenLieu.ToList();
            list.bill = db.Bill.ToList();

            return View(list);
        }

        public ActionResult ThemTU()
        {
            var list = new MutipleData();
            list.thucUongs = db.ThucUong.ToList();
            list.loaiThucUongs = db.LoaiThucUong.ToList();
            list.sizes = db.Size.ToList();
            return View(list);

        }

        [HttpPost]

        public ActionResult ThemTU(ThucUong tu, HttpPostedFileBase Image)
        {
            if (Image != null && Image.ContentLength > 0)
            {
                var fileName = Path.GetFileName(Image.FileName);
                var path = Path.Combine(Server.MapPath("~/img"), fileName);
                Image.SaveAs(path);

                tu.Image = fileName; // Lưu đường dẫn hình ảnh vào cơ sở dữ liệu
            }

            db.ThucUong.Add(tu);
            db.SaveChanges();

            return RedirectToAction("Menu");
        }

        public ActionResult SuaTU(int id)
        {
            var viewmodel = new MutipleData();
            viewmodel.thucUongs = db.ThucUong.Where(nl => nl.IdTU == id).ToList();
            viewmodel.loaiThucUongs = db.LoaiThucUong.ToList();
            viewmodel.sizes = db.Size.ToList();
            return View(viewmodel);
        }
        [HttpPost]

        public ActionResult SuaTU(ThucUong tu, HttpPostedFileBase Image)
        {
            if (Image != null && Image.ContentLength > 0)
            {
                var fileName = Path.GetFileName(Image.FileName);
                var path = Path.Combine(Server.MapPath("~/img"), fileName);
                Image.SaveAs(path);

                tu.Image = fileName; // Lưu đường dẫn hình ảnh vào thuộc tính Image của đối tượng ThucUong
            }

            db.Entry(tu).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Menu");
        }



        [HttpPost]
        public ActionResult XoaTU(int id)
        {

            ThucUong tu = db.ThucUong.Find(id);
            db.ThucUong.Remove(tu);
            db.SaveChanges();
            return RedirectToAction("Menu");
        }
    }
}