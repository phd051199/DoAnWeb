using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebTinTucFinal.Models;
using PagedList;
using PagedList.Mvc;
using System.Security.Cryptography;

namespace WebTinTucFinal.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        dbQLTintucDataContext db = new dbQLTintucDataContext();
        public ActionResult Index()
        {
            return View();
        }

        //hien thi chi tiet
        public ActionResult Chitietbai(int id)
        {
            BAIDANG baidang = db.BAIDANGs.SingleOrDefault(n => n.IDBaiDang == id);
            ViewBag.IDBaiDang = baidang.IDBaiDang;
            if(baidang==null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(baidang);
        }
        //xoa bai viet
        [HttpGet]
        public ActionResult Xoa(int id)
        {
            BAIDANG baidang = db.BAIDANGs.SingleOrDefault(n => n.IDBaiDang == id);
            ViewBag.IDBaiDang = baidang.IDBaiDang;
            if (baidang == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(baidang);
        }
        [HttpPost, ActionName("Xoa")]
        public ActionResult Xacnhanxoa(int id)
        {
            BAIDANG baidang = db.BAIDANGs.SingleOrDefault(n => n.IDBaiDang == id);
            ViewBag.IDBaiDang = baidang.IDBaiDang;
            if (baidang==null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.BAIDANGs.DeleteOnSubmit(baidang);
            db.SubmitChanges();
            return RedirectToAction("BaiViet");
        }
        //
        public ActionResult BaiViet(int ?page)
        {
            int pageSize = 5;
            int pageNum = (page ?? 1);
            return View(db.BAIDANGs.ToList().OrderBy(n=>n.IDBaiDang).ToPagedList(pageNum, pageSize));
        }

        [HttpGet]
        public ActionResult VietBai()
        {
            ViewBag.IDTheLoai = new SelectList(db.THELOAIs.ToList().OrderBy(n => n.TenTheLoai), "IDTheLoai", "TenTheLoai");
            ViewBag.IDTaiKhoan = new SelectList(db.TAIKHOANs.ToList().OrderBy(n => n.TenTaiKhoan), "IDTaiKhoan", "TenTaiKhoan");
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult VietBai(BAIDANG baidang, HttpPostedFileBase fileupload)
        {
            ViewBag.IDTheLoai = new SelectList(db.THELOAIs.ToList().OrderBy(n => n.TenTheLoai), "IDTheLoai", "TenTheLoai");
            ViewBag.IDTaiKhoan = new SelectList(db.TAIKHOANs.ToList().OrderBy(n => n.TenTaiKhoan), "IDTaiKhoan", "TenTaiKhoan");
            if (fileupload == null)
            {
                ViewBag.ThongBao = "Vui lòng chọn ảnh bìa !";
            }
            else 
            {
                if (ModelState.IsValid)
                {
                    var filename = Path.GetFileName(fileupload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/images"), filename);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.ThongBao = "Hình ảnh đã tồn tại";

                    }
                    else
                    {
                        fileupload.SaveAs(path);
                    }
                    baidang.AnhDaiDien = filename;
                    db.BAIDANGs.InsertOnSubmit(baidang);
                    db.SubmitChanges();
                }
                return RedirectToAction("BaiViet");
            }
            return View();
           
        }


        [HttpGet]
        public ActionResult Dangnhap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Dangnhap(FormCollection collection)
        {
            var tendn = collection["Username"];
            var matkhau = collection["Password"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                TAIKHOAN t = db.TAIKHOANs.SingleOrDefault(n => n.TenTaiKhoan == tendn && n.MatKhau == matkhau);
                if (t != null)
                {
                    Session["A"] = t;
                    return RedirectToAction("Index", "Admin");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }


    }
}