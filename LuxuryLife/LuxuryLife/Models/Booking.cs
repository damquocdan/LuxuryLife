using System;
using System.Collections.Generic;

namespace LuxuryLife.Models;

public partial class Booking
{
    public int BookingId { get; set; }

    public int? CustomerId { get; set; }

    public int? HomestayId { get; set; }

    public int? TourId { get; set; }

    public DateOnly BookingDate { get; set; }

    public DateOnly? CheckInDate { get; set; }

    public DateOnly? CheckOutDate { get; set; }

    public string? Status { get; set; }

    public double? TotalPrice { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual ICollection<BookingService> BookingServices { get; set; } = new List<BookingService>();

    public virtual Customer? Customer { get; set; }

    public virtual Homestay? Homestay { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Tour? Tour { get; set; }
}
