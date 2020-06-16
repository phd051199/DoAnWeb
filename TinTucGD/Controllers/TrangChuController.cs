using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TinTucGD.Models;

namespace TinTucGD.Controllers
{
    public class TrangChuController : Controller
    {
        dbQLTintucDataContext data = new dbQLTintucDataContext();
        // GET: TrangChu
        int pageSize = 4;
        public ActionResult TrangChu()
        {
            data = new dbQLTintucDataContext();
            ViewBag.pageCount = Math.Ceiling(1.0 * data.BAIVIETs.ToList().Count / pageSize);
            return View();
        }
    }
}