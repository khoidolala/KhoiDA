namespace QuanLyNhanSu.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblThuong")]
    public partial class tblThuong
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblThuong()
        {
            tblLuongs = new HashSet<tblLuong>();
        }

        [Key]
        [StringLength(15)]
        public string MaThuong { get; set; }

        public double? TienThuong { get; set; }
        [StringLength(50)]
        public string LoaiThuong { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblLuong> tblLuongs { get; set; }
    }
}
