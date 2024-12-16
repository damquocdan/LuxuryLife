using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LuxuryLife.Models
{
    public partial class Service
    {
        [Display(Name = "Mã Dịch Vụ")]
        public int ServiceId { get; set; }

        [Display(Name = "Tên Dịch Vụ")]
        [Required(ErrorMessage = "Tên dịch vụ không được để trống.")]
        public string ServiceName { get; set; } = null!;

        [Display(Name = "Mô Tả Dịch Vụ")]
        public string? Description { get; set; }

        [Display(Name = "Giá Dịch Vụ")]
        [DisplayFormat(DataFormatString = "{0:#,##0} VNĐ")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá dịch vụ phải lớn hơn hoặc bằng 0.")]
        public decimal? Price { get; set; }

        [Display(Name = "Mã Tour")]
        public int? TourId { get; set; }

        public virtual Tour? Tour { get; set; }
    }
}