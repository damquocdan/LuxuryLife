using System;
using System.Collections.Generic;

namespace LuxuryLife.Models​;

public partial class Service
{
    public int ServiceId { get; set; }

    public string ServiceName { get; set; } = null!;

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public int? TourId { get; set; }

    public virtual Tour? Tour { get; set; }
}
