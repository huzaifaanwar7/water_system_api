using GBS.Api.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GBS.Model
{

    public class EmployeeLedgerVM
    {
        public int Id { get; set; }

        public DateTime TransactionDate { get; set; }

        public int TransactionTypeIdFk { get; set; }
        public string TransactionType { get; set; } 

        public string? Description { get; set; }

        public decimal? DebitAmount { get; set; }

        public decimal? CreditAmount { get; set; }

        public string? SalaryMonth { get; set; }


        public decimal? RunningBalance { get; set; }

        public int StatusIdFk { get; set; }
        public string Status { get; set; }

        public bool IsRecovered { get; set; }

        public DateTime CreatedDate { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedBy { get; set; }

        public string? Remarks { get; set; }

        public bool IsActive { get; set; }

   
    }

}
