namespace QuanLyNhanSu.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    public partial class tblUser
    {
        [Required]
        [StringLength(30)]
        public string UserName { get; set; }

        [StringLength(50)]
        public string Password { get; set; }

        public int? Quyen { get; set; }

        [StringLength(15)]
        public string MaNV { get; set; }

        [StringLength(50)]
        public string TenNV { get; set; }

        public bool? TrangThai { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IDUser { get; set; }
        /*//Thuộc tính mới
        [NotMapped]
        public string ConnectionId { get; set; }
        [NotMapped]
        public bool IsOnline { get; set; }*/
    }
}
