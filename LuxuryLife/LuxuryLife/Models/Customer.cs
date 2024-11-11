using System;
using System.Collections.Generic;

namespace LuxuryLife.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public DateOnly? Dob { get; set; }

    public string? Demographics { get; set; }

    public string? Preferences { get; set; }

    public string? SearchHistory { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<CustomerSupport> CustomerSupports { get; set; } = new List<CustomerSupport>();

    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
