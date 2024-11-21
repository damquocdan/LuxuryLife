using System;
using System.Collections.Generic;

namespace LuxuryLife.Models;

public class Booking
{
    public int BookingId { get; set; }
    public int CustomerId { get; set; }
    public int TourId { get; set; }
    public DateTime BookingDate { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public string Status { get; set; }
    public decimal TotalPrice { get; set; }

    // Mối quan hệ: Booking thuộc về Customer và Tour
    public Customer Customer { get; set; }
    public Tour Tour { get; set; }
}
