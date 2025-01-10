using System;
using System.Collections.Generic;

namespace ASPNETCoreWebAPITour.Models​;

public partial class CustomerInteraction
{
    public int InteractionId { get; set; }

    public int? CustomerId { get; set; }

    public int? TourId { get; set; }

    public string? InteractionType { get; set; }

    public DateTime? InteractionDate { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Tour? Tour { get; set; }
}
