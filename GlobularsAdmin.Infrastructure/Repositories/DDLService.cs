
using Microsoft.EntityFrameworkCore;
using GlobularsAdmin.Domain;
using GlobularsAdmin.Domain.DbModels;

namespace GlobularsAdmin.Infrastructure
{
    public class DDLService : IDDLService
    {
        private readonly PrescottContext _dbContext;

        public DDLService(PrescottContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<List<Dropdownlistchild>> GetDropdownListByTypeAsync(string ddlType)
        {
            var parentId = await _dbContext.Dropdownlistparents.Where(ddl => ddl.Type == ddlType).Select(ddl => ddl.Id).FirstOrDefaultAsync();
            var childDDL = await _dbContext.Dropdownlistchildren.Where(ddl => ddl.ParentId == parentId).ToListAsync();
            return childDDL;
        }

    }
}
