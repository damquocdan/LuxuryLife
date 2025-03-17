using System;
using System.Collections.Generic;

namespace ASPNETCoreWebAPI.Models​;

public partial class Service
{
    public int ServiceId { get; set; }

    public string ServiceName { get; set; } = null!;

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public int? TourId { get; set; }

    public string? Image { get; set; }

    public virtual Tour? Tour { get; set; }
}
