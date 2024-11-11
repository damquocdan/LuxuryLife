using System;
using System.Collections.Generic;

namespace LuxuryLife.Models;

public partial class Favorite
{
    public int FavoriteId { get; set; }

    public int CustomerId { get; set; }

    public int? HomestayId { get; set; }

    public int? TourId { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Homestay? Homestay { get; set; }

    public virtual Tour? Tour { get; set; }
}
