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
    public class CTPNController : Controller
    {
        private QLTTEntities1 db = new QLTTEntities1();
        // GET: CTPN
        public ActionResult CTPN(int id)
        {
            var list = new MutipleData();
            list.chiTietPhieuNhaps = db.ChiTietPhieuNhap.Include("PhieuNhap");
            list.chiTietPhieuNhaps = db.ChiTietPhieuNhap.Include("NguyenLieu");
            list.chiTietPhieuNhaps = db.ChiTietPhieuNhap.Include("NhaCungCap");
            //list.chiTietPhieuNhaps = db.ChiTietPhieuNhap.Include("ChiTietPhieuNhap");
            list.phieuNhaps = db.PhieuNhap.ToList();
            list.chiTietPhieuNhaps = db.ChiTietPhieuNhap.Where(c => c.IdPN == id).ToList();
            list.khoNguyenLieus = db.KhoNguyenLieu.ToList();
            list.nhaCungCap = db.NhaCungCap.ToList();
            list.bill = db.Bill.ToList();

            return View(list);
        }
        public ActionResult ThemCTPN(int id)
        {
            var list = new MutipleData();
            list.chiTietPhieuNhaps = db.ChiTietPhieuNhap.Include("PhieuNhap");

            list.chiTietPhieuNhaps = db.ChiTietPhieuNhap.Include("NhaCungCap");
            list.chiTietPhieuNhaps = db.ChiTietPhieuNhap.Include("NguyenLieu");

            list.phieuNhaps = db.PhieuNhap.Where(c => c.IdPN == id).ToList();
            list.nguyenLieus = db.NguyenLieu.ToList();
            list.nhaCungCap = db.NhaCungCap.ToList();

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            ViewBag.MultipleDataJson = JsonConvert.SerializeObject(list, settings);
            return View(list);
        }

        [HttpPost]
        public ActionResult ThemCTPN(List<ChiTietPhieuNhap> listctpn)
        {
            var idpn = listctpn[0].IdPN;
            var listCTPN = db.ChiTietPhieuNhap.Where(c => c.IdPN == (int)idpn).ToList();
            double tongtien = 0;
            foreach (var i in listCTPN)
            {
                tongtien += (double)i.ThanhTien;
            }

            foreach (var i in listctpn)
            {
                tongtien += (double)i.ThanhTien;
            }
            var pn = db.PhieuNhap.Where(c => c.IdPN == (int)idpn).FirstOrDefault();
            pn.TongTien = (decimal)tongtien;
            pn.TinhTrang = "chưa duyệt";
            db.ChiTietPhieuNhap.AddRange(listctpn);
            db.SaveChanges();
            bool isSuccess = true; // Giả sử giá trị thành công là true

            return Json(new { success = isSuccess, redirectUrl = "/PhieuNhap/PhieuNhap", message = "Thêm chi tiết phiếu nhập thành công" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SuaCTPN(int id)
        {
            var viewmodel = new MutipleData();
            viewmodel.chiTietPhieuNhaps = db.ChiTietPhieuNhap.Where(ctpn => ctpn.IdCTPN == id).ToList();
            viewmodel.nguyenLieus = db.NguyenLieu.ToList();
            viewmodel.nhaCungCap = db.NhaCungCap.ToList();
            return View(viewmodel);
        }
        [HttpPost]
        public ActionResult SuaCTPN(ChiTietPhieuNhap ctpn)
        {
            db.Entry(ctpn).State = System.Data.Entity.EntityState.Modified;
            var pnTemp = db.ChiTietPhieuNhap.Where(c => c.IdCTPN == ctpn.IdCTPN && c.IdPN == ctpn.IdPN).FirstOrDefault();
            pnTemp.LoHang = ctpn.LoHang;
            pnTemp.IdNL = ctpn.IdNL;
            pnTemp.IdNhaCC = ctpn.IdNhaCC;
            pnTemp.GiaNhap = ctpn.GiaNhap;
            pnTemp.SoLuong = ctpn.SoLuong;
            pnTemp.ThanhTien = ctpn.ThanhTien;
            List<ChiTietPhieuNhap> listctpn = db.ChiTietPhieuNhap.Where(c => c.IdPN == ctpn.IdPN).ToList();
            
            double tongtien = 0;
            foreach (var i in listctpn)
            {
                tongtien += (double)i.ThanhTien;
            }
            var pn = db.PhieuNhap.Where(c => c.IdPN == (int)ctpn.IdPN).FirstOrDefault();
            pn.TongTien = (decimal)tongtien;
            pn.TinhTrang = "chưa duyệt";
            db.SaveChanges();
            return RedirectToAction("CTPN/" + ctpn.IdPN);
        }
        public ActionResult XoaCTPN(int id)
        {
            ChiTietPhieuNhap ctpn = db.ChiTietPhieuNhap.Find(id);
            var tempNL = db.NguyenLieu.Where(c => c.IdNgL == ctpn.IdNL).ToList();
            db.ChiTietPhieuNhap.Remove(ctpn);
            db.SaveChanges();
            return RedirectToAction("CTPN");
        }
    }
}