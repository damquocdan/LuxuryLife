using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LuxuryLife.Models
{
    public partial class Favourite
    {
        [Display(Name = "Mã Yêu Thích")]
        public int FavoriteId { get; set; }

        [Display(Name = "Mã Khách Hàng")]
        public int? CustomerId { get; set; }

        [Display(Name = "Mã Tour")]
        public int? TourId { get; set; }

        [Display(Name = "Tổng Giá")]
        [DisplayFormat(DataFormatString = "{0:#,##0} VNĐ")]
        public decimal? Sumprice { get; set; }

        [Display(Name = "Thông Tin Khách Hàng")]
        public virtual Customer? Customer { get; set; }

        [Display(Name = "Thông Tin Tour")]
        public virtual Tour? Tour { get; set; }
    }
}