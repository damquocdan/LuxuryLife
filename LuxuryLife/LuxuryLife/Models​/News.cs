using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LuxuryLife.Models;

public partial class News
{
    [Key]
    public int NewId { get; set; }

    [Display(Name = "Tiêu đề")]
    [Required(ErrorMessage = "Tiêu đề không được để trống.")]
    [StringLength(255, ErrorMessage = "Tiêu đề không được vượt quá 255 ký tự.")]
    public string Title { get; set; } = null!;

    [Display(Name = "Nội dung")]
    [Required(ErrorMessage = "Nội dung không được để trống.")]
    public string Content { get; set; } = null!;

    [Display(Name = "Hình ảnh")]
    [RegularExpression(@"^.*\.(jpg|jpeg|png)$", ErrorMessage = "Chỉ chấp nhận định dạng hình ảnh jpg, jpeg, png.")]
    public string? ImageUrl { get; set; }

    [Display(Name = "Ngày tạo")]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime? Createdate { get; set; }
}
