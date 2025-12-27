using System;
using System.Collections.Generic;

namespace GBS.Api.DbModels;

public partial class Product
{
    public int Id { get; set; }

    public string ProductCode { get; set; } = null!;

    public string ProductName { get; set; } = null!;

    public int? CategoryIdFk { get; set; }

    public string? Description { get; set; }

    public decimal? BaseStitchingCost { get; set; }

    public int? EstimatedTimeMinutes { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public int? CreatedBy { get; set; }

    public int? ModifiedBy { get; set; }

    public virtual Lookup? CategoryIdFkNavigation { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
