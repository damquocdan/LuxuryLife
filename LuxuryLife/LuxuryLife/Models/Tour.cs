using System;
using System.Collections.Generic;

namespace LuxuryLife.Models;

public partial class Tour
{
    public int TourId { get; set; }

    public string Name { get; set; } = null!;
    public string? Image {  get; set; } 
    public int? ListimageId { get; set; }

    public string? Description { get; set; }

    public string Destination { get; set; } = null!;

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public double? PricePerPerson { get; set; }

    public int? MaxParticipants { get; set; }

    public int? AvailableSeats { get; set; }

    public double? Rating { get; set; }

    public int? ProviderId { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    public virtual ICollection<Image> Images { get; set; } = new List<Image>();

    public virtual ICollection<News> News { get; set; } = new List<News>();

    public virtual Provider? Provider { get; set; }
    public virtual Listimage? Listimage { get; set; }

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();

    public virtual ICollection<TourPackage> TourPackages { get; set; } = new List<TourPackage>();

    public virtual ICollection<TourSchedule> TourSchedules { get; set; } = new List<TourSchedule>();
}
