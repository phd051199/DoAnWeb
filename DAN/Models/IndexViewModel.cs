using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DAN.Models
{
    public class IndexViewModel
    {
        public IEnumerable<Product> product { get; set; }
        public int total { get; set; }
        public int currentPage { get; set; }
    }
    public class CategoryViewModel
    {
        public Category category { get; set; }
        public IEnumerable<Product> product { get; set; }
        public int currentPage { get; set; }
        public int total { get; set; }
        public int catId { get; set; }
    }
    public class CartViewModel
    {
        public List<Product> product { get; set; }
    }
    public class ProductViewModel
    {
        public Product product { get; set; }
        public IEnumerable<Image> image { get; set; }
        public IEnumerable<Product> related { get; set; }
    }
    public class OrderViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        [Display(Name = "Họ và tên:")]
        public string fullName { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
        [Display(Name = "Địa chỉ nhận hàng:")]
        public string adress { get; set; }
        [Display(Name = "Số điện thoại:")]
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [DataType(DataType.PhoneNumber ,ErrorMessage = "Vui lòng nhập đúng định dạng SĐT")]
        public int phone { get; set; }
        [Display(Name = "Địa chỉ email:")]
        [EmailAddress(ErrorMessage = "Vui lòng nhập đúng định dạng email")]
        public string email { get; set; }
        [Display(Name = "Mã khuyến mãi")]
        public string Code { get; set; }
        public string PaidInfo { get; set; }
    }
    public class InvoiceViewModel
    {
        public Order order { get; set; }
        public IEnumerable<OrderDetail> orderDetail { get; set; }
    }
    public class SearchInvoiceModel
    {
        [Required(ErrorMessage = "Vui lòng nhập mã đơn hàng")]
        [Display(Name = "Mã đơn hàng:")]
        public string OId { get; set; }
    }
    public class ContactViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập trường này")]
        [EmailAddress(ErrorMessage = "Vui lòng nhập đúng định dạng email")]
        [Display(Name = "Email liên hệ:")]
        public string From { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập trường này")]
        [Display(Name = "Tiêu đề:")]
        public string Subject { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập trường này")]
        [Display(Name = "Nội dung:")]
        public string Message { get; set; }
    }
}