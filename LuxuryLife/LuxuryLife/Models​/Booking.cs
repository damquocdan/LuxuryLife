using System;
using System.Collections.Generic;

namespace LuxuryLife.Models​;

public partial class Booking
{
    public int BookingId { get; set; }

    public int? CustomerId { get; set; }

    public int? TourId { get; set; }

    public DateTime? BookingDate { get; set; }

    public DateTime? CheckInDate { get; set; }

    public DateTime? CheckOutDate { get; set; }

    public decimal? TotalPrice { get; set; }

    public string? Status { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<MomoPayment> MomoPayments { get; set; } = new List<MomoPayment>();

    public virtual Tour? Tour { get; set; }
}
