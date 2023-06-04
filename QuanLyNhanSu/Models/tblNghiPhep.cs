namespace QuanLyNhanSu.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Globalization;

    [Table("tblNghiPhep")]
    public class tblNghiPhep
    {
        [Key]
        public int MaNghiPhep { get; set; }
        [StringLength(15)]
        public string MaNV { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Ngày bắt đầu")]
        public DateTime? NgayBatDau { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Ngày kết thúc")]
        public DateTime? NgayKetThuc { get; set; }

        public bool? Duyet { get; set; }
        public virtual tblThongTinNV tblThongTinNV { get; set; }
    }
}