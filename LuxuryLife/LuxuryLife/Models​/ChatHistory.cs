using System;
using System.Collections.Generic;

namespace LuxuryLife.Models​;

public partial class ChatHistory
{
    public int ChatId { get; set; }

    public int CustomerId { get; set; }

    public string Message { get; set; } = null!;

    public string Response { get; set; } = null!;

    public DateTime? CreatedDate { get; set; }

    public virtual Customer Customer { get; set; } = null!;
}
