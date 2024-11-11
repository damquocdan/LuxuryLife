using System;
using System.Collections.Generic;

namespace LuxuryLife.Models;

public partial class CustomerSupport
{
    public int SupportId { get; set; }

    public int? CustomerId { get; set; }

    public string? IssueDescription { get; set; }

    public string? SupportResponse { get; set; }

    public string? Status { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? ResolvedDate { get; set; }

    public bool? IsResolved { get; set; }

    public virtual Customer? Customer { get; set; }
}
