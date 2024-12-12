using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LuxuryLife.Models
{
    public partial class Provider
    {
        [Display(Name = "Mã Nhà Cung Cấp")]
        public int ProviderId { get; set; }

        [Display(Name = "Tên Nhà Cung Cấp")]
        [Required(ErrorMessage = "Tên nhà cung cấp không được để trống.")]
        public string Name { get; set; } = null!;

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email không được để trống.")]
        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ.")]
        public string Email { get; set; } = null!;

        [Display(Name = "Mật Khẩu")]
        [Required(ErrorMessage = "Mật khẩu không được để trống.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Display(Name = "Ảnh Đại Diện")]
        [DataType(DataType.ImageUrl)]
        public string? Avatar { get; set; }

        [Display(Name = "Số Điện Thoại")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        public string? Phone { get; set; }

        [Display(Name = "Địa Chỉ")]
        public string? Address { get; set; }

        [Display(Name = "Đánh Giá")]
        [Range(0, 5, ErrorMessage = "Đánh giá phải nằm trong khoảng 0 đến 5.")]
        public decimal? Rating { get; set; }

        [Display(Name = "Ngày Tạo")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Createdate { get; set; }

        [Display(Name = "Danh Sách Tours")]
        public virtual ICollection<Tour> Tours { get; set; } = new List<Tour>();
    }
}
