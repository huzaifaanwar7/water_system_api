using System;
using GlobularsAdminAppBackend.Domain.DbModels;

namespace GlobularsAdminAppBackend.Domain
{

    public interface IAnnouncementService
    {
        Task<List<AnnouncementVM>> GetAllAnnouncementsAsync();
        Task<AnnouncementVM> GetAnnouncementByIdAsync(int announcementId);
        Task<AnnouncementVM> AddUpdateAnnouncementAsync(AnnouncementVM announcement);
        Task DeleteAnnouncementAsync(int announcementId);
    }
}