
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
            var result = await (from a in _dbContext.Announcements
                                join b in _dbContext.Buildings on a.BuildingId equals b.Id
                                join u in _dbContext.Users on a.CreatedBy equals u.Id
                                select new AnnouncementVM()
                                {
                                    Id = a.Id,
                                    Title = a.Title,
                                    BuildingId = a.BuildingId,
                                    Content = a.Content,
                                    CreatedBy = a.CreatedBy,
                                    CreatedAt = a.CreatedAt,
                                    UpdatedBy = a.UpdatedBy,
                                    UpdatedAt = a.UpdatedAt,
                                    BuildingName = b.BuildingName,
                                    CreatedByStr = (u.FirstName + " " + u.LastName).Trim(),
                                    UserVM = u,

                                }).ToListAsync();
            var announcementImages = await _dbContext.AnnouncementImages.Where(x => result.Select(a => a.Id).Contains(x.AnnouncementId)).ToListAsync();
            foreach (var r in result)
            {
                var images = announcementImages.Where(x => x.AnnouncementId == r.Id).ToList();
                r.AnnouncementImages = CustomMapper.MapList<AnnouncementImage, AnnouncementImageVM>(images);
            }

            return result;
        }
        public async Task<AnnouncementVM> GetAnnouncementByIdAsync(int announcementId)
        {
            var result = await (from a in _dbContext.Announcements
                                join b in _dbContext.Buildings on a.BuildingId equals b.Id
                                join u in _dbContext.Users on a.CreatedBy equals u.Id
                                where a.Id == announcementId
                                select new AnnouncementVM()
                                {
                                    Id = a.Id,
                                    Title = a.Title,
                                    BuildingId = a.BuildingId,
                                    Content = a.Content,
                                    CreatedBy = a.CreatedBy,
                                    CreatedAt = a.CreatedAt,
                                    UpdatedBy = a.UpdatedBy,
                                    UpdatedAt = a.UpdatedAt,
                                    BuildingName = b.BuildingName,
                                    CreatedByStr = (u.FirstName + " " + u.LastName).Trim(),
                                    UserVM = u

                                }).FirstOrDefaultAsync();

            var announcementImages = await _dbContext.AnnouncementImages.Where(x => x.AnnouncementId == announcementId).ToListAsync();
            result.AnnouncementImages = CustomMapper.MapList<AnnouncementImage, AnnouncementImageVM>(announcementImages);
            return result;
        }

        public async Task<AnnouncementVM> AddUpdateAnnouncementAsync(AnnouncementVM announcementVM)
        {
            Announcement announcement;
            AnnouncementVM vM;
            if (announcementVM.Id == 0)
            {
                announcement = new Announcement()
                {
                    BuildingId = announcementVM.BuildingId,
                    Title = announcementVM.Title,
                    Content = announcementVM.Content,
                    CreatedBy = announcementVM.CreatedBy,
                    CreatedAt = DateTime.Now,
                    IsDeleted = false
                };

                await _dbContext.Announcements.AddAsync(announcement);
                await _dbContext.SaveChangesAsync();

                vM = CustomMapper.Map<Announcement, AnnouncementVM>(announcement);

                if (announcementVM.AnnouncementImages != null && announcementVM.AnnouncementImages.Count > 0)
                {
                    foreach (var images in announcementVM.AnnouncementImages)
                    {
                        string filePath = IOHelper.SaveFile(images.File, images.FileName);
                        var announcementImage = new AnnouncementImage()
                        {
                            FilePath = filePath,
                            FileName = images.FileName,
                            FileType = images.FileType,
                            AnnouncementId = announcement.Id,
                        };
                        await _dbContext.AnnouncementImages.AddAsync(announcementImage);
                        await _dbContext.SaveChangesAsync();

                        var imageVM = CustomMapper.Map<AnnouncementImage, AnnouncementImageVM>(announcementImage);
                        vM.AnnouncementImages.Add(imageVM);
                    }
                }

            }
            else
            {
                vM = new AnnouncementVM();
                announcement = _dbContext.Announcements.FirstOrDefault(announcement => announcement.Id == announcementVM.Id);
                if (announcement != null)
                {
                    announcement.BuildingId = announcementVM.BuildingId;
                    announcement.Title = announcementVM.Title;
                    announcement.Content = announcementVM.Content;
                    announcement.UpdatedBy = announcementVM.UpdatedBy;
                    announcement.UpdatedAt = DateTime.Now;

                    var imagesToDelete = await _dbContext.AnnouncementImages.Where(a => a.AnnouncementId == announcement.Id && !announcementVM.AnnouncementImages.Select(img => img.Id).Contains(a.Id)).ToListAsync();

                    if (imagesToDelete.Any())
                    {
                        _dbContext.AnnouncementImages.RemoveRange(imagesToDelete);
                        await _dbContext.SaveChangesAsync();
                    }
                    vM = CustomMapper.Map<Announcement, AnnouncementVM>(announcement);

                    if (announcementVM.AnnouncementImages != null && announcementVM.AnnouncementImages.Count > 0)
                    {
                        foreach (var images in announcementVM.AnnouncementImages)
                        {
                            string filePath = IOHelper.SaveFile(images.File, images.FileName);
                            var announcementImage = new AnnouncementImage()
                            {
                                FilePath = filePath,
                                FileName = images.FileName,
                                FileType = images.FileType,
                                AnnouncementId = announcement.Id,
                            };
                            await _dbContext.AnnouncementImages.AddAsync(announcementImage);
                            await _dbContext.SaveChangesAsync();

                            var imageVM = CustomMapper.Map<AnnouncementImage, AnnouncementImageVM>(announcementImage);
                            vM.AnnouncementImages.Add(imageVM);
                        }
                    }
                    _dbContext.Announcements.Update(announcement);
                    await _dbContext.SaveChangesAsync();
                }
            }

            return vM;
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
