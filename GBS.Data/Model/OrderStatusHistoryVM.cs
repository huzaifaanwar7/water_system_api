namespace GBS.Data.Model
{
    public class OrderStatusHistoryVM
    {
        public int Id { get; set; }
        public int? OrderIdFk { get; set; }
        public int? StatusIdFk { get; set; }
        public DateTime? StatusDate { get; set; }
        public int? ChangedBy { get; set; }
        public string? Notes { get; set; }
    }
}
