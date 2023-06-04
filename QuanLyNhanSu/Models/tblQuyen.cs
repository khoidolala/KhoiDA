namespace QuanLyNhanSu.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblQuyen")]
    public partial class tblQuyen
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Quyen { get; set; }

        [StringLength(50)]
        public string TenQuyen { get; set; }

        [StringLength(200)]
        public string MoTa { get; set; }
    }
}
