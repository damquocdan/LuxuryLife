using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LuxuryLife.Models;

public partial class Booking
{
    [Key]
    public int BookingId { get; set; }

    [Display(Name = "Khách hàng")]
    [Required(ErrorMessage = "Khách hàng không được để trống.")]
    public int? CustomerId { get; set; }

    [Display(Name = "Tour")]
    [Required(ErrorMessage = "Tour không được để trống.")]
    public int? TourId { get; set; }

    [Display(Name = "Ngày đặt")]
    [DataType(DataType.Date)]
    [CustomValidation(typeof(Booking), nameof(ValidateBookingDate))]
    public DateTime? BookingDate { get; set; }

    [Display(Name = "Ngày bắt đầu")]
    [Required(ErrorMessage = "Ngày bắt đầu không được để trống.")]
    [DataType(DataType.Date)]
    [CustomValidation(typeof(Booking), nameof(ValidateCheckInDate))]
    public DateTime? CheckInDate { get; set; }

    [Display(Name = "Ngày kết thúc")]
    [Required(ErrorMessage = "Ngày kết thúc không được để trống.")]
    [DataType(DataType.Date)]
    [CustomValidation(typeof(Booking), nameof(ValidateCheckOutDate))]
    public DateTime? CheckOutDate { get; set; }

    [Display(Name = "Tổng giá tiền")]
    [Range(0, double.MaxValue, ErrorMessage = "Giá tiền không được nhỏ hơn 0.")]
    [DisplayFormat(DataFormatString = "{0:N0} VND", ApplyFormatInEditMode = false)]
    public decimal? TotalPrice { get; set; }


    [Display(Name = "Trạng thái")]
    [RegularExpression(@"^(Pending|Confirmed|Cancelled)$", ErrorMessage = "Trạng thái chỉ có thể là Pending, Confirmed hoặc Cancelled.")]
    public string? Status { get; set; }

    public virtual Customer? Customer { get; set; }
    public int? NumberOfGuests { get; set; }
    public virtual ICollection<MomoPayment> MomoPayments { get; set; } = new List<MomoPayment>();

    public virtual Tour? Tour { get; set; }

    // Validation methods
    public static ValidationResult? ValidateBookingDate(DateTime? bookingDate, ValidationContext context)
    {
        if (bookingDate.HasValue && bookingDate.Value < DateTime.Now.Date)
        {
            return new ValidationResult("Ngày đặt không được là ngày trong quá khứ.");
        }
        return ValidationResult.Success;
    }

    public static ValidationResult? ValidateCheckInDate(DateTime? checkInDate, ValidationContext context)
    {
        var instance = (Booking)context.ObjectInstance;
        if (checkInDate.HasValue && instance.BookingDate.HasValue && checkInDate.Value < instance.BookingDate.Value)
        {
            return new ValidationResult("Ngày bắt đầu phải sau hoặc trùng với ngày đặt.");
        }
        return ValidationResult.Success;
    }

    public static ValidationResult? ValidateCheckOutDate(DateTime? checkOutDate, ValidationContext context)
    {
        var instance = (Booking)context.ObjectInstance;
        if (checkOutDate.HasValue && instance.CheckInDate.HasValue && checkOutDate.Value <= instance.CheckInDate.Value)
        {
            return new ValidationResult("Ngày kết thúc phải sau ngày nhận phòng.");
        }
        return ValidationResult.Success;
    }
}
