using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAN.Models;
using System.Data.Entity.Validation;
using System.Data.Entity;
using System.Configuration;

namespace DAN.Controllers
{
    public class OrderController : Controller
    {
        DANEntities db = new DANEntities();
        [HttpGet]
        public ActionResult Index()
        {
            var model = new OrderViewModel();
            if (Session["Cart"] == null)
                return RedirectToAction("Index", "Cart");
            if (Session["User"] != null)
            {
                model = (OrderViewModel)Session["User"];
                return View(model);
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Confirm(OrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Code != null)
                {
                    var code = db.Codes.SingleOrDefault(e => e.Code1 == model.Code);
                    if (code == null || code.Expired.Value)
                    {
                        ModelState.AddModelError("Code", "Mã khuyến mãi không hợp lệ hoặc đã được sử dụng!");
                        return View("Index");
                    }
                }
                Session["User"] = model;
                return View(model);
            }

            return View("Index");
        }
        private void setDataVnPay(long OId,string PaidInfo,decimal Amount)
        {
            DateTime dateTime = DateTime.Now;
            Int32 now =(Int32)DateTime.UtcNow.Subtract(new DateTime(dateTime.Year, dateTime.Month,dateTime.Day)).TotalSeconds;
            string vnp_Returnurl = ConfigurationManager.AppSettings["vnp_Returnurl"]; //URL nhan ket qua tra ve 
            string vnp_Url = ConfigurationManager.AppSettings["vnp_Url"]; //URL thanh toan cua VNPAY 
            string vnp_TmnCode = ConfigurationManager.AppSettings["vnp_TmnCode"]; //Ma website
            string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Chuoi bi mat

            VnPayLibrary vnpay = new VnPayLibrary();

            vnpay.AddRequestData("vnp_Version", "2.0.0");
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);

            string locale = "vn";
            if (!string.IsNullOrEmpty(locale))
            {
                vnpay.AddRequestData("vnp_Locale", locale);
            }
            else
            {
                vnpay.AddRequestData("vnp_Locale", "vn");
            }

            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_TxnRef", now + OId.ToString());
            vnpay.AddRequestData("vnp_OrderInfo", PaidInfo);
            vnpay.AddRequestData("vnp_OrderType", "100001"); //default value: other
            vnpay.AddRequestData("vnp_Amount", (Convert.ToInt64(Amount)* 100).ToString());
            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());
            vnpay.AddRequestData("vnp_CreateDate", dateTime.ToString("yyyyMMddHHmmss"));

