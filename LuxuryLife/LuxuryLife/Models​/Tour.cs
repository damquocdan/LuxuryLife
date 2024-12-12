using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LuxuryLife.Models
{
    public partial class Tour
    {
        [Display(Name = "Mã Tour")]
        public int TourId { get; set; }

        [Display(Name = "Tên Tour")]
        [Required(ErrorMessage = "Tên tour không được để trống.")]
        public string Name { get; set; } = null!;

        [Display(Name = "Hình Ảnh")]
        public string? Image { get; set; }

        [Display(Name = "Mô Tả Tour")]
        public string? Description { get; set; }

        [Display(Name = "Mã Dịch Vụ")]
        public int? ServiceId { get; set; }

        [Display(Name = "Mã Homestay")]
        public int? HomestayId { get; set; }

        [Display(Name = "Giá Mỗi Người")]
        [DataType(DataType.Currency)] // Định dạng tiền tệ
        [Range(0, double.MaxValue, ErrorMessage = "Giá mỗi người phải lớn hơn hoặc bằng 0.")]
        public decimal? PricePerson { get; set; }

        [Display(Name = "Ngày Khởi Hành")]
        [DataType(DataType.Date)] // Định dạng ngày
        public DateTime? StartDate { get; set; }

        [Display(Name = "Ngày Kết Thúc")]
        [DataType(DataType.Date)] // Định dạng ngày
        public DateTime? EndDate { get; set; }

        [Display(Name = "Giá Tour")]
        [DisplayFormat(DataFormatString = "{0:#,##0} VNĐ")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá tour phải lớn hơn hoặc bằng 0.")]
        public decimal? Price { get; set; }

        [Display(Name = "Trạng Thái")]
        public string? Status { get; set; }

        [Display(Name = "Ngày Tạo")]
        [DataType(DataType.Date)] // Định dạng ngày
        public DateTime? Createdate { get; set; }

        [Display(Name = "Mã Nhà Cung Cấp")]
        public int? ProviderId { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

        public virtual ICollection<Favourite> Favourites { get; set; } = new List<Favourite>();

        public virtual ICollection<Listimagestour> Listimagestours { get; set; } = new List<Listimagestour>();

        public virtual Provider? Provider { get; set; }

        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

        public virtual ICollection<Service> Services { get; set; } = new List<Service>();
    }
}
