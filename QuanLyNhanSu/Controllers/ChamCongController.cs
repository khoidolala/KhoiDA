using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ExcelDataReader;
using iTextSharp.text;
using OfficeOpenXml;
using PagedList;
using QuanLyNhanSu.Models;
namespace QuanLyNhanSu.Controllers
{
    public class ChamCongController : Controller
    {
        private DBContext db = new DBContext();
        public DbSet<tblChamCongs> tblChamCongs { get; set; }

        // GET: ChamCong
        public ActionResult Index(string name, int page = 1)
        {
            page = page < 1 ? 1 : page;
            int pagesize = 10;
            var tblChamCong = db.tblChamCongs.OrderByDescending(x => x.Ngay).ThenBy(x => x.TenNV);
            return View(tblChamCong.ToPagedList(page, pagesize));
        }
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase excelFile)
        {
            if (excelFile != null && excelFile.ContentLength > 0 &&
        (Path.GetExtension(excelFile.FileName) == ".xls" || Path.GetExtension(excelFile.FileName) == ".xlsx"))
            {
                try
                {
                    // Create a stream for the Excel file
                    Stream stream = excelFile.InputStream;
                    IExcelDataReader reader = null;

                    // Read the Excel file with either OpenXML or ClosedXML
                    if (Path.GetExtension(excelFile.FileName) == ".xls")
                    {
                        reader = ExcelReaderFactory.CreateBinaryReader(stream);
                    }
                    else if (Path.GetExtension(excelFile.FileName) == ".xlsx")
                    {
                        reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    }

                    // Read the data from the Excel file
                    DataSet result = reader.AsDataSet();

                    // Loop through each sheet in the Excel file and read the data
                    
                   
                    foreach (DataTable table in result.Tables)
                    {
                        // Loop through each row in the sheet
                        for (int i = 1; i < table.Rows.Count; i++)
                        {
                            // Create a new TimeLog object and set its properties
                            var cc = new tblChamCongs();                            
                            cc.MaNV = table?.Rows[i][0].ToString();
                            cc.TenNV = table?.Rows[i][1].ToString();
                           
                            string dateString = table?.Rows[i][2].ToString();
                            if (DateTime.TryParse(dateString, out DateTime dateValue))
                            {
                                cc.Ngay = dateValue;
                                
                            }
                            else
                            {
                                ViewBag.Message = "Ngày không đúng định dạng";
                            }
                            string timeString = table?.Rows[i][3].ToString();
                            timeString = timeString.Replace("SA", "AM").Replace("CH", "PM");
                            TimeSpan timeSpan = DateTime.ParseExact(timeString, "dd/MM/yyyy h:mm:ss tt", CultureInfo.InvariantCulture).TimeOfDay;
                            cc.ThoiGianVao = timeSpan;
                            string timeString1 = table?.Rows[i][4].ToString();
                            timeString1 = timeString1.Replace("SA", "AM").Replace("CH", "PM");
                            TimeSpan timeSpan1 = DateTime.ParseExact(timeString1, "dd/MM/yyyy h:mm:ss tt", CultureInfo.InvariantCulture).TimeOfDay;
                            cc.ThoiGianRa = timeSpan1;

                            cc.TrangThai = table?.Rows[i][5].ToString();
                            //kiem tra trung lap thong tin
                            var chamcongs = db.tblChamCongs.Where(a => a.MaNV == cc.MaNV && a.Ngay == cc.Ngay);
                            if (chamcongs.Count() > 0)
                            {
                                db.SaveChanges();
                            }
                            else {
                                db.tblChamCongs.Add(cc);
                                db.SaveChanges();
                            }
                            
                        }

                    }
                    



                    // Close the Excel reader and stream
                    reader.Close();
                    stream.Close();

                    // Return success message
                    ViewBag.Message = "Tải lên thành công";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    
                    // Return error message
                    ViewBag.Message = "Lỗi: " + ex.Message;
                    return View();
                }
            }
            else
            {
                // Return error message if file is not Excel file
                ViewBag.Message = "Hãy chọn tệp Excel";
                return View();
            }
        }
        // GET: ChamCong/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblChamCongs tblChamCong = db.tblChamCongs.Find(id);
            if (tblChamCong == null)
            {
                return HttpNotFound();
            }
            return View(tblChamCong);
        }

        // GET: ChamCong/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ChamCong/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,MaNV,TenNV,Ngay,ThoiGianVao,ThoiGianRa,TrangThai")] tblChamCongs tblChamCong)
        {

            if (ModelState.IsValid)
            {

                db.tblChamCongs.Add(tblChamCong);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblChamCong);
        }

        // GET: ChamCong/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblChamCongs tblChamCong = db.tblChamCongs.Find(id);
            if (tblChamCong == null)
            {
                return HttpNotFound();
            }
            return View(tblChamCong);
        }

        // POST: ChamCong/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,MaNV,TenNV,Ngay,ThoiGianVao,ThoiGianRa,TrangThai")] tblChamCongs tblChamCong)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblChamCong).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblChamCong);
        }

        // GET: ChamCong/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblChamCongs tblChamCong = db.tblChamCongs.Find(id);
            if (tblChamCong == null)
            {
                return HttpNotFound();
            }
            return View(tblChamCong);
        }

        // POST: ChamCong/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblChamCongs tblChamCong = db.tblChamCongs.Find(id);
            db.tblChamCongs.Remove(tblChamCong);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
       
        public ActionResult ChamCongNhanVien()
        {
            
            string ma = (string)Session["MaNV"];
            var ttcc = db.tblChamCongs.Where(x => x.MaNV == ma).OrderByDescending(x => x.Ngay);
            return View(ttcc);
        }
        public ActionResult NhanVienChamCong()
        {
            /*bool isSunday = DateTime.Today.DayOfWeek == DayOfWeek.Sunday;

            if (isSunday)
            {
                ViewBag.Message = "Hôm nay là chủ nhật, không thể chấm công";
                return View();
            }
            if (DateTime.Now.Hour >= 20 || DateTime.Now.Hour < 8)
            {
                ViewBag.Message = "Không phải giờ hành chính của công ty, không thể chấm công";
                return View();
            }*/
            return View();
        }
        [HttpPost]
        public ActionResult NhanVienChamCong(tblChamCongs cc)
        {
            string ma = (string)Session["MaNV"];
                cc.MaNV = ma;
                if (ModelState.IsValid)
                {
                    cc.MaNV = ma;
                    cc.TenNV = db.tblThongTinNVs.Find(ma).TenNV;
                    cc.Ngay = DateTime.Now;
                    string timeString = Request.Form["timeBox"];
                    cc.ThoiGianVao = DateTime.ParseExact(timeString, "h:mm:ss tt", CultureInfo.InvariantCulture).TimeOfDay;
                    cc.ThoiGianRa = DateTime.ParseExact(Request.Form["timeBox1"], "h:mm:ss tt", CultureInfo.InvariantCulture).TimeOfDay;
                    cc.TrangThai = TinhTrang(cc.ThoiGianVao, cc.ThoiGianRa);
                    // Lưu thông tin chấm công vào cơ sở dữ liệu
                    db.tblChamCongs.Add(cc);
                    db.SaveChanges();

                    // Hiển thị thông tin chấm công của nhân viên đó
                    return View(cc);
                
            }
            // Trả về trang hiện tại nếu dữ liệu không hợp lệ
            return View();
        }
   

        private string TinhTrang(TimeSpan? gioVao, TimeSpan? gioRa)
        {
            if (!gioVao.HasValue || !gioRa.HasValue||(gioRa-gioVao)<=new TimeSpan(6,0,0))
            {
                return "Nghỉ";
            }

            if (gioVao > new TimeSpan(8, 15, 0))
            {
                return "Muộn";
            }
            
            return "đúng giờ";
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public TimeSpan ConvertStringToTimeSpan(string timeString)
        {
            DateTime baseTime = DateTime.ParseExact(timeString, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime dateTimeValue;
            if (DateTime.TryParseExact(timeString, "h:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTimeValue))
            {
                return dateTimeValue - baseTime;
            }
            else
            {
                throw new ArgumentException("Invalid time string format", nameof(timeString));
            }
        }
        

    }
}
