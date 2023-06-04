using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace QuanLyNhanSu.Models
{
    public partial class DBContext : DbContext
    {
        public DBContext()
            : base("name=DBContext")
        {
        }

        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<tblHSL> tblHSLs { get; set; }
        public virtual DbSet<tblLuong> tblLuongs { get; set; }
        public virtual DbSet<tblPhong> tblPhongs { get; set; }
        public virtual DbSet<tblPhuCap> tblPhuCaps { get; set; }
        public virtual DbSet<tblThongTinNV> tblThongTinNVs { get; set; }
        public virtual DbSet<tblThuong> tblThuongs { get; set; }
        public virtual DbSet<tblUser> tblUsers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tblThongTinNV>()
                .Property(e => e.GioiTinh)
                .IsFixedLength();
        }

        public System.Data.Entity.DbSet<QuanLyNhanSu.Models.tblChamCongs> tblChamCongs { get; set; }

        public System.Data.Entity.DbSet<QuanLyNhanSu.Models.tblQuyen> tblQuyens { get; set; }
        public System.Data.Entity.DbSet<QuanLyNhanSu.Models.tblNghiPhep> tblNghiPhep { get; set; }
    }
    
}
