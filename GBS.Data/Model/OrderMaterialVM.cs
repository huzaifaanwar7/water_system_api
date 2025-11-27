namespace GBS.Data.Model
{
    public class OrderMaterialVM
    {
        public int Id { get; set; }
        public int? OrderIdFk { get; set; }
        public int? MaterialIdFk { get; set; }
        public decimal? QuantityUsed { get; set; }
        public decimal? UnitCost { get; set; }
        public decimal? TotalCost { get; set; }
        public DateTime? UsageDate { get; set; }
        public string? Notes { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
