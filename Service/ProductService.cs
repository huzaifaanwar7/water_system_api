using Microsoft.EntityFrameworkCore;
using GBS.Api.DbModels;
using GBS.Model;

namespace GBS.Service
{
    public interface IProductService
    {
        Task<List<ProductVM>> GetProductList();
        Task<ProductVM?> GetProductById(int id);
    }

    public class ProductService : IProductService
    {
        private readonly GBS_DbContext _dbContext;

        public ProductService(GBS_DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<ProductVM>> GetProductList()
        {
            return await _dbContext.Products
                .Where(p => p.IsActive == true)
                .Select(p => new ProductVM
                {
                    Id = p.Id,
                    ProductCode = p.ProductCode,
                    ProductName = p.ProductName,
                    CategoryIdFk = p.CategoryIdFk,
                    Description = p.Description,
                    BaseStitchingCost = p.BaseStitchingCost,
                    EstimatedTimeMinutes = p.EstimatedTimeMinutes,
                    IsActive = p.IsActive,
                    CreatedDate = p.CreatedDate,
                    ModifiedDate = p.ModifiedDate,
                    CreatedBy = p.CreatedBy,
                    ModifiedBy = p.ModifiedBy
                })
                .ToListAsync();
        }

        public async Task<ProductVM?> GetProductById(int id)
        {
            return await _dbContext.Products
                .Where(p => p.Id == id)
                .Select(p => new ProductVM
                {
                    Id = p.Id,
                    ProductCode = p.ProductCode,
                    ProductName = p.ProductName,
                    CategoryIdFk = p.CategoryIdFk,
                    Description = p.Description,
                    BaseStitchingCost = p.BaseStitchingCost,
                    EstimatedTimeMinutes = p.EstimatedTimeMinutes,
                    IsActive = p.IsActive,
                    CreatedDate = p.CreatedDate,
                    ModifiedDate = p.ModifiedDate,
                    CreatedBy = p.CreatedBy,
                    ModifiedBy = p.ModifiedBy
                })
                .FirstOrDefaultAsync();
        }
    }
}
