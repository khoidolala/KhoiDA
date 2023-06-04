namespace QuanLyNhanSu.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblThongTinNV")]
    public partial class tblThongTinNV
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblThongTinNV()
        {
            tblChamCongs = new HashSet<tblChamCongs>();
            tblLuongs = new HashSet<tblLuong>();
        }

        [Key]
        [StringLength(15)]
        public string MaNV { get; set; }

        [StringLength(50)]
        [Display(Name ="Họ và tên")]
        public string TenNV { get; set; }

        [StringLength(50)]
        [Display(Name = "Số CMT/CCCD")]
        public string CMT { get; set; }

        [StringLength(3)]
        [Display(Name = "Giới tính")]
        public string GioiTinh { get; set; }

        [Column(TypeName = "date")]
        [Display(Name = "Ngày sinh")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? NgaySinh { get; set; }

        [StringLength(50)]
        [Display(Name = "Địa chỉ")]
        public string DiaChi { get; set; }

        [StringLength(50)]
        [Display(Name = "Chức vụ")]
        public string ChucVu { get; set; }

        [StringLength(50)]
        [Display(Name = "Trình độ")]
        public string TrinhDo { get; set; }

        [StringLength(15)]
        [Display(Name = "Hệ số lương")]
        public string MaHSL { get; set; }

        [StringLength(50)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [StringLength(15)]
        [Display(Name = "Số điện thoại")]
        public string SDT { get; set; }

        [StringLength(15)]
        [Display(Name = "Phòng/ ban")]
        public string MaPhong { get; set; }
        [Display(Name = "Mã BHXH")]
        public double? BHXH { get; set; }

        [StringLength(50)]
        [Display(Name = "Dân tộc")]
        public string DanToc { get; set; }

        [StringLength(50)]
        [Display(Name = "Tôn giáo")]
        public string TonGiao { get; set; }

        [StringLength(200)]
        [Display(Name = "Ảnh đại diện")]
        public string UrlAnh { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblChamCongs> tblChamCongs { get; set; }

        public virtual tblHSL tblHSL { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblLuong> tblLuongs { get; set; }

        public virtual tblPhong tblPhong { get; set; }
        
        public virtual ICollection<tblNghiPhep> TblNghiPheps { get; set; }
    }
}
