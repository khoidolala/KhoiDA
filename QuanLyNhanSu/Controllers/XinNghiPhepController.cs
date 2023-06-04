using QuanLyNhanSu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyNhanSu.Controllers
{
    public class XinNghiPhepController : Controller
    {
        private DBContext db = new DBContext();
        // GET: XinNghiPhep
        public ActionResult Index()
        {
            var manv = (string)Session["MaNV"];
            if (manv == "NV11")
            {
                var list = db.tblNghiPhep.ToList();
                ViewBag.quyen = db.tblUsers.SingleOrDefault(x => x.MaNV == manv).Quyen;
                return View(list);
            }
            else
            {
                var list = db.tblNghiPhep.Where(x=>x.MaNV == manv).ToList();
                ViewBag.quyen = db.tblUsers.SingleOrDefault(x => x.MaNV == manv).Quyen;
                return View(list);
            }
        }

        public ActionResult ThemMoi()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ThemMoi(DateTime start, DateTime end)
        {
            tblNghiPhep tblNghiPhep = new tblNghiPhep();
            tblNghiPhep.NgayBatDau = start;
            tblNghiPhep.NgayKetThuc = end;
            tblNghiPhep.MaNV = (string)Session["MaNV"];
            tblNghiPhep.Duyet = false;
            if (tblNghiPhep.NgayBatDau > tblNghiPhep.NgayKetThuc)
            {
                ViewBag.Message = "Ngày bắt đầu phải nhỏ hơn ngày kết thúc";
                return View();
            }
            db.tblNghiPhep.Add(tblNghiPhep);
            db.SaveChanges();

            return Redirect("/XinNghiPhep/Index");
        }

        public ActionResult Duyet(int id)
        {
            var nghiphep = db.tblNghiPhep.Find(id);
            nghiphep.Duyet = true;

            db.SaveChanges();
            return Redirect("/XinNghiPhep/Index");
        }
        public ActionResult Xoa(int id)
        {
            var nghiphep = db.tblNghiPhep.Find(id);
            db.tblNghiPhep.Remove(nghiphep);
            db.SaveChanges();
            return Redirect("/XinNghiPhep/Index");
        }
    }
}