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
        public List<Status> Status { get; set; } = new();
        public List<UserRole> UserRole { get; set; } = new();
        public List<JobRole> JobRole { get; set; } = new();
        public List<TechStack> TechStack { get; set; } = new();
    }
}
