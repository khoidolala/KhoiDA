using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyNhanSu.Models;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Web.Security;
using QuanLyNhanSu.Utils;
using System.Web.Services.Description;

namespace QuanLyNhanSu.Controllers
{
    public class LoginNhanVienController : Controller
    {
        DBContext db = new DBContext();
        // GET: LoginNhanVien
        public ActionResult Login()
        {
            if (Request.Cookies["LoginCookie"] != null && !HttpContext.User.Identity.IsAuthenticated)
            {
                var username = Request.Cookies["LoginCookie"]["username"];
                var password = Request.Cookies["LoginCookie"]["password"];

                // Kiểm tra đăng nhập
                if (username != null && password != null)
                {
                    // Đăng nhập thành công
                    FormsAuthentication.SetAuthCookie(username, true);
                }
                else
                {
                    ViewBag.Error = "";
                }
            }
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection field, bool? rememberMe)
        {
            string strerror = "";
            string username = field["username"];
            string password = field["password"];
            /*string ps = EncodePass.GetMD5(password);*/
            tblUser rowuser = db.tblUsers.Where(x => x.UserName == username && x.Password == password).FirstOrDefault();
            if (rowuser == null)
            {
                strerror = "Sai tên đăng nhập hoặc mật khẩu";
            }
            else
            {
                if (rememberMe.HasValue && rememberMe.Value)
                {
                    HttpCookie cookie = new HttpCookie("LoginCookie");
                    cookie.Values.Add("username", username);
                    cookie.Values.Add("password", password);
                    cookie.Expires = DateTime.Now.AddDays(7);
                    Response.Cookies.Add(cookie);
                }
                if (rowuser.Quyen == 1)
                {
                    strerror = "Trang này không dành cho Admin, vui lòng đăng nhập lại!";

                }
                if (rowuser.Quyen == 2)
                {
                    Session["MaNV"] = rowuser.MaNV;
                    Session["TenNV"] = rowuser.TenNV;
                    Session["Quyen"] = rowuser.Quyen;
                    return RedirectToAction("Index", "QLNhanVien");

                }
                else
                {
                    Session["MaNV"] = rowuser.MaNV;
                    Session["TenNV"] = rowuser.TenNV;
                    Session["Quyen"] = rowuser.Quyen;
                    return RedirectToAction("ThongTinCaNhan", "LoginNhanVien");
                }
            }

            ViewBag.Error = "<span class='text-danger'>" + strerror + "</span>";

            return View();
        }
        public ActionResult Logout()
        {
            Session["UserName"] = "";
            Session["IDUser"] = "";
            return RedirectToAction("Login", "LoginNhanVien");
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(string oldPassword, string newPassword, string comfirmPassword)
        {
            var id = Session["MaNV"] as string;
            var user = db.tblUsers.Where(x => x.MaNV == id).FirstOrDefault();
            Session["Password"] = user.Password;
            if (user == null)
            {
                return HttpNotFound();
            }
            if (user.Password != oldPassword)
            {
                ViewBag.Error = "Mật khẩu không khớp";
                return View();
            }
            if (newPassword != comfirmPassword)
            {
                ViewBag.Error1 = "Mật khẩu không khớp";
                return View();
            }
            user.Password = newPassword;
            db.SaveChanges();
            return RedirectToAction("Login", "LoginNhanVien");
        }
        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgotPassword(string email)
        {
            var nv = db.tblThongTinNVs.SingleOrDefault(x => x.Email == email);
            if (nv != null)
            {
                Random rnd = new Random();
                int code = rnd.Next(1000, 9999);

                EmailSender.Send("Mã xác minh", email, "khuongip564gb@gmail.com", "cjwbneedakkwoxnb", "Code mã minh: " + code);
                Session["Email"] = nv.Email;
                Session["code"] = code;
                return Redirect("/LoginNhanVien/ConfirmCode");
            }
            ViewBag.Error = "Email không tồn tại";
            return View();
        }
        public ActionResult ConfirmCode()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ConfirmCode(string code, string password)
        {
            if(code == Session["code"].ToString())
            {
                var email = Session["Email"] as string;
                var nv = db.tblThongTinNVs.SingleOrDefault(x => x.Email == email);
                var user = db.tblUsers.SingleOrDefault(x => x.MaNV == nv.MaNV);
                user.Password = password;
                db.SaveChanges();
                return Redirect("/LoginNhanVien/Login");
            }
            ViewBag.Error = "Mã xác minh không đúng";
            return View();
        }
        public ActionResult ThongTinCaNhan()
        {
            var id = Session["MaNV"] as string;
            var ttcn = db.tblThongTinNVs.Where(x => x.MaNV == id).FirstOrDefault();
            return View(ttcn);
        }
    }
}