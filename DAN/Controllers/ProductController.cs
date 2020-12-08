using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAN.Models;

namespace DAN.Controllers
{
    public class ProductController : Controller
    {
        DANEntities db = new DANEntities();
        public ActionResult Index(int itemId = 0)
        {
            if (itemId == 0)
                return RedirectToAction("Index", "Home");
            ProductViewModel model = new ProductViewModel();
            model.product = db.Products.SingleOrDefault(e => e.PId == itemId);
            if (model.product == null)
                return RedirectToAction("Index", "Home");
            model.product.View++;
            db.SaveChanges();
            model.image = db.Images.Where(e => e.PId == itemId).ToList();
            model.related = db.Products.Where(e => e.CId == model.product.CId && e.PId != model.product.PId).Take(4).OrderBy(e => Guid.NewGuid()).ToList();
            return View(model);
        }
        public FileContentResult GetProductImage(int itemId)
        {
          
            if (itemId != 0)
            {
                try
                {
                    var image = db.Images.FirstOrDefault(e => e.PId == itemId);
                    if (image != null)
                        return File(image.IData, image.IType);
                    return null;
                }
                catch(Exception e) { }
            }
            return null;
        }
        public FileContentResult GetImage(int itemId)
        {
            if (itemId != 0)
            {
                var image = db.Images.SingleOrDefault(e => e.Id == itemId);
                if (image != null)
                    return File(image.IData, image.IType);
                return null;
            }
            return null;
        }
    }
}