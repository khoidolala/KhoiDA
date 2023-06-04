using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using QuanLyNhanSu.Models;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace QuanLyNhanSu.Controllers
{
    public class ForgotPasswordController : Controller
    {
        DBContext db = new DBContext();
        // GET: ForgotPassword
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(ForgotPasswordViewModel model)
        {
            
            if (ModelState.IsValid)
            {
                // Tìm tài khoản dựa trên Email
                var user = db.tblUsers
                .Join(db.tblThongTinNVs, u => u.MaNV, nv => nv.MaNV, (u, nv) => new { u, nv })
                .Where(x => x.nv.Email == model.Email)
                .Select(x => x.nv)
                .FirstOrDefault();
                if (user == null)
                {
                    ModelState.AddModelError("", "Email không tồn tại trong hệ thống.");
                    return View(model);
                }

                // Tạo mã xác nhận
                var confirmationCode = GenerateConfirmationCode();

                // Gửi mã xác nhận qua email
                await SendConfirmationCode(model.Email, confirmationCode);
                
                // Lưu thông tin vào Session
                Session["Email"] = model.Email;
                Session["ConfirmationCode"] = confirmationCode;
                return RedirectToAction("Confirm", new { email = user.Email });
            }
            return View(model);
        }

        // GET: Confirm
        public ActionResult Confirm()
        {
            return View();
        }

        // POST: Confirm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Confirm(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra mã xác nhận
                if (Session["ConfirmationCode"] == null ||
                    Session["ConfirmationCode"].ToString() != model.ConfirmationCode)
                {
                    ModelState.AddModelError("", "Mã xác nhận không đúng.");
                    return View();
                }

                // Cập nhật mật khẩu mới
                var email = Session["Email"].ToString();
                var user = (from u in db.tblUsers
                join nv in db.tblThongTinNVs on u.MaNV equals nv.MaNV where nv.Email == email
                select new {u,nv})
                .FirstOrDefault();
                if (user == null)
                {
                    ModelState.AddModelError("", "Email không tồn tại trong hệ thống.");
                    return View();
                }

                user.u.Password = model.NewPassword; // Sử dụng dạng băm nếu có
                db.SaveChanges();

                TempData["Message"] = "Mật khẩu mới đã được cập nhật.";
                return RedirectToAction("Index", "Login");
            }

            return View(model);
        }
        private string GenerateConfirmationCode()
        {
            // Tạo mã xác nhận ngẫu nhiên, ví dụ 6 chữ số
            return new Random().Next(100000, 999999).ToString();
        }

        private async Task<bool> SendConfirmationCode(string email, string confirmationCode)
        {
            try
            {
                // Thiết lập thông tin email
                var fromAddress = new MailAddress("khoivsvt@gmail.com", "Your Name");
                var toAddress = new MailAddress(email, "");
                const string fromPassword = "zgkxhvijdwbueafa";
                const string subject = "Mã xác nhận để khôi phục mật khẩu";
                string body = $"Mã xác nhận của bạn là: <b>{confirmationCode}</b>";

                // Thiết lập cấu hình server email
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                    Timeout = 20000
                };

                // Gửi email
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                })
                {
                    await smtp.SendMailAsync(message);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}