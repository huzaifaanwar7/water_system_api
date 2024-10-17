
using Microsoft.EntityFrameworkCore;
using PrescottAppBackend.Domain;
using PrescottAppBackend.Domain.DbModels;

namespace PrescottAppBackend.Infrastructure
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly PrescottContext _dbContext;
        // private readonly IIOHelper _iOHelper;

        public AnnouncementService(PrescottContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            // _iOHelper = iOHelper ?? throw new ArgumentNullException(nameof(iOHelper));
        }

        public async Task<List<AnnouncementVM>> GetAllAnnouncementsAsync()
        {
            // return await _dbContext.Announcements.ToListAsync();

            var result = await (from a in _dbContext.Announcements
                                join b in _dbContext.Buildings on a.BuildingId equals b.Id
                                join u in _dbContext.Users on a.CreatedBy equals u.Id
                                select new AnnouncementVM()
                                {
                                    Id = a.Id,
                                    Title = a.Title,
                                    BuildingId = a.BuildingId,
                                    Content = a.Content,
                                    FileName = a.FileName,
                                    // File = a.File,
                                    FileType = a.FileType,
                                    FilePath = a.FilePath,
                                    CreatedBy = a.CreatedBy,
                                    CreatedAt = a.CreatedAt,
                                    UpdatedBy = a.UpdatedBy,
                                    UpdatedAt = a.UpdatedAt,
                                    BuildingName = b.BuildingName,
                                    CreatedByStr = (u.FirstName + " " + u.LastName).Trim(),
                                }).ToListAsync();

            return result;
        }
        public async Task<Announcement> GetAnnouncementByIdAsync(int announcementId)
        {
            return await _dbContext.Announcements.FindAsync(announcementId);
        }

        public async Task<Announcement> AddUpdateAnnouncementAsync(AnnouncementVM announcementVM)
        {
            Announcement announcement;
            if (announcementVM.Id == 0)
            {
                announcement = new Announcement()
                {
                    BuildingId = announcementVM.BuildingId,
                    Title = announcementVM.Title,
                    Content = announcementVM.Content,
                    FileName = announcementVM.FileName,
                    //File = announcementVM.File,
                    FileType = announcementVM.FileType,
                    CreatedBy = announcementVM.CreatedBy,
                    CreatedAt = DateTime.Now,
                    IsDeleted = false
                };

                string filePath = IOHelper.SaveFile(announcementVM.File, announcementVM.FileName);
                announcement.FilePath = filePath;

                await _dbContext.Announcements.AddAsync(announcement);
            }
            else
            {
                announcement = _dbContext.Announcements.FirstOrDefault(announcement => announcement.Id == announcementVM.Id);
                if (announcement != null)
                {
                    announcement.BuildingId = announcementVM.BuildingId;
                    announcement.Title = announcementVM.Title;
                    announcement.Content = announcementVM.Content;
                    announcement.FileName = announcementVM.FileName;
                    //announcement.File = announcementVM.File;
                    announcement.FileType = announcementVM.FileType;
                    announcement.UpdatedBy = announcementVM.UpdatedBy;
                    announcement.UpdatedAt = DateTime.Now;

                    string filePath = IOHelper.SaveFile(announcementVM.File, announcementVM.FileName);
                    announcement.FilePath = filePath;

                    _dbContext.Announcements.Update(announcement);
                }
            }

            await _dbContext.SaveChangesAsync();
            return announcement;
        }

        public async Task DeleteAnnouncementAsync(int announcementId)
        {
            var announcement = await _dbContext.Announcements.FindAsync(announcementId);
            if (announcement != null)
            {
                _dbContext.Announcements.Remove(announcement);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
