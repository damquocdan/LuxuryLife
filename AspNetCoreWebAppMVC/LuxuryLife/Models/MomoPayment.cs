using System;
using System.Collections.Generic;

namespace LuxuryLife.Models​;

public partial class MomoPayment
{
    public int PaymentId { get; set; }

    public int? BookingId { get; set; }

    public int? CustomerId { get; set; }

    public decimal Amount { get; set; }

    public string? PaymentStatus { get; set; }

    public DateTime? PaymentDate { get; set; }

    public string? RequestId { get; set; }

    public string? PaymentUrl { get; set; }

    public string? TransactionId { get; set; }

    public virtual Booking? Booking { get; set; }

    public virtual Customer? Customer { get; set; }
}
