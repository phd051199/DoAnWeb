using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTinTucFinal.Models;
using System.Data.Entity;

namespace WebTinTucFinal.Controllers
{
    public class NewsController : Controller
    {
        // GET: News
        dbQLTintucDataContext data = new dbQLTintucDataContext();
       
        public ActionResult Chuyenmuc()
        {
            var chuyenmuc = from cm in data.CHUYENMUCs select cm;
            return PartialView(chuyenmuc);
        }

        public ActionResult TinTheoChuyenmuc( int id)
        {
            var tintuc = from t in data.BAIDANGs where t.IDBaiDang == id select t;
            return View(tintuc);
        }
        //Lấy tất cả bài đăng ra
        public List<BAIDANG> AllPost()
        {
            return data.BAIDANGs.OrderByDescending(x => x.IDBaiDang).ToList();
        }
        //
        public List<BAIDANG> MoiNhat()
        {
            return data.BAIDANGs.OrderByDescending(x => x.NgayDang).ToList();
        }
 
        public ActionResult Index()
        {
            var allpost = AllPost();
            return View(allpost);
        }
        public ActionResult Baidangmoinhat()
        {
            var moinhat = MoiNhat();
            return View(moinhat);
        }
        //Tin theo the loai
       public ActionResult TinTheoTheLoai(int id)
        {
            var baiviet = from bv in data.BAIDANGs where bv.IDTheLoai == id select bv;
            return View(baiviet);
        }


    }
}