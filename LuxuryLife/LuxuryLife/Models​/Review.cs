using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LuxuryLife.Models​;

public partial class Review
{
    [Key]
    public int ReviewId { get; set; }

    [Display(Name = "Khách hàng")]
    public int? CustomerId { get; set; }

    [Display(Name = "Tour")]
    public int? TourId { get; set; }

    [Display(Name = "Xếp hạng")]
    [Range(1, 5, ErrorMessage = "Xếp hạng phải từ 1 đến 5.")]
    public decimal Rating { get; set; }

    [Display(Name = "Bình luận")]
    public string? Comment { get; set; }

    [Display(Name = "Ngày đánh giá")]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime? Createdate { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<ReviewOn> ReviewOns { get; set; } = new List<ReviewOn>();

    public virtual Tour? Tour { get; set; }
}
