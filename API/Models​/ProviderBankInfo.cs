using System;
using System.Collections.Generic;

namespace API.Models​;

public partial class ProviderBankInfo
{
    public int ProviderBankInfoId { get; set; }

    public int ProviderId { get; set; }

    public string BankAccountNumber { get; set; } = null!;

    public string BankName { get; set; } = null!;

    public string? Qrcode { get; set; }

    public string FullName { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual Provider Provider { get; set; } = null!;

    public virtual ICollection<Provider> Providers { get; set; } = new List<Provider>();
}
