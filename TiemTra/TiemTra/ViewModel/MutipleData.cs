using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using TiemTra.Models;

namespace TiemTra.ViewModel
{
    public class MutipleData
    {
        public IEnumerable<Account> Accounts { get; set; }
        public IEnumerable<Staff> Staff { get; set; }
        public IEnumerable<NhaCungCap> nhaCungCap { get; set; }
        public IEnumerable<KhoNguyenLieu> khoNguyenLieus { get; set; }
        public IEnumerable<PhieuNhap> phieuNhaps { get; set; }
        public IEnumerable<ChiTietPhieuNhap> chiTietPhieuNhaps { get; set; }
        public IEnumerable<LoaiThucUong> loaiThucUongs { get; set; }
        public IEnumerable<Size> sizes { get; set; }
        public IEnumerable<CongThuc> congThucs { get; set; }
        public IEnumerable<ThucUong> thucUongs { get; set; }

      
        public IEnumerable<Customer> customer { get; set; }
        public IEnumerable<NguyenLieu> nguyenLieus { get; set; }


     
        public IEnumerable<Bill> bill { get; set; }
        public IEnumerable<ChiTietBill> chiTietBill { get; set;
        }

    }
}