using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTinTucFinal.Models;

namespace WebTinTucFinal.Controllers
{
    public class DocBaoController : Controller
    {
        dbQLTintucDataContext data = new dbQLTintucDataContext();
        // GET: DocBao
        public ActionResult Baiviet(int id)
        {
            var ndbaiviet = from ndbv in data.BAIDANGs where ndbv.IDBaiDang == id select ndbv;
            return View(ndbaiviet);
        }
    }
}