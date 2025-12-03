using Microsoft.EntityFrameworkCore;
using GBS.Entities;
using GBS.Entities.DbModels;
using GBS.Data.Model;

namespace GBS.Service
{
    public interface IProductService
    {
        Task<List<ProductVM>> GetProductList();
        Task<ProductVM> GetProductById(int id);
        Task<int> SaveProduct(ProductVM model);
        Task<int> UpdateProduct(ProductVM model);
        Task<int> DeleteProduct(int id);
    }

    public class ProductService(GBS_DbContext _dbContext) : IProductService
    {
        // LIST ALL ACTIVE PRODUCTS
        public async Task<List<ProductVM>> GetProductList()
        {
            return await _dbContext.Products
                .Where(p => p.IsActive)
                .Select(p => new ProductVM
                {
                    Id = p.Id,
                    OrderName = p.OrderName,
                    ClientIdFk = p.ClientIdFk,
                    OrderDate = p.OrderDate,
                    DeliveryDate = p.DeliveryDate,
                    StatusIdFk = p.StatusIdFk,
                    TotalQuantity = p.TotalQuantity,
                    TotalAmount = p.TotalAmount,
                    AdvanceAmount = p.AdvanceAmount,
                    BalanceAmount = p.BalanceAmount,
                    Notes = p.Notes,
                    CreatedBy = p.CreatedBy,
                    CreatedDate = p.CreatedDate,
                    ModifiedBy = p.ModifiedBy,
                    ModifiedDate = p.ModifiedDate
                }).ToListAsync();
        }

        // GET PRODUCT BY ID
        public async Task<ProductVM> GetProductById(int id)
        {
            return await _dbContext.Products
                .Where(p => p.Id == id)
                .Select(p => new ProductVM
                {
                    Id = p.Id,
                    OrderName = p.OrderName,
                    ClientIdFk = p.ClientIdFk,
                    OrderDate = p.OrderDate,
                    DeliveryDate = p.DeliveryDate,
                    StatusIdFk = p.StatusIdFk,
                    TotalQuantity = p.TotalQuantity,
                    TotalAmount = p.TotalAmount,
                    AdvanceAmount = p.AdvanceAmount,
                    BalanceAmount = p.BalanceAmount,
                    Notes = p.Notes,
                    CreatedBy = p.CreatedBy,
                    CreatedDate = p.CreatedDate,
                    ModifiedBy = p.ModifiedBy,
                    ModifiedDate = p.ModifiedDate
                }).FirstOrDefaultAsync();
        }

      
     

    
    }
}
