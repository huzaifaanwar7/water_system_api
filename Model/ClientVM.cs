using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GBS.Model
{
    public class ClientVM
    {
        public int Id { get; set; }
        
        public string Reference { get; set; }

        public string ClientName { get; set; } = null!;

        public string? ContactPerson { get; set; }

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public string? Address { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? PostalCode { get; set; }

        public string? Gstnumber { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public List<OrderVM> Orders { get; set; }
        public List<PaymentVM> Payments { get; set; }

        public List<ProductVM> Products { get; set; }

    }
}
