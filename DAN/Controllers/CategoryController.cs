using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAN.Models;

namespace DAN.Controllers
{
    public class CategoryController : Controller
    {
        DANEntities db = new DANEntities();
        public ActionResult Index(int itemId = 1, int page = 0)
        {
            int numPerPage = 12;
            CategoryViewModel model = new CategoryViewModel();
            model.category = db.Categories.Where(e => e.CId == itemId).SingleOrDefault();
            int total = db.Products.Where(e => e.CId == itemId).ToList().Count / numPerPage;
            model.total = total;
            model.currentPage = page;
            model.product = db.Products.Where(e => e.CId == itemId).OrderByDescending(e => e.PId).Skip(page*numPerPage).Take(numPerPage).ToList();
            return View(model);
        }
    }
}