using System;
using System.Collections.Generic;

namespace LuxuryLife.Models;

public partial class Voucher
{
    public int VoucherId { get; set; }

    public string? Code { get; set; }

    public decimal? DiscountPercentage { get; set; }

    public DateTime? ValidFrom { get; set; }

    public DateTime? ValidUntil { get; set; }

    public decimal? MinSpendAmount { get; set; }

    public string? Status { get; set; }
}
