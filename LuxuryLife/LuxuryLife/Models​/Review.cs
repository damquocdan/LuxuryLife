using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LuxuryLife.Models
{
    public partial class Review
    {
        [Display(Name = "Mã Đánh Giá")]
        public int ReviewId { get; set; }

        [Display(Name = "Mã Khách Hàng")]
        public int? CustomerId { get; set; }

        [Display(Name = "Mã Tour")]
        public int? TourId { get; set; }

        [Display(Name = "Đánh Giá")]
        [Range(0, 5, ErrorMessage = "Đánh giá phải nằm trong khoảng 0 đến 5.")]
        public decimal Rating { get; set; }

        [Display(Name = "Bình Luận")]
        [DataType(DataType.MultilineText)]
        public string? Comment { get; set; }

        [Display(Name = "Ngày Đánh Giá")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)] // Định dạng ngày
        public DateTime? Createdate { get; set; }

        public virtual Customer? Customer { get; set; }

        public virtual Tour? Tour { get; set; }
    }
}
