using System;
using System.Collections.Generic;

namespace GBS.Api.DbModels;

public partial class OrderStatusHistory
{
    public int Id { get; set; }

    public int? OrderIdFk { get; set; }

    public int? StatusIdFk { get; set; }

    public DateTime? StatusDate { get; set; }

    public int? ChangedBy { get; set; }

    public string? Notes { get; set; }

    public virtual Order? OrderIdFkNavigation { get; set; }

    public virtual Lookup? StatusIdFkNavigation { get; set; }
}
