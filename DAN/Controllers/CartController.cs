using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAN.Models;

namespace DAN.Controllers
{
    public class CartController : Controller
    {

        CartViewModel model = new CartViewModel() { product = new List<Product>() };
        DANEntities db = new DANEntities();
        // GET: Cart
        public ActionResult Index()
        {
            if (Session["Cart"] != null)
                model.product = (List<Product>)Session["Cart"];
            return View(model);
            // trả về view giỏ hàng
        }
        public ActionResult ShowCart()
        {
            if (Session["Cart"] != null)
                model.product = (List<Product>)Session["Cart"];
            return PartialView(model);
            ///nhúng vào view giỏ hàng
        }
        public ActionResult AddCart(int itemId, int soLuong = 1)
        {
            if (Session["Cart"] == null)
            {
                var monMoi = db.Products.SingleOrDefault(e => e.PId == itemId);
                monMoi.Quantum = soLuong;
                model.product.Add(monMoi);
                Session["Cart"] = model.product;
                return JavaScript("$('#shopping-cart .counter').html("+model.product.Count+");");
            }
            else
            {
                List<Product> newM = (List<Product>)Session["Cart"];
                var monMoi = db.Products.SingleOrDefault(e => e.PId == itemId);
                monMoi.Quantum  = soLuong;
                var monCu = newM.SingleOrDefault(e => e.PId == itemId);
                if (monCu != null)
                {
                   // monCu.Quantum += soLuong;
                }
                    
                else
                    newM.Add(monMoi);
                Session["Cart"] = newM;
                return JavaScript("$('#shopping-cart .counter').html(" + newM.Count + ");");
            }
            return JavaScript("");
        }
        public ActionResult RemoveCart(int itemId, string returnUrl = null)
        {
            if (Session["Cart"] == null)
            {
                if (returnUrl != null)
                    return Redirect(returnUrl);
                return JavaScript("");
            }
            else
            {
                List<Product> newM = (List<Product>)Session["Cart"];
                var monCu = newM.SingleOrDefault(e => e.PId == itemId);
                newM.Remove(monCu);
                Session["Cart"] = newM;
                if (returnUrl != null)
                    return Redirect(returnUrl);
                return JavaScript("$('#shopping-cart .counter').html(" + newM.Count + ");");
            }
            if(returnUrl != null)
                return Redirect(returnUrl);
            return JavaScript("");
        }
    }
}