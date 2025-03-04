using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace LuxuryLife.Models​;

public partial class Tour
{
    [Key]
    public int TourId { get; set; }

    [Display(Name = "Tên Tour")]
    [Required(ErrorMessage = "Tên Tour không được để trống.")]
    public string Name { get; set; } = null!;

    [Display(Name = "Hình ảnh")]
    public string? Image { get; set; }

    [Display(Name = "Mô tả")]
    public string? Description { get; set; }

    [Display(Name = "Dịch vụ")]
    public int? ServiceId { get; set; }

    [Display(Name = "Homestay")]
    public int? HomestayId { get; set; }

    [Display(Name = "Giá một người/Ngày")]
    [Column(TypeName = "decimal(18,2)")]
    [DisplayFormat(DataFormatString = "{0:N0} VND", ApplyFormatInEditMode = false)]
    public decimal? PricePerson { get; set; }

    [Display(Name = "Ngày bắt đầu")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime? StartDate { get; set; }

    [Display(Name = "Ngày kết thúc")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime? EndDate { get; set; }

    [Display(Name = "Tổng giá")]
    [Column(TypeName = "decimal(18,2)")]
    [DisplayFormat(DataFormatString = "{0:N0} VND", ApplyFormatInEditMode = false)]
    public decimal? Price { get; set; }

    [Display(Name = "Trạng thái")]
    public string? Status { get; set; }

    [Display(Name = "Ngày tạo")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime? Createdate { get; set; }

    [Display(Name = "Nhà cung cấp")]
    public int? ProviderId { get; set; }


    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<CustomerInteraction> CustomerInteractions { get; set; } = new List<CustomerInteraction>();

    public virtual ICollection<Favourite> Favourites { get; set; } = new List<Favourite>();

    public virtual ICollection<Homestay> Homestays { get; set; } = new List<Homestay>();

    public virtual ICollection<Listimagestour> Listimagestours { get; set; } = new List<Listimagestour>();

    public virtual Provider? Provider { get; set; }

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
}
