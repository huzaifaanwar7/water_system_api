using GBS.Api.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GBS.Data.Model
{
    public partial class OrderVM
    {
        public int Id { get; set; }

        public string OrderNumber { get; set; } = null!;

        public int? ClientIdFk { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public int? StatusIdFk { get; set; }
        public string Status { get; set; }

        public int? TotalQuantity { get; set; }

        public decimal? TotalAmount { get; set; }

        public decimal? AdvanceAmount { get; set; }

        public decimal? BalanceAmount { get; set; }

        public string? Notes { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string? ModifiedBy { get; set; }
        public List<OrderCostVM> OrderCosts { get; set; }
        public List<OrderItemVM> OrderItems { get; set; }   
        public List<OrderLaborVM> OrderLabors { get; set; }
        public List<OrderMaterialVM> OrderMaterials { get; set; }
        public List<OrderStatusHistoryVM> OrderStatusHistories { get; set; }

    }

}
