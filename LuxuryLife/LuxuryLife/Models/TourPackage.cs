using System;
using System.Collections.Generic;

namespace LuxuryLife.Models;

public partial class TourPackage
{
    public int PackageId { get; set; }

    public int? TourId { get; set; }

    public string? PackageName { get; set; }

    public decimal? PackagePrice { get; set; }

    public string? Description { get; set; }

    public virtual Tour? Tour { get; set; }
}
