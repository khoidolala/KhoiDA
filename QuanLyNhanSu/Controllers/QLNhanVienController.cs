using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QuanLyNhanSu.Models;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using PagedList;
using System.Data.Entity.Migrations.Model;
namespace QuanLyNhanSu.Controllers
{
    public class QLNhanVienController : Controller
    {
        private DBContext db = new DBContext();

        // GET: NhanVien
        public ActionResult Index(string name, int page = 1)
        {
            ViewBag.CurrentTime = DateTime.Now.ToString();
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Login", "QLNhanVien");
            }
            page = page < 1 ? 1 : page;
            int pagesize = 5;
            var tblThongTinNVs = db.tblThongTinNVs.Include(t => t.tblHSL).Include(t => t.tblPhong).OrderBy(b => b.MaNV);
            if (!string.IsNullOrEmpty(name))
            {
                tblThongTinNVs = db.tblThongTinNVs.Where(x => x.TenNV.Contains(name)).OrderBy(x => x.TenNV);
            }
            return View(tblThongTinNVs.ToPagedList(page, pagesize));
        }


        // GET: NhanVien/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblThongTinNV tblThongTinNV = db.tblThongTinNVs.Find(id);
            if (tblThongTinNV == null)
            {
                return HttpNotFound();
            }
            return View(tblThongTinNV);
        }

        // GET: NhanVien/Create
        public ActionResult Create()
        {
            ViewBag.MaHSL = new SelectList(db.tblHSLs, "MaHSL", "HSL");
            ViewBag.MaPhong = new SelectList(db.tblPhongs, "MaPhong", "TenPhong");
            ViewBag.NgaySinh = DateTime.Today.ToString("dd/MM/yyyy");
            return View();
        }

        // POST: NhanVien/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaNV,TenNV,CMT,GioiTinh,NgaySinh,DiaChi,ChucVu,TrinhDo,MaHSL,Email,SDT,MaPhong,BHXH,DanToc,TonGiao")] tblThongTinNV tblThongTinNV, HttpPostedFileBase fileImage)
        {
            if (ModelState.IsValid)
            {
                if (fileImage != null || fileImage.ContentLength > 0)
                {
                    string filename = System.IO.Path.GetFileName(fileImage.FileName);
                    string UpdateFile = Path.Combine(Server.MapPath("/Images/"), filename);
                    if (System.IO.File.Exists(UpdateFile))
                    {
                        System.IO.File.Delete(UpdateFile);
                        fileImage.SaveAs(UpdateFile);
                    }
                    else
                    {
                        fileImage.SaveAs(UpdateFile);
                    }
                    tblThongTinNV.UrlAnh = filename;
                }
                db.tblThongTinNVs.Add(tblThongTinNV);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaHSL = new SelectList(db.tblHSLs, "MaHSL", "MaHSL", tblThongTinNV.MaHSL);
            ViewBag.MaPhong = new SelectList(db.tblPhongs, "MaPhong", "TenPhong", tblThongTinNV.MaPhong);
            return View(tblThongTinNV);
        }

        // GET: NhanVien/Edit/5
        public ActionResult Edit(string id)
        {

            ViewBag.quyen = (int ) Session["Quyen"];
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblThongTinNV tblThongTinNV = db.tblThongTinNVs.Find(id);
            if (tblThongTinNV == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaHSL = new SelectList(db.tblHSLs, "MaHSL", "HSL", tblThongTinNV.MaHSL);
            ViewBag.MaPhong = new SelectList(db.tblPhongs, "MaPhong", "TenPhong", tblThongTinNV.MaPhong);
            return View(tblThongTinNV);
        }

        // POST: NhanVien/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaNV,TenNV,CMT,GioiTinh,NgaySinh,DiaChi,ChucVu,TrinhDo,MaHSL,Email,SDT,MaPhong,BHXH,DanToc,TonGiao")] tblThongTinNV tblThongTinNV, HttpPostedFileBase editImage, FormCollection f)
        {
            
                if (editImage != null || editImage.ContentLength > 0)
                {
                    string filename = System.IO.Path.GetFileName(editImage.FileName);
                    string UpdateFile = Path.Combine(Server.MapPath("/Images/"), filename);
                    if (System.IO.File.Exists(UpdateFile))
                    {
                        System.IO.File.Delete(UpdateFile);
                        editImage.SaveAs(UpdateFile);
                    }
                    else
                    {
                        editImage.SaveAs(UpdateFile);
                    }
                    tblThongTinNV.UrlAnh = filename;
                    db.Entry(tblThongTinNV).State = EntityState.Modified;
                    db.SaveChanges();
                    if (ViewBag.quyen == 2) {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return Redirect("/LoginNhanVien/ThongTinCaNhan");
                    }
                }
                else
                {
                tblThongTinNV.UrlAnh = f["UrlAnh"];
                db.Entry(tblThongTinNV).State = EntityState.Modified;
                db.SaveChanges();
            }
            
            ViewBag.MaHSL = new SelectList(db.tblHSLs, "MaHSL", "MaHSL", tblThongTinNV.MaHSL);
            ViewBag.MaPhong = new SelectList(db.tblPhongs, "MaPhong", "TenPhong", tblThongTinNV.MaPhong);
            return View(tblThongTinNV);
        }

        // GET: NhanVien/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblThongTinNV tblThongTinNV = db.tblThongTinNVs.Find(id);
            if (tblThongTinNV == null)
            {
                return HttpNotFound();
            }
            return View(tblThongTinNV);
        }

        // POST: NhanVien/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            tblThongTinNV tblThongTinNV = db.tblThongTinNVs.Find(id);
            db.tblThongTinNVs.Remove(tblThongTinNV);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult NghiPhep()
        {
            return View();
        }
        public ActionResult NghiPhep(DateTime start, DateTime end)
        {
            var id = Session["MaNV"] as string;

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
       /* public ActionResult ThongTinCaNhan()
        {

            var id = Session["MaNV"] as string;
            var ttcn = db.tblThongTinNVs.Where(x => x.MaNV == id).FirstOrDefault();
            return View(ttcn);
        }*/
        /*public ActionResult Login()
        {
            if (!Session["UserName"].Equals(""))
            {
                return RedirectToAction("Index", "QLNhanVien");
            }
            ViewBag.Error = "";

            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection field)
        {
            string strerror = "";
            string username = field["username"];
            string password = field["password"];
            EncodePass.GetMD5(password);
            tblUser rowuser = db.tblUsers.Where(x => x.UserName == username && x.Password == password).FirstOrDefault();
            if (rowuser == null)
            {
                strerror = "Sai tên đăng nhập hoặc mật khẩu";
            }
            else
            {
                if (rowuser.Quyen == 1)
                {
                    strerror = "Trang này không dành cho Admin";

                }
                if (rowuser.Quyen == 2)
                {
                    Session["MaNV"] = rowuser.MaNV;
                    Session["TenNV"] = rowuser.TenNV;
                    return RedirectToAction("Index", "QLNhanVien");

                }
                if (rowuser.Quyen == 3)
                {
                    Session["MaNV"] = rowuser.MaNV;
                    Session["TenNV"] = rowuser.TenNV;
                    return RedirectToAction("Index", "QLLuongs");
                }
            }

            ViewBag.Error = "<span class='text-danger'>" + strerror + "</span>";
            return View();
        }
        public ActionResult Logout()
        {
            Session["UserName"] = "";
            Session["IDUser"] = "";
            return RedirectToAction("Login", "QLNhanVien");
        }

        // GET: User/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        // POST: User/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = UserManager.FindById(User.Identity.GetUserId);
                if (user == null)
                {
                    return HttpNotFound();
                }

                var result = UserManager.ChangePassword(user.Id, model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }
            }
            return View(model);
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


    }

    public class ChangePasswordViewModel
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }*/
    }
}
