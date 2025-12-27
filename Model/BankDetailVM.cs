using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GBS.Data.Model
{
    public class BankDetailVM
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public string? BankName { get; set; }

        public string? AccountTitle { get; set; }

        public string? BranchCode { get; set; }

        public string? AccountNumber { get; set; }

        public string? Iban { get; set; }

    }
}
