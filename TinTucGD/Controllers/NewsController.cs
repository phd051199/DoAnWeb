using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TinTucGD.Models;

namespace TinTucGD.Controllers
{
    public class NewsController : Controller
    {
        dbQLTintucDataContext data = new dbQLTintucDataContext();
        // GET: News
        public ActionResult Index()
        {
            return View();
        }
    }
}