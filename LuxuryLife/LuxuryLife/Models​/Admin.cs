using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace LuxuryLife.Models;

public partial class Admin
{
    [Key]
    public int AdminId { get; set; }

    [Display(Name = "Tên Admin")]
    [Required(ErrorMessage = "Tên không được để trống.")]
    [StringLength(255, ErrorMessage = "Tên không được vượt quá 255 ký tự.")]
    public string Name { get; set; } = null!;

    [Display(Name = "Email")]
    [Required(ErrorMessage = "Email không được để trống.")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
    public string Email { get; set; } = null!;

    [Display(Name = "Mật khẩu")]
    [Required(ErrorMessage = "Mật khẩu không được để trống.")]
    [MinLength(8, ErrorMessage = "Mật khẩu phải có ít nhất 8 ký tự.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$",
        ErrorMessage = "Mật khẩu phải có ít nhất 1 chữ hoa, 1 chữ thường, 1 số và 1 ký tự đặc biệt.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [Display(Name = "Ảnh đại diện")]
    [RegularExpression(@"^.*\.(jpg|jpeg|png)$", ErrorMessage = "Chỉ chấp nhận định dạng hình ảnh jpg, jpeg, png.")]
    public string? Avatar { get; set; }
}
