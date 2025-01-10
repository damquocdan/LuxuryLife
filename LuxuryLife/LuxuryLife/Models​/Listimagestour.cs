using System;
using System.Collections.Generic;

namespace LuxuryLife.Models​;

public partial class Listimagestour
{
    public int ListimagestourId { get; set; }

    public int? TourId { get; set; }

    public string? ImageUrl { get; set; }

    public string? ImageDescription { get; set; }

    public DateTime? Createdate { get; set; }

    public virtual Tour? Tour { get; set; }
}
