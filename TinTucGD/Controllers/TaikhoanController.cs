using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TinTucGD.Models;

namespace TinTucGD.Controllers
{
    public class TaikhoanController : Controller
    {
        dbQLTintucDataContext data = new dbQLTintucDataContext();
        public ActionResult DangKy()
        {
            return View();
        }

        // GET: Taikhoan
        public ActionResult Index()
        {
            return View();
        }
    }
}