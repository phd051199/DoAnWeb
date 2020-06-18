using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTinTucFinal.Models;

namespace WebTinTucFinal.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        dbQLTintucDataContext db = new dbQLTintucDataContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Dangnhap()
        {
            return View();
        }

        [HttpPost]

        public ActionResult Dangnhap(FormCollection collection)
        {
            var tendn = collection["Username"];
            var matkhau = collection["Password"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                TAIKHOAN t = db.TAIKHOANs.SingleOrDefault(n => n.TenTaiKhoan == tendn && n.MatKhau == matkhau);
                if (t != null)
                {
                    Session["A"] = t;
                    return RedirectToAction("Index", "Admin");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }

    }
}