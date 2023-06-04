using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using QuanLyNhanSu.Models;

namespace QuanLyNhanSu.Areas.Admin.Controllers
{
    public class AuthController : Controller
    {
        DBContext db = new DBContext();

        public string MyString { get; private set; }

        // GET: Admin/Auth
        public ActionResult Login()
        {
            if (!Session["UserName"].Equals(""))
            {
                return RedirectToAction("Index", "QLTaiKhoan");
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
            string ps = EncodePass.GetMD5(password);
            tblUser rowuser = db.tblUsers.Where(x => x.UserName.Contains(username) && x.Password == password).FirstOrDefault();
            if (rowuser == null)
            {
                strerror = "Sai tên đăng nhập hoặc mật khẩu";
            }
            else
            {
                
                if (rowuser.Quyen != 1)
                {
                    strerror = "Đây là trang của Admin hệ thống, vui lòng đăng nhập lại!";
                }
                else {
                    Session["TenNV"] = rowuser.TenNV;
                    return RedirectToAction("Index", "QLTaiKhoan");
                }
                    
            }

            ViewBag.Error = "<span class='text-danger'>" + strerror + "</span>";
            return View();
        }
        public ActionResult Logout()
        {
            Session["UserName"] = "";
            Session["IDUser"] = "";
            return RedirectToAction("Login", "Auth");
        }
      /*  public static string GetMD5(string str)
        {

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            byte[] bHash = md5.ComputeHash(Encoding.UTF8.GetBytes(str));

            StringBuilder sbHash = new StringBuilder();

            foreach (byte b in bHash)
            {

                sbHash.Append(String.Format("{0:x2}", b));

            }

            return sbHash.ToString();

        }
*/
    }
}