using System;
using System.Collections.Generic;

namespace ASPNETCoreWebAPI.Models​;

public partial class Contact
{
    public int ContactId { get; set; }

    public int? CustomerId { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public string Subject { get; set; } = null!;

    public string Message { get; set; } = null!;

    public DateTime? CreatedDate { get; set; }

    public string? Status { get; set; }

    public virtual Customer? Customer { get; set; }
}
