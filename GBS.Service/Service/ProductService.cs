using Microsoft.EntityFrameworkCore;
using GBS.Entities.DbModels;
using GBS.Data.Model;

namespace GBS.Service
{
    public interface IProductService
    {
        Task<List<Product>> GetProductList();
        Task<Product> GetProductById(int id);
      
    }

    public class ProductService : IProductService
    {
        private readonly GBS_DbContext _dbContext;

        public ProductService(GBS_DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Product>> GetProductList()
        {
            return await _dbContext.Products
                .Where(p => p.IsActive == true)
                .Include(p => p.CategoryIdFkNavigation)
                .ToListAsync();
        }

        public async Task<Product> GetProductById(int id)
        {
            return await _dbContext.Products
                .Include(p => p.CategoryIdFkNavigation)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

 
    }
}
