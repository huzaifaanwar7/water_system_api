using System;
using System.Collections.Generic;

namespace GBS.Api.Model
{
    public class OrderVM
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public int ClientIdFk { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public int StatusIdFk { get; set; }
        public string Status { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal AdvanceAmount { get; set; }
        public decimal BalanceAmount { get; set; }
        public string Notes { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
    }
}