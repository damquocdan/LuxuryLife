using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LuxuryLife.Models;

public partial class Homestay
{
    [Key]
    public int HomestayId { get; set; }

    [Display(Name = "Tên Homestay")]
    [Required(ErrorMessage = "Tên Homestay không được để trống.")]
    [StringLength(255, ErrorMessage = "Tên Homestay không được vượt quá 255 ký tự.")]
    public string Name { get; set; } = null!;

    [Display(Name = "Mô tả")]
    [StringLength(1000, ErrorMessage = "Mô tả không được vượt quá 1000 ký tự.")]
    public string? Description { get; set; }

    [Display(Name = "Địa chỉ")]
    [Required(ErrorMessage = "Địa chỉ không được để trống.")]
    [StringLength(500, ErrorMessage = "Địa chỉ không được vượt quá 500 ký tự.")]
    public string? Address { get; set; }

    [Display(Name = "Hình ảnh")]
    [RegularExpression(@"^.*\.(jpg|jpeg|png)$", ErrorMessage = "Chỉ chấp nhận định dạng hình ảnh jpg, jpeg, png.")]
    public string? Image { get; set; }

    [Display(Name = "Giá mỗi đêm")]
    [Column(TypeName = "decimal(18,2)")]
    [Range(0, double.MaxValue, ErrorMessage = "Giá phải là số dương.")]
    [DisplayFormat(DataFormatString = "{0:N0} VND", ApplyFormatInEditMode = false)]
    public decimal? PricePerNight { get; set; }

    [Display(Name = "Nhà cung cấp")]
    [Required(ErrorMessage = "Nhà cung cấp không được để trống.")]
    public int? ProviderId { get; set; }

    [Display(Name = "Tour liên kết")]
    public int? TourId { get; set; }

    public virtual Tour? Tour { get; set; }
}
