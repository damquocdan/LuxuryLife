using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LuxuryLife.Models
{
    public partial class Homestay
    {
        [Display(Name = "Mã Homestay")]
        public int HomestayId { get; set; }

        [Display(Name = "Tên Homestay")]
        [Required(ErrorMessage = "Tên homestay là bắt buộc.")]
        public string Name { get; set; } = null!;

        [Display(Name = "Mô Tả")]
        public string? Description { get; set; }

        [Display(Name = "Địa Chỉ")]
        public string? Address { get; set; }

        [Display(Name = "Hình Ảnh")]
        public string? Image { get; set; }

        [Display(Name = "Giá Mỗi Đêm")]
        [DisplayFormat(DataFormatString = "{0:#,##0} VNĐ")]
        public decimal? PricePerNight { get; set; }

        [Display(Name = "Mã Nhà Cung Cấp")]
        public int? ProviderId { get; set; }
        [Display(Name = "Mã Tour")]
        public int? TourId { get; set; }
        public virtual Tour? Tour { get; set; }
    }
}