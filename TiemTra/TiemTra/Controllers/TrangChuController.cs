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
    public class TrangChuController : Controller
    {
        // GET: TrangChu
        private QLTTEntities1 db = new QLTTEntities1();
        public ActionResult Index()
        {
            var list = new MutipleData();
             list.thucUongs = db.ThucUong.Include("LoaiThucUong").Include("Size").ToList();
            
            return View(list);
        }

        public ActionResult ThongTin()
        {
         
                return View();
        }

            [HttpPost]

        public ActionResult ThongTin(Customer c)
            {
           db.Customer.Add(c);
            db.SaveChanges();
                     return View();
             }

        [HttpPost]
        public ActionResult ThanhToan(List<ChiTietBill> ctb)

        {
            var idbll = ctb[0].IdBill;
            var listCTBill = db.ChiTietBill.Where(c => c.IdBill == (int)idbll).ToList();
            double tongtien = 0;
            foreach (var i in listCTBill)
            {
                tongtien += (double)i.ThanhTien;
            }

            foreach (var i in ctb)
            {
                tongtien += (double)i.ThanhTien;
            }
            var bill = db.Bill.Where(c => c.IdBill == (int)idbll).FirstOrDefault();
            bill.TongTien = (decimal)tongtien;
            bill.TinhTrang = "chưa duyệt";
            db.ChiTietBill.AddRange(ctb);
            db.SaveChanges();
            bool isSuccess = true; // Giả sử lưu thành công
            string message = "Đã lưu giỏ hàng thành công";

            return Json(new { success = isSuccess, message = message });

        }
        [HttpPost]
        public ActionResult LuuBill(Customer customerInfo, Bill model, ChiTietBill ctb) // The action method with Customer object as a parameter
        {
            if (ModelState.IsValid)
            {
                // Lưu thông tin khách hàng từ form "Thông tin"
                Customer customer = new Customer
                {
                    IdKH = customerInfo.IdKH,
                    TenKH = customerInfo.TenKH,
                    Sdt = customerInfo.Sdt,
                    DiaChi = customerInfo.DiaChi,
                    Email = customerInfo.Email,
                    GhiChu = customerInfo.GhiChu
                    // ... (other customer details)
                };

                db.Customer.Add(customer);
                db.SaveChanges();

                // Lấy thông tin khách hàng đã lưu để sử dụng cho Bill
                int customerId = customer.IdKH;

                // Lưu thông tin vào bảng Bill
                Bill bill = new Bill
                {
                    IdKH = customerId,
                    Sdt = customerInfo.Sdt,
                    DiaChi = customerInfo.DiaChi,
                    GhiChu = customerInfo.GhiChu,
                    NgayBan = DateTime.Now,
                    TinhTrang = "chưa duyệt"
                    // ... (other bill details)
                };

                db.Bill.Add(bill);
                db.SaveChanges();

                // Retrieve the generated IdBill from the just-inserted Bill record
                int newBillId = bill.IdBill;

                // Process the cart items and save them to ChiTietBill table
               
                var ctbill = db.ChiTietBill.Where(c => c.IdBill == null).ToList();
                foreach (var item in ctbill)
                {

                    item.IdBill = newBillId; // Use the newly generated IdBill
                    
                 }
                 

                // Save the details to the ChiTietBill table
                
                db.SaveChanges();
                
                
                var ct = db.ChiTietBill.Where( c => c.IdBill == newBillId).ToList();
               
                    var tongtien = 0;
                    foreach (var item in ct)
                    {
                        tongtien += (int)item.ThanhTien;

                    }

                    var b = db.Bill.Where(c => c.IdBill == newBillId).FirstOrDefault();
                    b.TongTien = tongtien;
                    db.SaveChanges();
                bool isSuccess = true; // Giả sử giá trị thành công là true

                return Json(new { success = isSuccess, redirectUrl = "/TrangChu/Index", message = "Đặt hàng thành công" }, JsonRequestBehavior.AllowGet);

            }

            return Json(new { success = false, message = "Lỗi khi lưu thông tin" });
        }




    }
}