using System;
using System.Collections.Generic;

namespace GBS.Api.DbModels;

public partial class OrderItem
{
    public int Id { get; set; }

    public int? OrderIdFk { get; set; }

    public int? ProductIdFk { get; set; }

    public int Quantity { get; set; }

    public int? SizeIdFk { get; set; }

    public string? Color { get; set; }

    public decimal? UnitPrice { get; set; }

    public decimal? TotalPrice { get; set; }

    public string? SpecialInstructions { get; set; }

    public bool? IsCompleted { get; set; }

    public int? CompletedQuantity { get; set; }

    public virtual Order? OrderIdFkNavigation { get; set; }

    public virtual ICollection<OrderLabor> OrderLabors { get; set; } = new List<OrderLabor>();

    public virtual Product? ProductIdFkNavigation { get; set; }

    public virtual Lookup? SizeIdFkNavigation { get; set; }
}
