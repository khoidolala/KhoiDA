namespace QuanLyNhanSu.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblChamCongs")]
    public partial class tblChamCongs
    {
        public int ID { get; set; }

        [StringLength(15)]
        [Display(Name ="Mã nhân viên")]
        public string MaNV { get; set; }

        [StringLength(50)]
        [Display(Name = "Tên nhân viên")]
        public string TenNV { get; set; }

        [Column(TypeName = "date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Ngày")]
        public DateTime? Ngay { get; set; }
        [Display(Name = "Giờ vào làm")]
        public TimeSpan? ThoiGianVao { get; set; }
        [Display(Name = "Giờ tan làm")]
        public TimeSpan? ThoiGianRa { get; set; }

        [StringLength(50)]
        [Display(Name = "Trạng thái")]
        public string TrangThai { get; set; }

        public virtual tblThongTinNV tblThongTinNV { get; set; }
    }
}
