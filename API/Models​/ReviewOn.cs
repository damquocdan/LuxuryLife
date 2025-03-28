using System;
using System.Collections.Generic;

namespace API.Models​;

public partial class ReviewOn
{
    public int ReviewOnId { get; set; }

    public int ReviewId { get; set; }

    public int CustomerId { get; set; }

    public string Comment { get; set; } = null!;

    public DateTime? Createdate { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Review Review { get; set; } = null!;
}
