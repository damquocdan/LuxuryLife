using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LuxuryLife.Models
{
    public partial class Customer
    {
        [Display(Name = "Mã Khách Hàng")]
        public int CustomerId { get; set; }

        [Display(Name = "Họ và Tên")]
        [Required(ErrorMessage = "Tên không được để trống")]
        public string Name { get; set; } = null!;

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Định dạng email không hợp lệ")]
        public string Email { get; set; } = null!;

        [Display(Name = "Mật Khẩu")]
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Display(Name = "Số Điện Thoại")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string? Phone { get; set; }

        [Display(Name = "Ảnh Đại Diện")]
        [DataType(DataType.ImageUrl, ErrorMessage = "Đường dẫn ảnh không hợp lệ")]
        public string? Avatar { get; set; }

        [Display(Name = "Địa Chỉ")]
        public string? Address { get; set; }

        [Display(Name = "Ngày Sinh")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateOnly? Dob { get; set; }

        [Display(Name = "Nhân Khẩu Học")]
        public string? Demographics { get; set; }

        [Display(Name = "Sở Thích")]
        public string? Preferences { get; set; }

        [Display(Name = "Lịch Sử Tìm Kiếm")]
        public string? SearchHistory { get; set; }

        [Display(Name = "Ngày Tạo Tài Khoản")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? Createdate { get; set; }

        [Display(Name = "Danh Sách Đặt Chỗ")]
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

        [Display(Name = "Danh Sách Yêu Thích")]
        public virtual ICollection<Favourite> Favourites { get; set; } = new List<Favourite>();

        [Display(Name = "Danh Sách Đánh Giá")]
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
