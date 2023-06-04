using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PagedList;
using QuanLyNhanSu.Models;


namespace QuanLyNhanSu.Controllers
{
    public class QLLuongsController : Controller
    {
        private DBContext db = new DBContext();


        // GET: QLLuongs
        public ActionResult Index(string searchName, int? searchMonth, int page = 1)
        {
            var tblLuongs = db.tblLuongs.Include(t => t.tblPhuCap).Include(t => t.tblThongTinNV).Include(t => t.tblThuong).OrderBy(b => b.MaNV);
            page = page < 1 ? 1 : page;
            int pagesize = 5;
            if (!string.IsNullOrEmpty(searchName))
            {
                tblLuongs = db.tblLuongs.Include(t => t.tblPhuCap).Include(t => t.tblThongTinNV).Include(t => t.tblThuong).Where(l => l.tblThongTinNV.TenNV.Contains(searchName)).OrderBy(b => b.MaNV);
            }

            if (searchMonth.HasValue)
            {
                tblLuongs = db.tblLuongs.Where(s => s.Thang == searchMonth.Value).OrderBy(x => x.MaNV);
            }
            if (!string.IsNullOrEmpty(searchName) && searchMonth.HasValue)
            {
                tblLuongs = db.tblLuongs.Include(t => t.tblPhuCap).Include(t => t.tblThongTinNV).Include(t => t.tblThuong).Where(l => l.tblThongTinNV.TenNV.Contains(searchName) && l.Thang == searchMonth.Value).OrderBy(x => x.MaNV);
            }
            return View(tblLuongs.ToPagedList(page, pagesize));
        }

        // GET: QLLuongs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblLuong tblLuong = db.tblLuongs.Find(id);
            if (tblLuong == null)
            {
                return HttpNotFound();
            }
            return View(tblLuong);
        }

        // GET: QLLuongs/Create
        public ActionResult Create()
        {
            ViewBag.MaHSL = new SelectList(db.tblHSLs, "MaHSL", "HSL");
            ViewBag.MaPhuCap = new SelectList(db.tblPhuCaps, "MaPhuCap", "MaPhuCap");
            ViewBag.MaNV = new SelectList(db.tblThongTinNVs, "MaNV", "TenNV");
            ViewBag.MaThuong = new SelectList(db.tblThuongs, "MaThuong", "MaThuong");

            return View();
        }

        // POST: QLLuongs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaLuong,Thang,MaNV,SoNgayLamViec,SoGioLamViec,MaHSL,MaThuong,MaPhuCap,TienPhat,TamUng")] tblLuong tblLuong)
        {
            if (ModelState.IsValid)
            {
                var listngaynghi = db.tblNghiPhep.Where(x => x.MaNV == tblLuong.MaNV && x.Duyet == true).ToList();
                int ngaynghi = 0;
                foreach (var item in listngaynghi)
                {
                    TimeSpan t = item.NgayKetThuc.Value - item.NgayBatDau.Value;
                    int songay = Convert.ToInt32(t.TotalDays);
                    ngaynghi += songay;
                }
                tblLuong.SoNgayLamViec += ngaynghi;
                db.tblLuongs.Add(tblLuong);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaPhuCap = new SelectList(db.tblPhuCaps, "MaPhuCap", "TienPhuCap", tblLuong.MaPhuCap);
            ViewBag.MaNV = new SelectList(db.tblThongTinNVs, "MaNV", "TenNV", tblLuong.MaNV);
            ViewBag.MaThuong = new SelectList(db.tblThuongs, "MaThuong", "TienThuong", tblLuong.MaThuong);
            return View(tblLuong);
        }

        // GET: QLLuongs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblLuong tblLuong = db.tblLuongs.Find(id);
            if (tblLuong == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaPhuCap = new SelectList(db.tblPhuCaps, "MaPhuCap", "TienPhuCap", tblLuong.MaPhuCap);
            ViewBag.MaNV = new SelectList(db.tblThongTinNVs, "MaNV", "TenNV", tblLuong.MaNV);
            ViewBag.MaThuong = new SelectList(db.tblThuongs, "MaThuong", "LoaiThuong", tblLuong.MaThuong);
            return View(tblLuong);
        }

        // POST: QLLuongs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaLuong,Thang,MaNV,SoNgayLamViec,SoGioLamViec,HSL,MaThuong,MaPhuCap,TienPhat,TamUng")] tblLuong tblLuong)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblLuong).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaPhuCap = new SelectList(db.tblPhuCaps, "MaPhuCap", "MaPhuCap", tblLuong.MaPhuCap);
            ViewBag.MaNV = new SelectList(db.tblThongTinNVs, "MaNV", "TenNV", tblLuong.MaNV);
            ViewBag.MaThuong = new SelectList(db.tblThuongs, "MaThuong", "MaThuong", tblLuong.MaThuong);
            return View(tblLuong);
        }

