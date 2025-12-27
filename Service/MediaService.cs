
using Microsoft.EntityFrameworkCore;

using GBS.Api.DbModels;
using GBS.Data.Model;

namespace GBS.Service
{
    public interface IMediaService
    {
        Task<int> Save(MediaFile file);
        Task<MediaFile> Get(Guid Id);
        Task<int> Update(MediaFile file);
    }
    public class MediaService(GBS_DbContext _dbContext) : IMediaService
    {

        public async Task<int> Save(MediaFile file)
        {
            await _dbContext.MediaFiles.AddAsync(file);
            return await _dbContext.SaveChangesAsync();
        }
        public async Task<MediaFile> Get(Guid Id)
        {
            return await _dbContext.MediaFiles.Where(m => m.Id == Id).FirstOrDefaultAsync();
        }
        public async Task<int> Update(MediaFile file)
        {
            _dbContext.MediaFiles.Update(file);
            return await _dbContext.SaveChangesAsync();
        }
    }
}
