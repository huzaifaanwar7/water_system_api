namespace GBS.Api.Model.Post
{
    public class OrderItemPM
    {
        public int Id { get; set; }             
        public int OrderId { get; set; }
        public int ProductIdFk { get; set; }
        public int SizeIdFk { get; set; }
        public string? Color { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string? SpecialInstructions { get; set; }
    }
}
