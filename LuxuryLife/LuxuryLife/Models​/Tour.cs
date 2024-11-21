using System;
using System.Collections.Generic;

namespace LuxuryLife.Models​;

public partial class Tour
{
    public int TourId { get; set; }

    public string Name { get; set; } = null!;

    public string? Image { get; set; }

    public string? Description { get; set; }

    public int? ServiceId { get; set; }

    public int? HomestayId { get; set; }

    public decimal? PricePerson { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public decimal? Price { get; set; }

    public string? Status { get; set; }

    public DateTime? Createdate { get; set; }

    public int? ProviderId { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<Favourite> Favourites { get; set; } = new List<Favourite>();

    public virtual ICollection<Listimagestour> Listimagestours { get; set; } = new List<Listimagestour>();

    public virtual Provider? Provider { get; set; }

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
}
