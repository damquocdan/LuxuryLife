using System;
using System.Collections.Generic;

namespace LuxuryLife.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int? BookingId { get; set; }

    public double Amount { get; set; }

    public string? PaymentMethod { get; set; }

    public string? PaymentStatus { get; set; }

    public DateOnly? PaymentDate { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual Booking? Booking { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
