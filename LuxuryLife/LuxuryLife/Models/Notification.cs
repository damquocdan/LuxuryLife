using System;
using System.Collections.Generic;

namespace LuxuryLife.Models;

public partial class Notification
{
    public int NotificationId { get; set; }

    public int? RecipientId { get; set; }

    public string? RecipientType { get; set; }

    public string? Message { get; set; }

    public DateTime? NotificationDate { get; set; }

    public bool? IsRead { get; set; }
}
