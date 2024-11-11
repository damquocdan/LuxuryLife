using System;
using System.Collections.Generic;

namespace LuxuryLife.Models;

public partial class Provider
{
    public int ProviderId { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public double? Revenue { get; set; }

    public double? Rating { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual ICollection<Homestay> Homestays { get; set; } = new List<Homestay>();

    public virtual ICollection<News> News { get; set; } = new List<News>();

    public virtual ICollection<Tour> Tours { get; set; } = new List<Tour>();
}
