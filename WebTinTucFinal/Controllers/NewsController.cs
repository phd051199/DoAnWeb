using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTinTucFinal.Models;
using PagedList;
using PagedList.Mvc;
using System.Threading;

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
            return data.BAIDANGs.OrderBy(x => x.IDTheLoai).Take(idtl).ToList();
        }
        public ActionResult Index(int? page)
        {
            int pageSize = 4;
            int pageNum = (page ?? 1);
            var allpost = AllPost();
            return View(allpost.ToPagedList(pageNum, pageSize));
        }
        public ActionResult Baidangmoinhat()
        {
            var moinhat = MoiNhat(4);
            return View(moinhat);
        }
        //Tin theo the loai
       public ActionResult TinTheoTheLoai(int id)
        {
            var baiviet = from bv in data.BAIDANGs where bv.IDTheLoai == id select bv;
            //var baiviettheotheloai = TheoTheLoai(id);
            //ViewBag.tintheotl= TheoTheLoai(id);
            return View(baiviet);
        }

    }
}