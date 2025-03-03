using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace LuxuryLife.Models;

public partial class Customer
{
    [Key]
    public int CustomerId { get; set; }

    [Required(ErrorMessage = "Tên không được để trống.")]
    [MinLength(3, ErrorMessage = "Tên phải có ít nhất 3 ký tự.")]
    [DisplayName("Họ và tên")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Email không được để trống.")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
    [DisplayName("Email")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Mật khẩu không được để trống.")]
    [MinLength(8, ErrorMessage = "Mật khẩu phải có ít nhất 8 ký tự.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]+$",
        ErrorMessage = "Mật khẩu phải chứa ít nhất một chữ hoa, một chữ thường, một số và một ký tự đặc biệt.")]
    [DisplayName("Mật khẩu")]
    public string Password { get; set; } = null!;

    [RegularExpression(@"^(\+?\d{1,3}[- ]?)?\d{10}$", ErrorMessage = "Số điện thoại không hợp lệ.")]
    [DisplayName("Số điện thoại")]
    public string? Phone { get; set; }

    [DisplayName("Ảnh đại diện")]
    public string? Avatar { get; set; }

    [DisplayName("Địa chỉ")]
    public string? Address { get; set; }

    [CustomValidation(typeof(Customer), nameof(ValidateDob))]
    [DisplayName("Ngày sinh")]
    public DateOnly? Dob { get; set; }

    [DisplayName("Thông tin nhân khẩu học")]
    public string? Demographics { get; set; }

    [DisplayName("Sở thích")]
    public string? Preferences { get; set; }

    [DisplayName("Lịch sử tìm kiếm")]
    public string? SearchHistory { get; set; }

    [CustomValidation(typeof(Customer), nameof(ValidateCreatedate))]
    [DisplayName("Ngày tạo tài khoản")]
    public DateTime? Createdate { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    public virtual ICollection<Contact> Contacts { get; set; } = new List<Contact>();
    public virtual ICollection<CustomerInteraction> CustomerInteractions { get; set; } = new List<CustomerInteraction>();
    public virtual ICollection<Favourite> Favourites { get; set; } = new List<Favourite>();
    public virtual ICollection<MomoPayment> MomoPayments { get; set; } = new List<MomoPayment>();
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    // Kiểm tra ngày sinh (Dob) không được lớn hơn ngày hiện tại
    public static ValidationResult? ValidateDob(DateOnly? dob, ValidationContext context)
    {
        if (dob.HasValue && dob.Value > DateOnly.FromDateTime(DateTime.Now))
        {
            return new ValidationResult("Ngày sinh không thể là ngày trong tương lai.");
        }
        return ValidationResult.Success;
    }

    // Kiểm tra Createdate không được lớn hơn ngày hiện tại
    public static ValidationResult? ValidateCreatedate(DateTime? createdate, ValidationContext context)
    {
        if (createdate.HasValue && createdate.Value > DateTime.Now)
        {
            return new ValidationResult("Ngày tạo không thể là ngày trong tương lai.");
        }
        return ValidationResult.Success;
    }
}
