using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyNhanSu.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        // GET: Admin/Base
        public ActionResult Index()
        {
            if (System.Web.HttpContext.Current.Session["UserName"].Equals(""))
            {
                System.Web.HttpContext.Current.Response.Redirect("~/Admin/Login");
            }
            return View();
        }
    }
}