﻿using System;
using System.Collections.Generic;

namespace LuxuryLife.Models​;

public partial class Favourite
{
    public int FavoriteId { get; set; }

    public int? CustomerId { get; set; }

    public int? TourId { get; set; }

    public decimal? Sumprice { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Tour? Tour { get; set; }
}
