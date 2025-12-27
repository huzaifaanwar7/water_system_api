using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GBS.Model
{
    public partial class PaymentVM
    {
        public int Id { get; set; }
        public int OrderIdFk { get; set; }

        public DateTime PaymentDate { get; set; }

        public decimal Amount { get; set; }
        public int PaymentMethodIdFk { get; set; }

        public string? Reference { get; set; }

        public string? Notes { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }


    }
}
