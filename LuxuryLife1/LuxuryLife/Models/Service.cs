using System;
using System.Collections.Generic;

namespace LuxuryLife.Models;

public class Service
{
    public int ServiceId { get; set; }
    public string ServiceName { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int TourId { get; set; }

    // Mối quan hệ: Một Service thuộc về một Tour
}