using System;
using System.Collections.Generic;

namespace GBS.Api.DbModels;

public partial class Payment
{
    public int Id { get; set; }

    public string? Reference { get; set; }

    public int OrderIdFk { get; set; }

    public DateTime PaymentDate { get; set; }

    public decimal Amount { get; set; }

    public int PaymentMethodIdFk { get; set; }

    public string? Notes { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual Order OrderIdFkNavigation { get; set; } = null!;

    public virtual Lookup PaymentMethodIdFkNavigation { get; set; } = null!;
}
