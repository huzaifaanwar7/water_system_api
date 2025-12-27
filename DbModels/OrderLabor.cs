using System;
using System.Collections.Generic;

namespace GBS.Api.DbModels;

public partial class OrderLabor
{
    public int Id { get; set; }

    public int? OrderIdFk { get; set; }

    public int? OrderItemIdFk { get; set; }

    public int? EmployeeIdFk { get; set; }

    public DateTime? WorkDate { get; set; }

    public int? QuantityCompleted { get; set; }

    public decimal? HoursWorked { get; set; }

    public decimal? RatePerPiece { get; set; }

    public decimal? TotalLaborCost { get; set; }

    public string? Notes { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual Order? OrderIdFkNavigation { get; set; }

    public virtual OrderItem? OrderItemIdFkNavigation { get; set; }
}
