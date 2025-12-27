using System;
using System.Collections.Generic;

namespace GBS.Api.DbModels;

public partial class Order
{
    public int Id { get; set; }

    public string? Reference { get; set; }

    public int ClientIdFk { get; set; }

    public DateTime OrderDate { get; set; }

    public DateTime DeliveryDate { get; set; }

    public int StatusIdFk { get; set; }

    public int TotalQuantity { get; set; }

    public decimal TotalAmount { get; set; }

    public decimal AdvanceAmount { get; set; }

    public decimal BalanceAmount { get; set; }

    public string? Notes { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public virtual Client ClientIdFkNavigation { get; set; } = null!;

    public virtual ICollection<OrderCost> OrderCosts { get; set; } = new List<OrderCost>();

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<OrderLabor> OrderLabors { get; set; } = new List<OrderLabor>();

    public virtual ICollection<OrderMaterial> OrderMaterials { get; set; } = new List<OrderMaterial>();

    public virtual ICollection<OrderStatusHistory> OrderStatusHistories { get; set; } = new List<OrderStatusHistory>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Lookup StatusIdFkNavigation { get; set; } = null!;
}
