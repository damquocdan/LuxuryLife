using System;
using System.Collections.Generic;

namespace LuxuryLife.Models;

public partial class BookingService
{
    public int BookingServiceId { get; set; }

    public int? BookingId { get; set; }

    public string? ServiceName { get; set; }

    public decimal? ServicePrice { get; set; }

    public virtual Booking? Booking { get; set; }
}
