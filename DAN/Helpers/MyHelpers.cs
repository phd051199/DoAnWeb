using System.Web;
using System.Web.Mvc;

namespace DAN
{
    public static class MyHelpers
    {
        public static MvcHtmlString Paging(this HtmlHelper helper, int currentPage, int total, string ajaxFunction)
        {
            string html = "<div class=\"paging\">";
            for(int i = 0; i < total; i++)
            {
                if(i == currentPage)
                    html += "<span class=\"page active\">" + (i+1) + "</span>";
                else
                    html += "<span class=\"page\"><a onclick=\""+ajaxFunction+"("+i+");\">" + (i+1) + "</a></span>";
            }
            html += "</div>";
            MvcHtmlString str = new MvcHtmlString(html);
            return str;
        }
        public static MvcHtmlString Paging(this HtmlHelper helper, int currentPage, int total)
        {
            string html = "<div class=\"paging\">";
            for (int i = 0; i < total; i++)
            {
                if (i == currentPage)
                    html += "<span class=\"page active\">" + (i + 1) + "</span>";
                else
                    html += "<span class=\"page\"><a href=\"?&page="+i+">" + (i + 1) + "</a></span>";
            }
            html += "</div>";
            MvcHtmlString str = new MvcHtmlString(html);
            return str;
        }
        public static string Md5(this HtmlHelper helper, string str)
        {
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(str);
            byte[] hash = md5.ComputeHash(data);
            return System.BitConverter.ToString(hash).Replace("-", "").ToLower();
           
           
        }
        public static string Md5(string str)
        {
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(str);
            byte[] hash = md5.ComputeHash(data);
            return System.BitConverter.ToString(hash).Replace("-", "").ToLower();
           
        }
    }
}