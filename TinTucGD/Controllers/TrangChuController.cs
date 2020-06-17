using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TinTucGD.Models;

namespace TinTucGD.Controllers
{
    public class TrangChuController : Controller
    {
        dbQLTintucDataContext data = new dbQLTintucDataContext();
        // GET: TrangChu
        int pageSize = 4;
        public ActionResult TrangChu()
        {
            data = new dbQLTintucDataContext();
            ViewBag.pageCount = Math.Ceiling(1.0 * data.BAIVIETs.ToList().Count / pageSize);
            return View();
        }

        public ActionResult LoadBaiVietPartial(int pageNo, int pageSize = 4)
        {
            data = new dbQLTintucDataContext();
            var l = data.BAIVIETs.ToList();
            var model = l.Skip(pageNo * pageSize).Take(pageSize).ToList();
            return PartialView(model);
        }
        public ViewResult BaiVietTheoLoai(string maLoai = "1")
        {

            LOAIBAIVIET l = data.LOAIBAIVIETs.SingleOrDefault(n => n.MaLoai == maLoai);
            if (l == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            List<BAIVIET> lstBV = data.BAIVIETs.Where(n => n.MaLoai == maLoai).ToList();
            if (lstBV.Count == 0)
            {
                ViewBag.BaiViet = "Không có bài viết nào thuộc chủ đề" + l.TenLoai;

            }

            ViewBag.lstLoaiBV = data.LOAIBAIVIETs.ToList();
            return View(lstBV);
        }
        public ActionResult Search(string query)
        {
            data = new dbQLTintucDataContext();
            List<BAIVIET> list = data.BAIVIETs.Where(x =>
                x.TuaDe.ToLower().Contains(query.ToLower())).ToList<BAIVIET>();
            return PartialView(list);
        }
        public ActionResult Help()
        {
            ViewBag.Message = "Thông tin liên lạc";
            return View();
        }
    }
}