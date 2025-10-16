using System;
using System.Collections.Generic;

namespace GBS.Entities.DbModels;

public partial class Material
{
    public int Id { get; set; }

    public string MaterialCode { get; set; } = null!;

    public string MaterialName { get; set; } = null!;

    public int? MaterialTypeIdFk { get; set; }

    public int? UnitIdFk { get; set; }

    public decimal? CurrentStock { get; set; }

    public decimal? MinStockLevel { get; set; }

    public decimal? LastPurchasePrice { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public int? CreatedBy { get; set; }

    public int? ModifiedBy { get; set; }

    public virtual ICollection<MaterialPurchase> MaterialPurchases { get; set; } = new List<MaterialPurchase>();

    public virtual Lookup? MaterialTypeIdFkNavigation { get; set; }

    public virtual ICollection<OrderMaterial> OrderMaterials { get; set; } = new List<OrderMaterial>();

    public virtual Lookup? UnitIdFkNavigation { get; set; }
}
