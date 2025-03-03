using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace LuxuryLife.Models;

public partial class Contact
{
    [Key]
    public int ContactId { get; set; }

    public int? CustomerId { get; set; }

    [Required(ErrorMessage = "Tên không được để trống.")]
    [MinLength(3, ErrorMessage = "Tên phải có ít nhất 3 ký tự.")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Email không được để trống.")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
    public string Email { get; set; } = null!;

    [RegularExpression(@"^(\+?\d{1,3}[- ]?)?\d{10}$", ErrorMessage = "Số điện thoại không hợp lệ.")]
    public string? Phone { get; set; }

    [Required(ErrorMessage = "Tiêu đề không được để trống.")]
    [MaxLength(255, ErrorMessage = "Tiêu đề không được vượt quá 255 ký tự.")]
    public string Subject { get; set; } = null!;

    [Required(ErrorMessage = "Nội dung tin nhắn không được để trống.")]
    [MinLength(10, ErrorMessage = "Tin nhắn phải có ít nhất 10 ký tự.")]
    public string Message { get; set; } = null!;

    [DataType(DataType.DateTime)]
    [CustomValidation(typeof(Contact), nameof(ValidateCreatedDate))]
    public DateTime? CreatedDate { get; set; }

    [Required(ErrorMessage = "Trạng thái không được để trống.")]
    [RegularExpression(@"^(Pending|Resolved|Closed)$", ErrorMessage = "Trạng thái chỉ có thể là Pending, Resolved hoặc Closed.")]
    public string? Status { get; set; }

    public virtual Customer? Customer { get; set; }

    // Xử lý ngoại lệ cho CreatedDate
    public static ValidationResult? ValidateCreatedDate(DateTime? createdDate, ValidationContext context)
    {
        if (createdDate.HasValue && createdDate.Value > DateTime.Now)
        {
            return new ValidationResult("Ngày tạo không thể là ngày trong tương lai.");
        }
        return ValidationResult.Success;
    }
}
