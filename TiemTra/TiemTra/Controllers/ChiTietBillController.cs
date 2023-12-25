using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiemTra.Models;
using TiemTra.ViewModel;

namespace TiemTra.Controllers
{
    public class ChiTietBillController : Controller
    {
        // GET: ChiTietBill
        QLTTEntities1 db = new QLTTEntities1();
        public ActionResult ChiTietBill(int id)
        {
            var list = new MutipleData();
            list.chiTietBill = db.ChiTietBill.Include("Bill");
            list.chiTietBill = db.ChiTietBill.Include("ThucUong");
            list.phieuNhaps = db.PhieuNhap.ToList();
            list.khoNguyenLieus = db.KhoNguyenLieu.ToList();
            list.customer = db.Customer.ToList();
            list.bill = db.Bill.ToList();

            list.chiTietBill = db.ChiTietBill.ToList();
            list.chiTietBill = db.ChiTietBill.Where(c => c.IdBill == id).ToList();
            return View(list);
        }
    }
}