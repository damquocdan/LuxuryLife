using System;
using System.Collections.Generic;

namespace LuxuryLife.Models​;

public partial class ProviderRevenue
{
    public int RevenueId { get; set; }

    public int ProviderId { get; set; }

    public decimal RevenueAmount { get; set; }

    public int RevenueMonth { get; set; }

    public int RevenueYear { get; set; }

    public DateTime? CreatedAt { get; set; }
    public string? Status { get; set; }
    public virtual Provider Provider { get; set; } = null!;
}
