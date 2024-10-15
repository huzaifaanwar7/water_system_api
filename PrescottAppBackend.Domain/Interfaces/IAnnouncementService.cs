using System;
using PrescottAppBackend.Domain.DbModels;

namespace PrescottAppBackend.Domain
{

    public interface IAnnouncementService
    {
        Task<List<AnnouncementVM>> GetAllAnnouncementsAsync();
        Task<Announcement> GetAnnouncementByIdAsync(int announcementId);
        Task<Announcement> AddUpdateAnnouncementAsync(AnnouncementVM announcement);
        Task DeleteAnnouncementAsync(int announcementId);
    }
}