using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace QuanLyNhanSu.helpp
{
    public static class HtmlExtentions
    {
        public static MvcHtmlString Image(this HtmlHelper html, byte[] AnhDaiDien, string className, object htmlAttributes = null)
        {
            var builder = new TagBuilder("img");
            builder.MergeAttribute("class", className);
            builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));

            var imageString = AnhDaiDien != null ? Convert.ToBase64String(AnhDaiDien) : "";
            var img = string.Format("data:image/jpg;base64,{0}", imageString);
            builder.MergeAttribute("src", img);

            return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));
        }
    }
}