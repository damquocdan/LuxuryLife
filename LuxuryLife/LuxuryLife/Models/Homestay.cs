using System;
using System.Collections.Generic;

namespace LuxuryLife.Models;

public partial class Homestay
{
    public int HomestayId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string Location { get; set; } = null!;

    public double? PricePerNight { get; set; }

    public bool? Availability { get; set; }

    public double? Rating { get; set; }

    public int? ProviderId { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    public virtual ICollection<Image> Images { get; set; } = new List<Image>();

    public virtual ICollection<News> News { get; set; } = new List<News>();

    public virtual Provider? Provider { get; set; }

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
}
