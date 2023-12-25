using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiemTra.Models;
using TiemTra.ViewModel;

namespace TiemTra.Controllers
{
    public class BillController : Controller
    {
        // GET: Bill
        QLTTEntities1 db = new QLTTEntities1();
        public ActionResult Bill()
        {
            var list = new MutipleData();
            list.bill = db.Bill.Include("Customer");
            
            list.phieuNhaps = db.PhieuNhap.ToList();
            list.khoNguyenLieus = db.KhoNguyenLieu.ToList();
            list.customer = db.Customer.ToList();
            list.chiTietBill = db.ChiTietBill.ToList();
            return View(list);
        }

        public ActionResult Duyet(int id)
        {
            var temp = db.Bill.Where(c => c.IdBill == id).ToList();
            if (temp.Count > 0)
            {
                temp[0].TinhTrang = "Đã Duyệt";

                var listctbill = db.ChiTietBill.Where(c => c.IdBill == id).ToList();

                foreach (var i in listctbill)
                {
                    var tempTU = db.ThucUong.Where(c => c.IdTU == i.IdTU && c.IdSize == i.ThucUong.IdSize).FirstOrDefault();

                    if (tempTU != null)
                    {
                        var tempCT = db.CongThuc.Where(c => c.IdTU == tempTU.IdTU).ToList();

                        foreach (var s in tempCT)
                        {
                            if (tempCT.Count > 0)
                            {
                                var kho = db.KhoNguyenLieu.Where(c => c.TenNL == s.IdNL).OrderBy(c => c.HanSuDung).ToList();

                                int remainingQuantity = i.SoLuong;
                                int remainingFromLastBatch = 0; // Số lượng còn thiếu từ lô hàng trước đó

                                foreach (var loHang in kho)
                                {
                                    if (remainingQuantity <= 0)
                                        break;

                                    if (loHang.SoLuongTon.HasValue && loHang.SoLuongTon > 0)
                                    {
                                        var amountToSubtract = s.SoLuong * i.SoLuong;

                                        // Trừ số lượng còn thiếu từ lô hàng trước đó
                                        var a = loHang.SoLuongTon - remainingFromLastBatch;

                                        if (loHang.SoLuongTon >= amountToSubtract && remainingFromLastBatch == 0)
                                        {
                                            loHang.SoLuongTon -= amountToSubtract;
                                            remainingQuantity = 0; // Đã cung cấp đủ số lượng
                                            break;
                                        }
                                        else if  (loHang.SoLuongTon >= amountToSubtract && remainingFromLastBatch != 0)
                                        {
                                            loHang.SoLuongTon = a;
                                            remainingQuantity = 0;
                                            
                                        }
                                        else if (loHang.SoLuongTon > 0) // Xử lý trường hợp số lượng tồn của lô hàng không đủ để trừ hết
                                        {
                                            remainingFromLastBatch = amountToSubtract - loHang.SoLuongTon.Value; // Lưu lại số lượng còn thiếu cho lần lặp tiếp theo
                                            loHang.SoLuongTon = 0; // Đặt số lượng tồn của lô hàng hiện tại = 0
                                            float result = (float)amountToSubtract / s.SoLuong;
                                            remainingQuantity = (int)Math.Ceiling(result);
                                        }
                                    }
                                }

                            }
                        }
                    }
                }
                db.SaveChanges();
            }
            return RedirectToAction("Bill");
        }

    }

}