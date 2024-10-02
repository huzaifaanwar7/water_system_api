
using Microsoft.EntityFrameworkCore;
using PrescottAppBackend.Domain;
using PrescottAppBackend.Domain.DbModels;

namespace PrescottAppBackend.Infrastructure
{
    public class DDLService : IDDLService
    {
        private readonly PrescottContext _dbContext;

        public DDLService(PrescottContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<List<DropdownListChild>> GetDropdownListByTypeAsync(string ddlType)
        {
            var parentId = await _dbContext.DropdownListParents.Where(ddl => ddl.Type == ddlType).Select(ddl => ddl.Id).FirstOrDefaultAsync();
            var childDDL = await _dbContext.DropdownListChildren.Where(ddl => ddl.ParentId == parentId).ToListAsync();
            return childDDL;
        }

    }
}
