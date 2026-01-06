using Microsoft.EntityFrameworkCore;
using GBS.Api.DbModels;
using GBS.Model;

namespace GBS.Service
{
    public interface IProductService
    {
        Task<List<ProductVM>> GetProductList();
        Task<ProductVM?> GetProductById(int id);
        
        Task<int>AddProduct(ProductVM model);
        Task<bool>UpdateProduct(int Id,ProductVM model);
        Task<bool>DeleteProduct(int Id);
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
                    Reference = p.Reference,
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
                .Where(p => p.Id == id && p.IsActive == true)
                .Select(p => new ProductVM
                {
                    Id = p.Id,
                    Reference = p.Reference,
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
        public async Task<int>AddProduct(ProductVM model)
        {
                var product = new Product
    {
        Reference = model.Reference,
        ProductName = model.ProductName,
        CategoryIdFk = model.CategoryIdFk ?? 0,
        Description = model.Description,
        BaseStitchingCost = model.BaseStitchingCost ?? 0,
        EstimatedTimeMinutes = model.EstimatedTimeMinutes,
        IsActive = model.IsActive ?? true,
        CreatedDate = DateTime.Now,
        CreatedBy = model.CreatedBy ?? 0
    };

    _dbContext.Products.Add(product);
    await _dbContext.SaveChangesAsync();
    return product.Id;
        }
        public async Task<bool>UpdateProduct(int Id,ProductVM model)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id ==Id && p.IsActive == true);
            if (product == null)
            return false;
                product.Reference = model.Reference;
    product.ProductName = model.ProductName;
    product.CategoryIdFk = model.CategoryIdFk ?? product.CategoryIdFk;
    product.Description = model.Description;
    product.BaseStitchingCost = model.BaseStitchingCost ?? product.BaseStitchingCost;
    product.EstimatedTimeMinutes = model.EstimatedTimeMinutes;
    product.IsActive = model.IsActive;
    product.ModifiedDate = DateTime.Now;
    product.ModifiedBy = model.ModifiedBy;

    await _dbContext.SaveChangesAsync();
    return true;
        }
        public async Task<bool>DeleteProduct(int Id)
        {
                var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == Id && p.IsActive == true);
    if (product == null)
        return false;
    product.IsActive = false;   
    product.ModifiedDate = DateTime.Now;
    _dbContext.Products.Update(product);
    await _dbContext.SaveChangesAsync();
    return true;
        }
    }
}
