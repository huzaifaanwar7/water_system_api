namespace GBS.Data.Model
{
    public class OrderItemVM
    {
        public int Id { get; set; }
        public int? OrderIdFk { get; set; }
        public int? ProductIdFk { get; set; }
        public decimal? Quantity { get; set; }
        public int? SizeIdFk { get; set; }
        public string? Color { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? TotalPrice { get; set; }
        public string? SpecialInstructions { get; set; }
        public bool? IsCompleted { get; set; }
        public decimal? CompletedQuantity { get; set; }
    }
}
