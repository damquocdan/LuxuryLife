using System;
using System.Collections.Generic;

namespace LuxuryLife.Models;

public partial class PaymentMethod
{
    public int MethodId { get; set; }

    public string? MethodName { get; set; }

    public string? Description { get; set; }
}
