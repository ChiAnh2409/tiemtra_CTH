using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiemTra.Models;
using TiemTra.ViewModel;

namespace TiemTra.Controllers
{
    public class TKNVController : Controller
    {
        private QLTTEntities1 db = new QLTTEntities1();
        // GET: TKNV
        public ActionResult TKNV()
        {
            var list = new MutipleData();
            list.Accounts = db.Account.Include("Staff");
            list.Staff = db.Staff.ToList();
            list.phieuNhaps = db.PhieuNhap.ToList();
            list.bill = db.Bill.ToList();

            list.khoNguyenLieus = db.KhoNguyenLieu.ToList();
            return View(list);
        }

        public ActionResult ThemTK()
        {
            var list = new MutipleData();
            list.Accounts = db.Account.Include("Staff");
            list.Staff = db.Staff.ToList();
            return View(list);
        }
        [HttpPost]
        public ActionResult ThemTK(Account nl)
        {

            db.Account.Add(nl);
            db.SaveChanges();
            return RedirectToAction("TKNV");

        }

        public ActionResult SuaTK(int id)
        {
            var viewmodel = new MutipleData();
            viewmodel.Accounts = db.Account.Where(nl => nl.IdAccount == id).ToList();
            viewmodel.Staff = db.Staff.ToList();
            return View(viewmodel);
        }
        [HttpPost]
        public ActionResult SuaTK(Account ngl)
        {

            db.Entry(ngl).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("TKNV");
        }
        [HttpPost]
        public ActionResult XoaTK(int id)
        {

            Account ngl = db.Account.Find(id);
            db.Account.Remove(ngl);
            db.SaveChanges();
            return RedirectToAction("TKNV");
        }
    }
}