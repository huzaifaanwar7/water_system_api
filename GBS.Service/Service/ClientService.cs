
using Microsoft.EntityFrameworkCore;
using GBS.Entities;
using GBS.Entities.DbModels;
using GBS.Data.Model;

namespace GBS.Service
{
    public interface IClientService
    {
        Task<List<Client>> GetClientList();
        Task<Client> GetClientById(int Id);
        Task<int> SaveClient(Client user);
      
        Task<int> DeleteClient(string Id);

        Task<int> UpdateClient(Client client);
     

    }
    public class ClientService(GBS_DbContext _dbContext) : IClientService
    {


        public async Task<List<Client>> GetClientList()
        {
            var clients = await _dbContext.Clients.Where(e => e.IsActive).ToListAsync();
            return clients;
        }

        public async Task<Client> GetClientById(int Id)
        {
            return await _dbContext.Clients.Where(u => u.Id == Id).
                Include(a =>a.Orders)
                .ThenInclude(x=> x.StatusIdFkNavigation)
                .FirstOrDefaultAsync();
        }

        public async Task<int> SaveClient(Client user)
        {
            if (user.Id == 0) { await _dbContext.Clients.AddAsync(user); }
            else { _dbContext.Clients.Update(user); }
            return await _dbContext.SaveChangesAsync();
        }


        public async Task<int> DeleteClient(string Id)
        {
            var user = await _dbContext.Clients.FindAsync(Id);
            if (user != null)
            {
                user.IsActive = false;
                _dbContext.Clients.Update(user);

                return await _dbContext.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<int> UpdateClient(Client client)
        {
            _dbContext.Clients.Update(client);
            return await _dbContext.SaveChangesAsync();
        }


    }
}
