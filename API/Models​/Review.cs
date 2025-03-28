using System;
using System.Collections.Generic;

namespace API.Models​;

public partial class Review
{
    public int ReviewId { get; set; }

    public int? CustomerId { get; set; }

    public int? TourId { get; set; }

    public decimal Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime? Createdate { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<ReviewOn> ReviewOns { get; set; } = new List<ReviewOn>();

    public virtual Tour? Tour { get; set; }
}
