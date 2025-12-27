namespace GBS.Api.Model.Post
{
    public partial class OrderPM
    {
        public int Id { get; set; }
        public int ClientId { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime DeliveryDate { get; set; }

        public int StatusId { get; set; }

        public int TotalQuantity { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal AdvanceAmount { get; set; }

        public decimal? BalanceAmount { get; set; }

        public string? Notes { get; set; }

  
    }

}
