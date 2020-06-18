using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTinTucFinal.Models;
using PagedList;
using PagedList.Mvc;
using System.Threading;
using Microsoft.Ajax.Utilities;

namespace WebTinTucFinal.Controllers
{
    public class NewsController : Controller
    {
        // GET: News
        dbQLTintucDataContext data = new dbQLTintucDataContext();
        
        public List<BAIDANG> tuyensinh()
        {
            return data.BAIDANGs.Where(x => x.IDTheLoai == 2).OrderByDescending(x => x.IDBaiDang).Take(4).ToList();
        }
        public List<BAIDANG> tinhocduong()
        {
            return data.BAIDANGs.Where(x => x.IDTheLoai == 3).OrderByDescending(x => x.IDBaiDang).Take(4).ToList();
        }
        public List<BAIDANG> duhoc()
        {
            return data.BAIDANGs.Where(x => x.IDTheLoai == 4).OrderByDescending(x => x.IDBaiDang).Take(4).ToList();
        }
        public List<BAIDANG> giaoduc40()
        {
            return data.BAIDANGs.Where(x => x.IDTheLoai == 5).OrderByDescending(x => x.IDBaiDang).Take(4).ToList();
        }
        public List<BAIDANG> list1()
        {
            return data.BAIDANGs.OrderByDescending(x => x.NgayDang).Take(1).ToList();
        }
        public List<BAIDANG> list2345()
        {
            return data.BAIDANGs.OrderByDescending(x => x.NgayDang).Take(5).Skip(1).ToList();
        }

        public ActionResult Chuyenmuc()
        {
            var chuyenmuc = from cm in data.CHUYENMUCs select cm;
            return PartialView(chuyenmuc);
        }

        //Lấy tất cả bài đăng ra
        public List<BAIDANG> AllPost()
        {
            return data.BAIDANGs.OrderByDescending(x => x.IDBaiDang).ToList();
        }
        //
        public List<BAIDANG> MoiNhat(int i)
        {
            return data.BAIDANGs.OrderByDescending(x => x.NgayDang).ToList();
        }
        public List<BAIDANG> TheoTheLoai(int idtl)
        {
            return data.BAIDANGs.OrderBy(x => x.IDTheLoai).ToList();
        }
       
        public ActionResult Index(int? page)
        {

            ViewBag.duhoc = new NewsController().duhoc();
            ViewBag.thd = new NewsController().tinhocduong();
            ViewBag.ts = new NewsController().tuyensinh();
            ViewBag.gd4 = new NewsController().giaoduc40();
            ViewBag.list1 = new NewsController().list1();
            ViewBag.list2345 = new NewsController().list2345();

            int pageSize = 4;
            int pageNum = (page ?? 1);
            var allpost = AllPost();
            return View(allpost.ToPagedList(pageNum, pageSize));
        }
        //public ActionResult Baidangmoinhat()
        //{
        //    var moinhat = MoiNhat(4);
        //    return View(moinhat);
        //}
        //Tin theo the loai
       public ActionResult TinTheoTheLoai(int id)
        {
            var baiviet = from bv in data.BAIDANGs where bv.IDTheLoai == id select bv;
            return View(baiviet);
        }

        public ActionResult Dangky()
        {
            return View();
        }

        [HttpPost]

        public ActionResult Dangky(FormCollection collection, TAIKHOAN tk)
        {
            var hoten = collection["HotenDG"];
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            var gt = collection["Gioitinh"];
            var email = collection["Email"];
            var diachi = collection["Diachi"];
            var dienthoai = collection["Dienthoai"];
            var ngaysinh = String.Format("{0:MM/dd/yyyy}", collection["Ngaysinh"]);
            var image = collection["Image"];


            if (String.IsNullOrEmpty(hoten))
            {
                ViewData["Loi1"] = "Họ tên độc giả không được để trống";
            }
            else if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi2"] = "Chưa nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi3"] = "Chưa nhập mật khẩu";
            }
            else if (String.IsNullOrEmpty(gt))
            {
                ViewData["Loi4"] = "Giới tính không nên để trống";
            }
            else if (String.IsNullOrEmpty(email))
            {
                ViewData["Loi5"] = "Email không được để trống";
            }
            else if (String.IsNullOrEmpty(diachi))
            {
                ViewData["Loi6"] = "Chưa nhập địa chỉ";
            }
            else if (String.IsNullOrEmpty(dienthoai))
            {
                ViewData["Loi7"] = "Điện thoại không nên để trống";
            }
            else
            {
                tk.HoTen = hoten;
                tk.TenTaiKhoan = tendn;
                tk.MatKhau = matkhau;
                tk.GioiTinh = gt;
                tk.Email = email;
                tk.DiaChi = diachi;
                tk.SDT = dienthoai;
                tk.AnhDaiDien = image;
                tk.NgaySinh = DateTime.Parse(ngaysinh);
                tk.QuyenHan = Char.Parse("U");
                tk.TrangThaiNguoiDung = "bình thường";
                data.TAIKHOANs.InsertOnSubmit(tk);
                data.SubmitChanges();
                return RedirectToAction("Dangnhap", "Admin");
            }
            return this.Dangky();
        }


    }
}