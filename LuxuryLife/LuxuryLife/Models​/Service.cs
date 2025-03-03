using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace LuxuryLife.Models;

public partial class Service
{
    [Key]
    public int ServiceId { get; set; }

    [Display(Name = "Tên dịch vụ")]
    [Required(ErrorMessage = "Tên dịch vụ không được để trống.")]
    public string ServiceName { get; set; } = null!;

    [Display(Name = "Mô tả")]
    public string? Description { get; set; }

    [Display(Name = "Giá dịch vụ")]
    [Column(TypeName = "decimal(18,2)")]
    [DisplayFormat(DataFormatString = "{0:N0} VND", ApplyFormatInEditMode = false)]
    public decimal? Price { get; set; }

    [Display(Name = "Tour liên kết")]
    public int? TourId { get; set; }

    public virtual Tour? Tour { get; set; }

    public string GetFormattedPrice()
    {
        return Price.HasValue ? string.Format(new CultureInfo("vi-VN"), "{0:N0} VND", Price.Value) : "Chưa có giá";
    }
}
