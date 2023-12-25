using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TiemTra.Models;
using TiemTra.ViewModel;

namespace TiemTra.Controllers
{
    public class CartController : Controller
    {
        private QLTTEntities1 db = new QLTTEntities1();

        // Hiển thị giỏ hàng
        public ActionResult Cart()
        {
            var list = new MutipleData();
            list.chiTietBill = db.ChiTietBill.Include("ThucUong");
            list.chiTietBill = db.ChiTietBill.Include("Bill");

            list.thucUongs = db.ThucUong.ToList();
            list.bill = db.Bill.ToList();

            return View();
        }

        [HttpPost]
        public ActionResult SaveChiTietBill(List<ChiTietBill> chiTietBillData)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    foreach (var item in chiTietBillData)
                    {
                       
                        var chiTietBill = new ChiTietBill
                        {
                            //IdBill = item.IdBill,
                            IdTU = item.IdTU,
                            Size = item.Size,
                            SoLuong = item.SoLuong,
                            ThanhTien = item.ThanhTien
                        };

                        db.ChiTietBill.Add(chiTietBill);
                    }

                    db.SaveChanges();

                    return Json(new { success = true, message = "" });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = $"Error saving ChiTietBill data: {ex.Message}" });
                }
            }

            return Json(new { success = false, message = "Invalid data received" });
        }


    }
}
