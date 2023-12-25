using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiemTra.Models;
using TiemTra.ViewModel;

namespace TiemTra.Controllers
{
    public class PhieuNhapController : Controller
    {
        private QLTTEntities1 db = new QLTTEntities1();
        // GET: PhieuNhap
        public ActionResult PhieuNhap()
        {
            var list = new MutipleData();
            list.phieuNhaps = db.PhieuNhap.ToList();
            list.khoNguyenLieus = db.KhoNguyenLieu.ToList();
            list.bill = db.Bill.ToList();

            return View(list);
        }
        public ActionResult ThemPN()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ThemPN(PhieuNhap pn)
        {

            db.PhieuNhap.Add(pn);

            db.SaveChanges();
            return RedirectToAction("PhieuNhap");
        }
        public ActionResult SuaPN(int id)
        {
            PhieuNhap pn = db.PhieuNhap.Find(id);
            return View(pn);
        }
        [HttpPost]
        public ActionResult SuaPN(PhieuNhap pn, List<ChiTietPhieuNhap> listctpn)
        {
            
            db.Entry(pn).State = System.Data.Entity.EntityState.Modified;
           
            if (listctpn != null && listctpn.Count > 0)
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
                var u = db.PhieuNhap.Where(c => c.IdPN == (int)idpn).FirstOrDefault();
                u.TongTien = (decimal)tongtien;
            }
            else
            {
                db.SaveChanges();
                return RedirectToAction("PhieuNhap");
            }
            
            db.SaveChanges();
            return RedirectToAction("PhieuNhap");
        }
        [HttpPost]
        public ActionResult XoaPN(int id)
        {
            PhieuNhap pn = db.PhieuNhap.Find(id);
            db.PhieuNhap.Remove(pn);
            db.SaveChanges();
            return RedirectToAction("PhieuNhap");
        }
        public ActionResult ThanhToan(int id)
        {
            var tempPN = db.PhieuNhap.Where(c => c.IdPN == id).ToList();
            if (tempPN.Count > 0)
            {
                tempPN[0].TinhTrang = "Đã Duyệt";
                var listCTPN = db.ChiTietPhieuNhap.Where(c => c.IdPN == id).ToList();
                foreach (var i in listCTPN)
                {
                    // tìm xem có nguyen liệu cùng vs nhà cc nếu có + dồn
                    var tempNL = db.KhoNguyenLieu.Where(c => c.TenNL == i.IdNL && c.LoHang == i.LoHang).ToList();
                    if (tempNL.Count > 0)
                    {
                        if (tempNL[0].DVT.ToLower() =="g")
                        {
                            tempNL[0].SoLuongTon += i.SoLuong*50;
                        }
                        else if (tempNL[0].DVT.ToLower() == "ml")
                        {
                            tempNL[0].SoLuongTon += i.SoLuong * 1000;
                        }
                        else if (tempNL[0].DVT.ToLower() == "mieng")
                        {
                            tempNL[0].SoLuongTon += i.SoLuong * 40;
                        }
                        else if (tempNL[0].DVT.ToLower() == "chai")
                        {
                            tempNL[0].SoLuongTon += i.SoLuong * 500;
                        }
                        else if (tempNL[0].DVT.ToLower() == "bich")
                        {
                            tempNL[0].SoLuongTon += i.SoLuong * 1000;
                        }
                        else if (tempNL[0].DVT.ToLower() == "trai")
                        {
                            tempNL[0].SoLuongTon += i.SoLuong * 36;
                        }
                        else 
                        {
                            tempNL[0].SoLuongTon += i.SoLuong;
                        }
                        tempNL[0].TonKho += i.SoLuong;
                    }
                    else
                    {
                        //nếu k có nek thì tạo 1 cái nguyên liệu cũ vs nhà cc khác 
                        var tempNLnew = db.NguyenLieu.Where(c => c.IdNgL == i.IdNL).FirstOrDefault();
                        var tempNLnew2 = db.KhoNguyenLieu.FirstOrDefault(c => c.NguyenLieu.IdNgL == i.IdNL);
                        KhoNguyenLieu NL = new KhoNguyenLieu();
                        NL.IdNL = tempNLnew.IdNgL;
                        NL.LoHang = i.LoHang;
                        NL.TenNL = tempNLnew.IdNgL;
                        NL.IdNhaCC = i.IdNhaCC;
                        NL.TonKho = i.SoLuong;
                        NL.HanSuDung = i.HanSuDung;
                        
                        if (tempNLnew2 != null)
                        {
                            switch (tempNLnew2.DVT.ToLower())
                            {
                                case "g":
                                    NL.SoLuongTon = i.SoLuong * 50;
                                    NL.DVT = tempNLnew2.DVT;
                                    break;
                                case "ml":
                                    NL.SoLuongTon = i.SoLuong * 1000;
                                    NL.DVT = tempNLnew2.DVT;
                                    break;
                                case "mieng":
                                    NL.SoLuongTon = i.SoLuong * 40;
                                    NL.DVT = tempNLnew2.DVT;
                                    break;
                                case "chai":
                                    NL.SoLuongTon = i.SoLuong * 500;
                                    NL.DVT = tempNLnew2.DVT;
                                    break;
                                case "bich":
                                    NL.SoLuongTon = i.SoLuong * 1000;
                                    NL.DVT = tempNLnew2.DVT;
                                    break;
                                case "trai":
                                    NL.SoLuongTon = i.SoLuong * 36;
                                    NL.DVT = tempNLnew2.DVT;
                                    break;
                                default:
                                    NL.SoLuongTon = i.SoLuong; // Giữ nguyên giá trị ban đầu
                                    NL.DVT = tempNLnew2.DVT;
                                    break;
                            }
                        }
                        else
                        {
                            NL.DVT = "null";
                            NL.SoLuongTon += i.SoLuong;
                        }
                        
                        db.KhoNguyenLieu.Add(NL);
                    }
                    db.SaveChanges();
                }
            }
            return RedirectToAction("PhieuNhap");
        }
    }
}