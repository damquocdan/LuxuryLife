using System;
using System.Collections.Generic;

namespace LuxuryLife.Models​;

public partial class Review
{
    public int ReviewId { get; set; }

    public int? CustomerId { get; set; }

    public int? TourId { get; set; }

    public decimal Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime? Createdate { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Tour? Tour { get; set; }
}
