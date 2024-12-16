using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LuxuryLife.Models
{
    public partial class Listimagestour
    {
        [Display(Name = "Mã Hình Ảnh Tour")]
        public int ListimagestourId { get; set; }

        [Display(Name = "Mã Tour")]
        public int? TourId { get; set; }

        [Display(Name = "URL Hình Ảnh")]
        [DataType(DataType.ImageUrl)]
        public string? ImageUrl { get; set; }

        [Display(Name = "Mô Tả Hình Ảnh")]
        public string? ImageDescription { get; set; }

        [Display(Name = "Ngày Tạo")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)] // Định dạng ngày
        public DateTime? Createdate { get; set; }

        [Display(Name = "Thông Tin Tour")]
        public virtual Tour? Tour { get; set; }
    }
}