        // GET: QLLuongs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblLuong tblLuong = db.tblLuongs.Find(id);
            if (tblLuong == null)
            {
                return HttpNotFound();
            }
            return View(tblLuong);
        }

        // POST: QLLuongs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            tblLuong tblLuong = db.tblLuongs.Find(id);
            db.tblLuongs.Remove(tblLuong);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public FileResult ExportToExcel(int? month)
        {
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[11] { new DataColumn("Mã lương"),
                                                     new DataColumn("ID"),
                                                     new DataColumn("Họ và tên"),
                                                     new DataColumn("Tháng"),

                                                     new DataColumn("Số ngày làm việc"),
                                                     new DataColumn("Số giờ làm việc"),
                                                     new DataColumn("Mã hệ số lương"),
                                                     new DataColumn("Mã phụ cấp"),
                                                     new DataColumn("Tiền phạt"),
                                                     new DataColumn("Tạm ứng"),
                                                     new DataColumn("Tổng lương"),});

            var insuranceCertificate = from tblLuong in db.tblLuongs
                                       join tblThongTinNV in db.tblThongTinNVs on tblLuong.MaNV equals tblThongTinNV.MaNV
                                       where tblLuong.Thang == month
                                       select new { tblLuong, tblThongTinNV };

            foreach (var insurance in insuranceCertificate)
            {
                dt.Rows.Add(insurance.tblLuong.MaLuong, insurance.tblThongTinNV.MaNV, insurance.tblThongTinNV.TenNV, insurance.tblLuong.Thang, insurance.tblLuong.SoNgayLamViec,
                    insurance.tblLuong.SoGioLamViec, insurance.tblLuong.MaHSL, insurance.tblLuong.MaPhuCap, insurance.tblLuong.TienPhat, insurance.tblLuong.TamUng, insurance.tblLuong.TongLuong());
            }

            using (XLWorkbook wb = new XLWorkbook()) //Install ClosedXml from Nuget for XLWorkbook  
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream()) //using System.IO;  
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachLuong.xlsx");
                }
            }
        }
        public ActionResult DownloadPDF(string id)
        {
            // Lấy thông tin lương của nhân viên với ID được truyền vào
            tblLuong tblLuong = db.tblLuongs.Find(id);

            // Tạo file PDF
            MemoryStream stream = new MemoryStream();
            Document pdfDoc = new Document(PageSize.A4, 50, 50, 25, 25);
            PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
            BaseFont timesNewRoman = BaseFont.CreateFont("C:/Users/Admin/Desktop/TTTN/QuanLyNhanSu/QuanLyNhanSu/fonts/font-times-new-roman/times-new-roman-14.ttf", BaseFont.IDENTITY_H, true);
            Font font = new Font(timesNewRoman, 14, Font.NORMAL);
            pdfDoc.Open();

            // Thêm thông tin lương vào file PDF
            Paragraph para = new Paragraph("Thông tin lương của nhân viên", font);
            pdfDoc.Add(para);

            para = new Paragraph("Mã nhân viên: " + tblLuong.MaNV, font);
            pdfDoc.Add(para);

            para = new Paragraph("Tên nhân viên: " + tblLuong.tblThongTinNV.TenNV, font);
            pdfDoc.Add(para);

            para = new Paragraph("Hệ số lương: " + tblLuong.tblHSL.HSL.ToString(), font);
            pdfDoc.Add(para);

            para = new Paragraph("Số ngày làm việc: " + tblLuong.SoNgayLamViec.ToString(), font);
            pdfDoc.Add(para);

            para = new Paragraph("số giờ làm việc: " + tblLuong.SoGioLamViec.ToString(), font);
            pdfDoc.Add(para);

            para = new Paragraph("Phụ cấp: " + tblLuong.tblPhuCap.TienPhuCap.ToString(), font);
            pdfDoc.Add(para);

            para = new Paragraph("Tiền thưởng: " + tblLuong.tblThuong.TienThuong.ToString() + "đ", font);
            pdfDoc.Add(para);

            para = new Paragraph("Tiền phạt: " + tblLuong.TienPhat.ToString() + "đ", font);
            pdfDoc.Add(para);

            para = new Paragraph("Tạm ứng: " + tblLuong.TamUng.ToString() + "đ", font);
            pdfDoc.Add(para);

            para = new Paragraph("Tổng lương: " + tblLuong.TongLuong().ToString(), font);
            pdfDoc.Add(para);

            //font chữ, căn chỉnh
            /*Font font = new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL);
            para.setFont(font);

            // Thiết lập kiểu căn chỉnh
            para.setAlignment(Element.ALIGN_CENTER);

            // Thiết lập độ lề trái
            Paragraph.setIndentationLeft(20);*/
            // Kết thúc tạo file PDF
            pdfDoc.Close();
            writer.Close();

            // Gửi file PDF cho người dùng để tải về
            byte[] file = stream.ToArray();
            return File(file, "application/pdf", "employee-salary.pdf");
        }
        public ActionResult LuongNV(string ma)
        {
            ma = Session["MaNV"] as string;
            List<tblLuong> listLuong = db.tblLuongs.Where(x => x.MaNV == ma).OrderBy(x=>x.Thang).ToList();
            return View(listLuong);
        }
        public ActionResult ChiTietLuong(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblLuong tblLuong = db.tblLuongs.Find(id);
            if (tblLuong == null)
            {
                return HttpNotFound();
            }
            return View(tblLuong);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
