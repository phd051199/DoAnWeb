using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAN.Models;
namespace DAN.Controllers
{
    public class HomeController : Controller
    {
        DANEntities db = new DANEntities();
        public ActionResult Index()
        {
            return View();
        }
        [AjaxOnly]
        public ActionResult NewProduct(int page = 0)
        {
            int numPerPage = 8;
            IndexViewModel model = new IndexViewModel();
            int total = db.Products.ToList().Count / numPerPage;
            model.product = db.Products.OrderByDescending(e => e.PId).Skip(page * numPerPage).Take(numPerPage).ToList();
            model.total = total;
            model.currentPage = page;
            return PartialView(model);
        }
        [AjaxOnly]
        public ActionResult MostView(int page = 0)
        {
            int numPerPage = 8;
            IndexViewModel model = new IndexViewModel();
            int total = db.Products.ToList().Count / numPerPage;
            model.product = db.Products.OrderByDescending(e => e.View).Skip(page * numPerPage).Take(numPerPage).ToList();
            model.total = total;
            model.currentPage = page;
            return PartialView(model);
        }
        public ActionResult KhuyenMai(int itemId = 0)
        {
            if (itemId == 0)
            {
                var model = db.Sales.OrderByDescending(e => e.Id).Take(10).ToList();
                return View(model);
            }
            else
            {
                var model = db.Sales.SingleOrDefault(e => e.Id == itemId);
                if (model == null)
                    return RedirectToAction("KhuyenMai");
                return View("ChiTietKhuyenMai", model);
            }
        }
        public ActionResult VanChuyen()
        {
            return View();
        }
        [HttpGet]
        public ActionResult LienHe()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LienHe(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                var contact = new Contact()
                {
                    From = model.From,
                    Subject = model.Subject,
                    Message = model.Message,
                    Read = false
                };
                db.Contacts.Add(contact);
                db.SaveChanges();
                ModelState.AddModelError("", "Cảm ơn bạn đã góp ý. Thư của bạn đã được gửi đi, chúng tôi sẽ phản hồi lại qua email "+model.From+" !");
            }
            return View(model);
        }

        public ActionResult SearchProduct(string searchStr, int page = 0)
        {
            var product = from p in db.Products select p;
            if (!String.IsNullOrEmpty(searchStr))
            {
                product = product.Where(p => p.Pname.Contains(searchStr));
            }
            return View("SearchProduct", product.ToList());
        }
    }
}