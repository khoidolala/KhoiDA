using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using QuanLyNhanSu.Models;

namespace QuanLyNhanSu
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Session_Start()
        {
            Session["UserName"] = "";
            Session["IDUser"] = "";
            Session["TenNV"] = "";
            Session["MaNV"] = "";
            Session["Password"] = "";
            Session["Email"] = "";
            Session["ConfirmationCode"] = "";
            Session["Quyen"] = "";
        }
    }
}

