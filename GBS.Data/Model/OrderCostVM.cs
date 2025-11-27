using System;
namespace GBS.Data.Model
{
    public class OrderCostVM
    {
        public int Id { get; set; }
        public int OrderIdFk { get; set; }
        public int CostCategoryIdFk { get; set; }
        public string CostDescription { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitCost { get; set; }
        public decimal TotalCost { get; set; }
        public DateTime CostDate { get; set; }
        public string VendorName { get; set; }
        public string InvoiceNumber { get; set; }
        public string Notes { get; set; }
    }
}