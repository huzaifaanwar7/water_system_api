using GBS.Entities.DbModels;

namespace GBS.Data.Model
{
    public class LookupsVM
    {
        public List<LookupVM> EmployeeStatus { get; set; } = new();
        public List<LookupVM> UserRole { get; set; } = new();
        public List<LookupVM> JobRole { get; set; } = new();
        public List<LookupVM> TechStack { get; set; } = new();
        public List<LookupVM> TransactionType { get; set; } = new();
        public List<LookupVM> LedgerStatus { get; set; } = new();
        public List<LookupVM> CostCategory { get; set; } = new();
        public List<LookupVM> MaterialType { get; set; } = new();
        public List<LookupVM> MaterialUnit { get; set; } = new();
        public List<LookupVM> OrderStatus { get; set; } = new();
        public List<LookupVM> PaymentMethod { get; set; } = new();
        public List<LookupVM> ProductCategory { get; set; } = new();
        public List<LookupVM> ProductSize { get; set; } = new();
        public List<LookupVM> SalaryChangeType { get; set; } = new();
    }
    public partial class LookupVM
    {
        public int Id { get; set; }

        public string Type { get; set; } = null!;

        public string Name { get; set; } = null!;

        public bool IsActive { get; set; }

        public int? SortOrder { get; set; }

        public int? ParentIdFk { get; set; }
    }
}
