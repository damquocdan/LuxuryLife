using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LuxuryLife.Models
{
    public partial class Booking
    {
        [Display(Name = "Mã Đặt Chỗ")]
        public int BookingId { get; set; }

        [Display(Name = "Mã Khách Hàng")]
        public int? CustomerId { get; set; }

        [Display(Name = "Mã Tour")]
        public int? TourId { get; set; }

        [Display(Name = "Ngày Đặt Chỗ")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? BookingDate { get; set; }

        [Display(Name = "Ngày Nhận Phòng")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? CheckInDate { get; set; }

        [Display(Name = "Ngày Trả Phòng")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? CheckOutDate { get; set; }

        [Display(Name = "Tổng Giá Tiền")]
        [DisplayFormat(DataFormatString = "{0:#,##0} VNĐ")]
        public decimal? TotalPrice { get; set; }

        [Display(Name = "Trạng Thái")]
        public string? Status { get; set; } = "Chưa thanh toán";

        [Display(Name = "Khách Hàng")]
        public virtual Customer? Customer { get; set; }

        [Display(Name = "Tour")]
        public virtual Tour? Tour { get; set; }
    }
}