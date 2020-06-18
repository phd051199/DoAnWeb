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
        public List<BAIDANG> daotao()
        {
            return data.BAIDANGs.Where(x => x.IDTheLoai == 1).OrderByDescending(x => x.IDBaiDang).Take(4).ToList();
        }
        public List<BAIDANG> nghiencuukhoahoc()
        {
            return data.BAIDANGs.Where(x => x.IDTheLoai == 2).OrderByDescending(x => x.IDBaiDang).Take(4).ToList();
        }
        public List<BAIDANG> tinhocduong()
        {
            return data.BAIDANGs.Where(x => x.IDTheLoai == 3).OrderByDescending(x => x.IDBaiDang).Take(4).ToList();
        }
        public List<BAIDANG> tuyensinh()
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
            ViewBag.dt = new NewsController().daotao();
            ViewBag.nckh = new NewsController().nghiencuukhoahoc();
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

    }
}