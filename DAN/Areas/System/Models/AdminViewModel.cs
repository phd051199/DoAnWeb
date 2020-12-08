using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DAN.Areas.System.Models
{
    public class AdminViewModel
    {
    }

    public class DatetimeViewModel
    {
        public string monthName { get; set; }
        public int value { get; set; }
    }

    public class UserViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập trường này")]
        [Display(Name = "Tài khoản:")]
        public string Uname { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập trường này")]
        [Display(Name = "Mật khẩu:")]
        public string Upass { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập trường này")]
        [Display(Name = "Xác nhận mật khẩu:")]
        public string Rpass { get; set; }
        public long Uid { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập trường này")]
        [Display(Name = "Tài khoản:")]
        public string Uname { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập trường này")]
        [Display(Name = "Mật khẩu:")]
        public string Upass { get; set; }
    }
    public class CategoryViewModel
    {
        public long CId { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập trường này")]
        [Display(Name = "Tên danh mục:")]
        public string Cname { get; set; }
        [Display(Name = "Mô tả:")]
        public string Cdesc { get; set; }
    }
    public class ProductViewModel
    {
        public long PId { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập trường này")]
        [Display(Name = "Tên sản phẩm:")]
        public string Pname { get; set; }
        [Display(Name = "Mô tả, giới thiệu sản phẩm (có thể dùng HTML):")]
        public string Pdesc { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập trường này")]
        [Display(Name = "Giá:")]
        public decimal Price { get; set; }
        [Display(Name = "Khuyến mãi:")]
        public int Sale { get; set; }
        [Display(Name = "Chọn danh mục:")]
        [Required(ErrorMessage = "Vui lòng chọn danh mục")]
        public long CId { get; set; }
        [Display(Name = "Hình ảnh (có thể tải lên nhiều hình):")]
        public IEnumerable<HttpPostedFileBase> image { get; set; }
        public IEnumerable<DAN.Models.Category> category { get; set; }
        public IEnumerable<DAN.Models.Image> img { get; set; }
    }
    public class SaleViewModel
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập trường này")]
        [Display(Name = "Tiêu đề tin:")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập trường này")]
        [Display(Name = "Nội dung:")]
        public string Content { get; set; }
        public DateTime CreatOn { get; set; }
    }
}