using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LuxuryLife.Models​;

public partial class Provider
{
    [Key]
    public int ProviderId { get; set; }

    [Display(Name = "Tên nhà cung cấp")]
    [Required(ErrorMessage = "Tên nhà cung cấp không được để trống.")]
    public string Name { get; set; } = null!;

    [Display(Name = "Email")]
    [Required(ErrorMessage = "Email không được để trống.")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
    public string Email { get; set; } = null!;

    [Display(Name = "Mật khẩu")]
    [Required(ErrorMessage = "Mật khẩu không được để trống.")]
    [MinLength(8, ErrorMessage = "Mật khẩu phải có ít nhất 8 ký tự.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "Mật khẩu phải chứa ít nhất một chữ hoa, một chữ thường, một số và một ký tự đặc biệt.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [Display(Name = "Ảnh đại diện")]
    [RegularExpression(@".*\\.(jpg|jpeg|png)$", ErrorMessage = "Ảnh đại diện phải là định dạng jpg, jpeg hoặc png.")]
    public string? Avatar { get; set; }

    [Display(Name = "Số điện thoại")]
    [RegularExpression(@"^(\+84|0)[3|5|7|8|9][0-9]{8}$", ErrorMessage = "Số điện thoại không hợp lệ.")]
    public string? Phone { get; set; }

    [Display(Name = "Địa chỉ")]
    public string? Address { get; set; }

    [Display(Name = "Xếp hạng")]
    [Range(0, 5, ErrorMessage = "Xếp hạng phải từ 0 đến 5.")]
    public decimal? Rating { get; set; }

    [Display(Name = "Ngày tạo")]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime? Createdate { get; set; }

    public virtual ICollection<Tour> Tours { get; set; } = new List<Tour>();
}