            string paymenturl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            Response.Redirect(paymenturl);

        }

        //public ActionResult ReturnPayMent()
        //{
        //    if (checkpayment() == true)
        //    {
        //        return RedirectToAction("ReturnVNPay");
        //        //return View("KetQua");
        //    }
        //    return RedirectToAction("Index");

        //}
        VnPayLibrary vnpay = new VnPayLibrary();

      
        public ActionResult ReturnPayMent()
        {
            string error = "Thanh toán không thành công!";
            if (Request.QueryString.Count > 0)
            {
                string dataAsString = Request.Cookies["StoreCookies"].Value;
                List<string> data = new List<string>();
                data.AddRange(dataAsString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
                string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Chuoi bi mat
                var vnpayData = Request.QueryString;

                //if (vnpayData.Count > 0)
                //{
                foreach (string s in vnpayData)
                {
                    //get all querystring data
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        vnpay.AddResponseData(s, vnpayData[s]);
                    }
                }
                // }

                //vnp_TxnRef: Ma don hang merchant gui VNPAY tai command=pay    
                long orderId = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
                //vnp_TransactionNo: Ma GD tai he thong VNPAY
                long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
                //vnp_ResponseCode:Response code from VNPAY: 00: Thanh cong, Khac 00: Xem tai lieu
                string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
                //vnp_SecureHash: MD5 cua du lieu tra ve
                String vnp_SecureHash = Request.QueryString["vnp_SecureHash"];
                bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
                if (checkSignature)
                {
                    var user = (OrderViewModel)Session["User"];
                    var cart = (List<Product>)Session["Cart"];
                    if (vnp_ResponseCode == "00")
                    {
                        var order = new Order()
                        {
                            Prefix = "BTA-",
                            Fullname = data[0],
                            Address = data[1],
                            Phone = data[2],
                            Email = data[3],
                            Code = data[4],
                            Status = true,
                            Paid = true,
                            CreatOn = DateTime.Now,
                            TotalPrice = Convert.ToDecimal(data[5]),
                            PaidInfo = data[6]
                        };

                        db.Orders.Add(order);
                        db.SaveChanges();
                        foreach (var item in cart)
                        {
                            db.OrderDetails.Add(new OrderDetail()
                            {
                                Pname = item.Pname,
                                OId = order.Id,
                                Quantum = item.Quantum,
                                Price = item.Quantum * item.SalePrice
                            });

                            db.SaveChanges();
                        }

                        long OId = db.Orders.Max(x => x.Id);

                        var result = db.Orders.First(x => x.Id == OId);

                        Session.Remove("Cart");
                        return View("ReturnVNPay", result);
                    }
                    else
                    {
                        return View("ReturnVNPay");
                    }
                }
                else
                {
                    return View("ReturnVNPay");
                }
            }
            else
            {
                return View("ReturnVNPay");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Done()
        {
            var user = (OrderViewModel)Session["User"];
            var cart = (List<Product>)Session["Cart"];
            try
            {
                var order = new Order()
                {
                    Prefix = "BTA-",
                    Fullname = user.fullName,
                    Address = user.adress,
                    Phone = user.phone.ToString(),
                    Email = user.email,
                    Code = user.Code ?? "Không sử dụng",
                    CreatOn = DateTime.Now,
                    TotalPrice = cart.Sum(e => e.Quantum * e.SalePrice),
                    PaidInfo = user.PaidInfo
                };
                HttpCookie httpCookie = new HttpCookie("StoreCookies");
                DateTime now = DateTime.Now;

                // Set the cookie value.
                List<String> listData = new List<string>();
                listData.Add(user.fullName);
                listData.Add(user.adress);
                listData.Add(user.phone.ToString());
                listData.Add(user.email);
                listData.Add(user.Code ?? "Không sử dụng");
                listData.Add(cart.Sum(e => e.Quantum * e.SalePrice).ToString());
                listData.Add(user.PaidInfo);
                string dataAsString = listData.Aggregate((a, b) => a = a + "," + b);
                httpCookie.Value = dataAsString;
                // Set the cookie expiration date.
                httpCookie.Expires = now.AddSeconds(180); // For a cookie to effectively never expire

                // Add the cookie.
                Response.Cookies.Add(httpCookie);
                setDataVnPay(order.Id, user.PaidInfo, cart.Sum(e => e.Quantum * e.SalePrice));
                //bool checkSignature = vnpay.ValidateSignature(secureHash, hashSecret);
                //db.Orders.Add(order);
                //db.SaveChanges();
                //foreach (var item in cart)
                //{
                //    db.OrderDetails.Add(new OrderDetail()
                //    {
                //        Pname = item.Pname,
                //        OId = order.Id,
                //        Quantum = item.Quantum,
                //        Price = item.Quantum * item.SalePrice
                //});
                    
                //    db.SaveChanges();
                //}
                //Session.Remove("Cart");
                return View("Done");
            }
            catch
            {
                ModelState.AddModelError("", "Có lỗi xảy ra, Vui lòng thử lại!");
                return View("Index");
            }
        }
        public ActionResult SearchInvoice()
        {
            return View("SearchInvoice");
        }
        [HttpPost]
        public ActionResult SearchInvoice(SearchInvoiceModel model)
        {
            if (ModelState.IsValid)
            {
                var order = db.Orders.SingleOrDefault(e => e.OId == model.OId);
                if (order == null)
                {
                    ModelState.AddModelError("", "Không tìm thấy đơn hàng nào!");
                    return View(model);
                }
                else
                    return RedirectToAction("Invoice", new { OId = model.OId });
            }
            return View(model);
        }
  
        public ActionResult Invoice(string OId)
        {
            var order = db.Orders.FirstOrDefault(e => e.OId == OId);
            var orderDetail = db.OrderDetails.Where(e => e.OId == order.Id);
            if (order == null)
                return RedirectToAction("SearchInvoice");
           
            var model = new InvoiceViewModel()
            {
                order = order,
                orderDetail = orderDetail
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult Invoice(string key, string OId, string returnUrl)
        {
            switch (key)
            {
                case "status":
                    var order = db.Orders.SingleOrDefault(e => e.OId == OId);
                    order.Status = true;
                    db.SaveChanges();
                    break;
                case "paid":
                    var orders = db.Orders.SingleOrDefault(e => e.OId == OId);
                    orders.Paid = true;
                    db.SaveChanges();
                    break;
            }
            return Redirect(returnUrl);
        }

        //private string HashSecret { get => _hashSecret; set => _hashSecret = value; }
        //private string SecureHash { get => secureHash; set => secureHash = value; }

    }
}