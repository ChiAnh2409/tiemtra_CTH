﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TiemTra.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class QLTTEntities1 : DbContext
    {
        public QLTTEntities1()
            : base("name=QLTTEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<Bill> Bill { get; set; }
        public virtual DbSet<CongThuc> CongThuc { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<ChiTietBill> ChiTietBill { get; set; }
        public virtual DbSet<ChiTietPhieuNhap> ChiTietPhieuNhap { get; set; }
        public virtual DbSet<KhoNguyenLieu> KhoNguyenLieu { get; set; }
        public virtual DbSet<LoaiThucUong> LoaiThucUong { get; set; }
        public virtual DbSet<NguyenLieu> NguyenLieu { get; set; }
        public virtual DbSet<NhaCungCap> NhaCungCap { get; set; }
        public virtual DbSet<PhieuNhap> PhieuNhap { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<RoleUser> RoleUser { get; set; }
        public virtual DbSet<Size> Size { get; set; }
        public virtual DbSet<Staff> Staff { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<ThucUong> ThucUong { get; set; }
    }
}
