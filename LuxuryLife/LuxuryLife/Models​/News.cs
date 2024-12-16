using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LuxuryLife.Models
{
    public partial class News
    {
        [Display(Name = "Mã Tin Tức")]
        public int NewId { get; set; }

        [Display(Name = "Tiêu Đề")]
        [Required(ErrorMessage = "Tiêu đề không được để trống.")]
        public string Title { get; set; } = null!;

        [Display(Name = "Nội Dung")]
        [Required(ErrorMessage = "Nội dung không được để trống.")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; } = null!;

        [Display(Name = "URL Hình Ảnh")]
        [DataType(DataType.ImageUrl)]
        public string? ImageUrl { get; set; }

        [Display(Name = "Ngày Tạo")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)] // Định dạng ngày
        public DateTime? Createdate { get; set; }
    }
}