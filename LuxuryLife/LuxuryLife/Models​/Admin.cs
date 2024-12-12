using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; // Import namespace cho Display

namespace LuxuryLife.Models
{
    public partial class Admin
    {
        [Display(Name = "Mã Quản Trị Viên")]
        public int AdminId { get; set; }

        [Display(Name = "Tên Quản Trị Viên")]
        [Required(ErrorMessage = "Tên không được để trống")]
        public string Name { get; set; } = null!;

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string Email { get; set; } = null!;

        [Display(Name = "Mật Khẩu")]
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Display(Name = "Ảnh Đại Diện")]
        [DataType(DataType.ImageUrl, ErrorMessage = "Đường dẫn ảnh không hợp lệ")]
        public string? Avatar { get; set; }
    }
}
