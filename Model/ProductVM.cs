using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GBS.Model
{
public class ProductVM
{
    public int Id { get; set; }
    public string Reference { get; set; } = null!;
    public string ProductName { get; set; } = null!;
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