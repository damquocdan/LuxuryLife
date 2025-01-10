using System;
using System.Collections.Generic;

namespace ASPNETCoreWebAPITour.Models​;

public partial class Homestay
{
    public int HomestayId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? Address { get; set; }

    public string? Image { get; set; }

    public decimal? PricePerNight { get; set; }

    public int? ProviderId { get; set; }

    public int? TourId { get; set; }

    public virtual Tour? Tour { get; set; }
}
