namespace QuanLyNhanSu.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Globalization;

    [Table("tblLuong")]
    public partial class tblLuong
    {
        [Key]
        [StringLength(15)]
        public string MaLuong { get; set; }

        public int? Thang { get; set; }

        [StringLength(15)]
        public string MaNV { get; set; }

        public int? SoNgayLamViec { get; set; }

        public int? SoGioLamViec { get; set; }

        [StringLength(15)]
        public string MaHSL { get; set; }

        [StringLength(15)]
        public string MaThuong { get; set; }

        [StringLength(15)]
        public string MaPhuCap { get; set; }

        public double? TienPhat { get; set; }

        public double? TamUng { get; set; }

        public virtual tblHSL tblHSL { get; set; }

        public virtual tblPhuCap tblPhuCap { get; set; }

        public virtual tblThongTinNV tblThongTinNV { get; set; }

        public virtual tblThuong tblThuong { get; set; }
        public string TongLuong()
        {
            double Luong = (double)(tblHSL.HSL*1550000/30*SoNgayLamViec+tblPhuCap.TienPhuCap+tblThuong.TienThuong-TienPhat-TamUng);
            return Luong.ToString("C0", CultureInfo.CreateSpecificCulture("vi-VN"));
        }
    }
}
