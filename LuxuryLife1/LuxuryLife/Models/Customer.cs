using System;
using System.Collections.Generic;

namespace LuxuryLife.Models;

public class Customer
{
    public int CustomerId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Phone { get; set; }
    public string Avatar { get; set; }
    public string Address { get; set; }
    public DateTime Dob { get; set; }
    public string Demographics { get; set; }
    public string Preferences { get; set; }
    public string SearchHistory { get; set; }
    public DateTime Createdate { get; set; }

    // Mối quan hệ: Một Customer có nhiều Booking, Favourite và Review
    public ICollection<Booking> Bookings { get; set; }
    public ICollection<Favourite> Favourites { get; set; }
    public ICollection<Review> Reviews { get; set; }
}