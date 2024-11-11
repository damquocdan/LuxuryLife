using System;
using System.Collections.Generic;

namespace LuxuryLife.Models;

public partial class Transaction
{
    public int TransactionId { get; set; }

    public int? PaymentId { get; set; }

    public int? CustomerId { get; set; }

    public decimal? Amount { get; set; }

    public DateTime? TransactionDate { get; set; }

    public string? TransactionStatus { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Payment? Payment { get; set; }
}
