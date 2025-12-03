namespace GBS.Data.Model
{
    public class ProductVM
    {
        public int? Id { get; set; }
        public string? ProductCode { get; set; }
        public string? ProductName { get; set; }
        public int? CategoryIdFk { get; set; }
        public string? Description { get; set; }
        public decimal? BaseStitchingCost { get; set; }
        public int? EstimatedTimeMinutes { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
