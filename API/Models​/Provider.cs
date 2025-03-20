using System;
using System.Collections.Generic;

namespace API.Models​;

public partial class Provider
{
    public int ProviderId { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Avatar { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public decimal? Rating { get; set; }

    public DateTime? Createdate { get; set; }

    public virtual ICollection<Tour> Tours { get; set; } = new List<Tour>();
}
