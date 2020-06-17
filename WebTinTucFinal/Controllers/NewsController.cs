using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTinTucFinal.Models;

namespace WebTinTucFinal.Controllers
{
    public class NewsController : Controller
    {
        // GET: News
        dbQLTintucDataContext data = new dbQLTintucDataContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Chuyenmuc()
        {
            var chuyenmuc = from cm in data.CHUYENMUCs select cm;
            return PartialView(chuyenmuc);
        }

        public ActionResult TinTheoChuyenmuc( int id)
        {
            var tintuc = from t in data.BAIDANGs where t.IDBaiDang == id select t;
            return View(tintuc);
        }
    }
}