using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace QuanLyNhanSu.Models
{
    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=tblChamCong")
        {
        }
        
        public virtual DbSet<tblChamCongs> tblChamCongs { get; set; }
        public virtual DbSet<tblQuyen> TblQuyens { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
