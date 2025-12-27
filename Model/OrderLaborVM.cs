namespace GBS.Model
{
    public class OrderLaborVM
    {
        public int Id { get; set; }
        public int? OrderIdFk { get; set; }
        public int? OrderItemIdFk { get; set; }
        public int? EmployeeIdFk { get; set; }
        public DateTime? WorkDate { get; set; }
        public decimal? QuantityCompleted { get; set; }
        public decimal? HoursWorked { get; set; }
        public decimal? RatePerPiece { get; set; }
        public decimal? TotalLaborCost { get; set; }
        public string? Notes { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
