using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAN.Areas.System.Models;
using DAN.Helpers;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.Data.Entity.Core.Objects;

namespace DAN.Areas.System.Controllers
{
    [DataContract]
    public class DataPoint
    {
        public DataPoint(decimal x, decimal y)
        {
            this.X = x;
            this.Y = y;
        }

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "x")]
        public Nullable<decimal> X = null;

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "y")]
        public Nullable<decimal> Y = null;
    }

    //[Authorize(Roles = "Admin")]
    [Authorize]
    public class ManageController : Controller
    {
        DAN.Models.DANEntities db = new DAN.Models.DANEntities();


        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ThongKeTheoThang(DatetimeViewModel model)
        {
            int defaulDate = model.value;
            decimal sumPrice1 = 0;
            if (model  != null)
            {
                int numMonth1 = model.value;
                var getOrder = from s in db.Orders where s.CreatOn.Month == numMonth1
                               orderby s.CreatOn descending
                               select s.CreatOn;
                List<DataPoint> listTotal = new List<DataPoint>();
                var dateTim2 = getOrder.ToList().Select(x => x.Date).Distinct();
                List<DateTime> listDate = dateTim2.ToList();
                try
                {
                    for (int i = 0; i < listDate.Count(); i++)
                    {
                        var numDate1 = listDate[i].Day;
                        var numYear2 = listDate[i].Year;
                        var total1 = from s in db.Orders where (s.CreatOn.Month == numMonth1 && s.CreatOn.Year == numYear2) select s.TotalPrice;
                        var total2 = from s in db.Orders where (s.CreatOn.Day == numDate1 && s.CreatOn.Month == numMonth1 && s.CreatOn.Year == numYear2) select s.TotalPrice;
                        decimal sumPrice = total2.Sum();
                        sumPrice1 = total1.Sum();
                        listTotal.Add(new DataPoint(numDate1, sumPrice));

                    }
                }catch(Exception e)
                {

                }
                List<DataPoint> dataPoints = new List<DataPoint>();
                ViewBag.DataPoints = JsonConvert.SerializeObject(listTotal);

            }
            ViewBag.sumPrice1 = sumPrice1;
            ViewBag.defaulDate = defaulDate;
            return View("ThongKeDT");
        }

       
        public ActionResult ThongKeDT()
        { 
            DateTime defaulDate1 =  DateTime.Now;
            int defaulDate = Convert.ToInt32(defaulDate1.Month);
            var getOrder = from s in db.Orders  where s.CreatOn.Month == defaulDate1.Month
                           orderby s.CreatOn descending
                           select  s.CreatOn;
            decimal sumPrice1 = 0;
            List<DataPoint> listTotal = new List<DataPoint>();
            var dateTim2 = getOrder.ToList().Select(x => x.Date).Distinct();
            List<DateTime> listDate = dateTim2.ToList();
            for(int i = 0; i < listDate.Count(); i++)
            {
                var numDate = listDate[i].Day;
                var numMonth = listDate[i].Month;
                var numYear = listDate[i].Year;
                var total1 = from s in db.Orders where ( s.CreatOn.Month == numMonth && s.CreatOn.Year == numYear) select s.TotalPrice;
                var total2 = from s in db.Orders where (s.CreatOn.Day == numDate &&  s.CreatOn.Month == numMonth && s.CreatOn.Year == numYear) select s.TotalPrice;
                sumPrice1 = total1.Sum();
                decimal sumPrice2 = total2.Sum();
                listTotal.Add(new DataPoint(numDate, sumPrice2));

            }
            List<DataPoint> dataPoints = new List<DataPoint>();
            ViewBag.DataPoints = JsonConvert.SerializeObject(listTotal);
            ViewBag.defaulDate =  defaulDate;
            ViewBag.sumPrice1 = sumPrice1;
            return View("ThongKeDT");
        }

        public ActionResult User(int page = 0)
        {

            //int numPerPage = 10;
            //var model = db.Users.OrderByDescending(e => e.UId).Skip(page * numPerPage).Take(numPerPage);
            //int total = db.Users.ToList().Count / numPerPage;
            //ViewBag.total = total;
            //ViewBag.currentPage = page;sssssss
            var model = db.Users.ToList();
            return View(model);
            //return PartialView(model);
        }

        [HttpGet]
        public ActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddUser(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new DAN.Models.User() { Uname = model.Uname, Upass = MyHelpers. Md5(model.Upass)};
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("User");
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult EditUser(int itemId, string act)
        {
            switch (act)
            {
                case "edit":
                    var user = db.Users.Find(itemId);
                    if (user == null)
                        return RedirectToAction("User");
                    var model = new UserViewModel()
                    {
                        Uname = user.Uname,
                        Upass = MyHelpers.Md5(user.Upass)
                    };
                    return View(model);
                case "delete":
                    var users = db.Users.Find(itemId);
                    db.Users.Remove(users);
                    db.SaveChanges();
                    return RedirectToAction("User");
                default:
                    Response.StatusCode = 404;
                    return HttpNotFound();
            }
        }
        [HttpPost]
        public ActionResult EditUser(UserViewModel model)
        {
            var user = db.Users.Find(model.Uid);
            user.Uname = model.Uname;
            user.Upass = MyHelpers.Md5(user.Upass);
            db.SaveChanges();
            return RedirectToAction("User");
        }

        [AjaxOnly]
        public ActionResult GetOrder(int page = 0)
        {
            int numPerPage = 10;
            var model = db.Orders.OrderByDescending(e => e.Id).Skip(page * numPerPage).Take(numPerPage);
            int total = db.Orders.ToList().Count / numPerPage;
            ViewBag.total = total;
            //ViewBag.total = model.Count()/numPerPage;
            ViewBag.currentPage = page;
            return PartialView(model);
        }
        public ActionResult Category()
        {
            //int numPerPage = 10;
            //var model = new DAN.Models.CategoryViewModel();
            //int total = db.Categories.ToList().Count / numPerPage;
            //model.category = db.Categories.OrderByDescending(e => e.CId).Skip(page * numPerPage).Take(numPerPage).ToList();
            //model.total = total;
            //model.currentPage = page;
            IEnumerable<DAN.Models.Category> model = db.Categories;
            return View(model);
        }
        [HttpGet]
        public ActionResult AddCategory()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddCategory(CategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var cat = new DAN.Models.Category()
                    {
                        Cname = model.Cname,
                        Cdesc = model.Cdesc
                    };

                    if (db.Categories.FirstOrDefault(e => e.Cname == model.Cname) == null)
                    {
                        db.Categories.Add(cat);
                        db.SaveChanges();
                        return RedirectToAction("Category");
                    }
                    ModelState.AddModelError("", "Danh mục với tên: " + model.Cname + " đã tồn tại!");
                    return View();
                }catch(Exception e)
                {
                }
               
            }
            return View();
        }
        [HttpGet]
        public ActionResult EditCategory(int itemId, string act)
        {
            switch (act)
            {
                case "edit":
                    var cat = db.Categories.Find(itemId);
                    var model = new CategoryViewModel()
                    {
                        CId = cat.CId, Cname = cat.Cname,
                        Cdesc = cat.Cdesc
                    };
                    if(model == null)
                        return RedirectToAction("Category");
                    return View(model);
                case "delete":
                    try
                    {
                        var category = db.Categories.Find(itemId);
                        var products = db.Products.Where(e => e.CId == category.CId).ToList();
                        foreach (var p in products)
                        {
                            var img = db.Images.Where(e => e.PId == p.PId).ToList();
                            db.Images.RemoveRange(img);
                        }
                        db.Products.RemoveRange(products);
                        db.Categories.Remove(category);
                        db.SaveChanges();
                        return RedirectToAction("Category");
                    }
                    catch
                    {
                        return RedirectToAction("Category");
                    }
                default:
                    HttpContext.Response.StatusCode = 404;
                    return HttpNotFound();
            }
            HttpContext.Response.StatusCode = 404;
            return HttpNotFound();
        }
        [HttpPost]
        public ActionResult EditCategory(CategoryViewModel model)
        {
            try
            {
                var cat = db.Categories.Find(model.CId);
                if (cat == null)
                    return RedirectToAction("Category");
                cat.Cname = model.Cname;
                cat.Cdesc = model.Cdesc;
                db.SaveChanges();
                return RedirectToAction("Category");
            }
            catch
            {
                return RedirectToAction("Category");
            }
        }
        public ActionResult Product()
        {
            return View();
        }
        [AjaxOnly]
        public ActionResult GetProduct(int page = 0)
        {
            int numPerPage = 10;
            var model = new DAN.Models.IndexViewModel();
            int total = db.Products.ToList().Count / numPerPage;
            model.product = db.Products.OrderByDescending(e => e.PId).Skip(page * numPerPage).Take(numPerPage).ToList();
            model.total = total;
            model.currentPage = page;
            //var model = new DAN.Models.IndexViewModel();
            //int numPerPage = 10;
            //model.product = db.Products.OrderByDescending(e => e.PId).Skip(page * numPerPage).Take(numPerPage).ToList();
            //model.total = model.product.Count() / numPerPage;
            //model.currentPage = page;
            return PartialView(model);
        }
        [HttpGet]
        public ActionResult AddProduct()
        {
            IEnumerable<DAN.Models.Category> categories = db.Categories;
            var model = new ProductViewModel() { category = categories };
            return View(model);
        }
        [HttpPost]
        public ActionResult AddProduct(ProductViewModel model)
        {
            model.category = db.Categories;
            if (ModelState.IsValid)
            {
                var product = new DAN.Models.Product()
                {
                    Pname = model.Pname,
                    Pdesc = model.Pdesc,
                    Price = model.Price,
                    Sale = model.Sale,
                    SalePrice = model.Sale == 0 ? model.Price : model.Price - (model.Price*Convert.ToDecimal(model.Sale/(double)100)),
                    CId = model.CId,
                    View = 0
                };
                try
                {
                    db.Products.Add(product);
                    foreach (var image in model.image)
                    {
                        if (image != null)
                        {
                            var img = new DAN.Models.Image()
                            {
                                IData = new byte[image.ContentLength],
                                IType = image.ContentType,
                                PId = product.PId
                            };
                            image.InputStream.Read(img.IData, 0, image.ContentLength);
                            db.Images.Add(img);
                            db.SaveChanges();
                        }
                    }
                    return RedirectToAction("Product");
                }
                catch
                {
                    ModelState.AddModelError("", "Có lỗi xảy ra! Vui lòng thử lại");
                    return View(model);
                }
            }
            ModelState.AddModelError("", "Có lỗi xảy ra! Vui lòng thử lại");
            return View(model);
        }
        [HttpGet]
        public ActionResult EditProduct(int itemId, string act)
        {
            switch (act)
            {
                case "edit":
                    var product = db.Products.Find(itemId);
                    var img = db.Images.Where(e => e.PId == product.PId);
                    var model = new ProductViewModel()
                    {
                        PId = product.PId,
                        Pname = product.Pname,
                        Pdesc = product.Pdesc,
                        Price = product.Price,
                        Sale = product.Sale.Value,
                        CId = product.CId,
                        category = db.Categories,
                        img = img
                    };
                    if (model == null)
                        return RedirectToAction("Product");
                    return View(model);
                case "delete":
                    try
                    {
                        var p = db.Products.Find(itemId);
                        var images = db.Images.Where(e => e.PId == p.PId).ToList();
                        db.Images.RemoveRange(images);
                        db.Products.Remove(p);
                        db.SaveChanges();
                        return RedirectToAction("Product");
                    }
                    catch
                    {
                        return RedirectToAction("Product");
                    }
                default:
                    Response.StatusCode = 404;
                    return HttpNotFound();
            }
        }
        [HttpPost]
        public ActionResult EditProduct(ProductViewModel model)
        {
            model.img = db.Images.Where(e => e.PId == model.PId);
            model.category = db.Categories;
            if (ModelState.IsValid)
            {
                try
                {
                    var product = db.Products.Find(model.PId);
                    product.Pname = model.Pname;
                    product.Pdesc = model.Pdesc;
                    product.Price = model.Price;
                    product.Sale = model.Sale;
                    product.SalePrice = model.Sale == 0 ? model.Price : model.Price - (model.Price * Convert.ToDecimal(model.Sale / (double)100));
                    product.CId = model.CId;
                    db.SaveChanges();
                    return RedirectToAction("Product");
                }
                catch
                {
                    ModelState.AddModelError("", "Có lỗi xảy ra! Vui lòng thử lại");
                    return View(model);
                }
            }
            return View(model);
        }
        public ActionResult Sale()
        {
            //int numPerPage = 10;
            //var model = db.Orders.OrderByDescending(e => e.Id).Skip(page * numPerPage).Take(numPerPage);
            //int total = db.Orders.ToList().Count / numPerPage;
            //ViewBag.total = total;
            ////ViewBag.total = model.Count()/numPerPage;
            //ViewBag.currentPage = page;
            //return PartialView(model);


            //int numPerPage = 3;
            //var model = new DAN.Models.IndexViewModel();
            //int total = db.Sales.ToList().Count / numPerPage;
            //model.sale = db.Sales.OrderByDescending(e => e.Id).Skip(page * numPerPage).Take(numPerPage).ToList();
            //model.total = total;
            //model.currentPage = page;
            //var model = db.Sales.ToList();
            //int numperpage = 2;
            //var model = db.Sales.OrderByDescending(e => e.Id).Skip(page * numperpage).Take(numperpage);
            //int total = db.Sales.ToList().Count / numperpage;
            //ViewBag.total = total;
            //viewbag.total = model.count() / numperpage;
            //ViewBag.currentPage = page;
            //return View(model);
            var model = db.Sales.ToList();
            return View(model);
        }
        [HttpGet]
        public ActionResult EditSale(int itemId, string act)
        {
            switch (act)
            {
                case "edit":
                    var sale = db.Sales.Find(itemId);
                    if (sale == null)
                        return RedirectToAction("Sale");
                    var model = new SaleViewModel()
                    {
                        Id = sale.Id,
                        Title = sale.Title,
                        Content = sale.Content
                    };
                    return View(model);
                case "delete":
                    var sales = db.Sales.Find(itemId);
                    db.Sales.Remove(sales);
                    db.SaveChanges();
                    return RedirectToAction("Sale");
                default:
                    Response.StatusCode = 404;
                    return HttpNotFound();
            }
        }
        [HttpPost]
        public ActionResult EditSale(SaleViewModel model)
        {
            var sale = db.Sales.Find(model.Id);
            sale.Title = model.Title;
            sale.Content = model.Content;
            db.SaveChanges();
            return RedirectToAction("Sale");
        }
        [HttpGet]
        public ActionResult AddSale()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddSale(SaleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var sale = new DAN.Models.Sale() { Content = model.Content, Title = model.Title, CreatOn = DateTime.Now };
                db.Sales.Add(sale);
                db.SaveChanges();
                return RedirectToAction("Sale");
            }
            return View(model);
        }
        public ActionResult Contact(int itemId = 0)
        {
            if(itemId == 0)
            {
                var model = db.Contacts.OrderByDescending(e => e.Id);
                return View(model);
            }
            else
            {
                var model = db.Contacts.Find(itemId);
                if (model == null)
                    return RedirectToAction("Contact");
                model.Read = true;
                db.SaveChanges();
                return View("ViewContact", model);
            }
        }

        public ActionResult FileUpload(HttpPostedFileBase file)
        {
            if (file != null)
            {
                string pic = Path.GetFileName(file.FileName);
                string path = Path.Combine( Server.MapPath("~/images/"), pic);
                file.SaveAs(path);
                using (MemoryStream ms = new MemoryStream())
                {
                    file.InputStream.CopyTo(ms);
                    byte[] array = ms.GetBuffer();
                }

            }
            // after successfully uploading redirect the user
            return RedirectToAction("AddProduct");
        }
    }
}