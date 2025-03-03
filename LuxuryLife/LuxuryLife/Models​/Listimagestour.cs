using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LuxuryLife.Models;

public partial class Listimagestour
{
    [Key]
    public int ListimagestourId { get; set; }

    [Display(Name = "Tour liên kết")]
    [Required(ErrorMessage = "Tour không được để trống.")]
    public int? TourId { get; set; }

    [Display(Name = "Hình ảnh")]
    [Required(ErrorMessage = "Hình ảnh không được để trống.")]
    [RegularExpression(@"^.*\.(jpg|jpeg|png)$", ErrorMessage = "Chỉ chấp nhận định dạng hình ảnh jpg, jpeg, png.")]
    public string? ImageUrl { get; set; }

    [Display(Name = "Mô tả hình ảnh")]
    [StringLength(500, ErrorMessage = "Mô tả hình ảnh không được vượt quá 500 ký tự.")]
    public string? ImageDescription { get; set; }

    [Display(Name = "Ngày tạo")]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime? Createdate { get; set; }

    public virtual Tour? Tour { get; set; }
}
