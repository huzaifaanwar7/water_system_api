using GBS.Entities.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GBS.Data.Model
{
    public class LookupsVM
    {
        public List<Lookup> EmployeeStatus { get; set; } = new();
        public List<Lookup> UserRole { get; set; } = new();
        public List<Lookup> JobRole { get; set; } = new();
        public List<Lookup> TechStack { get; set; } = new();
        public List<Lookup> TransactionType { get; set; } = new();
        public List<Lookup> LedgerStatus { get; set; } = new();
    }
}
