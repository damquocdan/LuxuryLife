using System;
using System.Collections.Generic;

namespace LuxuryLife.Models​;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Avatar { get; set; }

    public string? Address { get; set; }

    public DateOnly? Dob { get; set; }

    public string? Demographics { get; set; }

    public string? Preferences { get; set; }

    public string? SearchHistory { get; set; }

    public DateTime? Createdate { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<Favourite> Favourites { get; set; } = new List<Favourite>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
