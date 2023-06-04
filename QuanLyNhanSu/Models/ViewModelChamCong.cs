using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyNhanSu.Models
{
    public class ViewModelChamCong
    {
        public tblChamCongs ChamCong { get; set; }
        public IEnumerable<tblChamCongs> DanhSachChamCong { get; set; }
    }
}