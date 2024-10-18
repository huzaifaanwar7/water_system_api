using System;
using PrescottAppBackend.Domain.DbModels;

namespace PrescottAppBackend.Domain
{

    public interface IAnnouncementService
    {
        Task<List<AnnouncementVM>> GetAllAnnouncementsAsync();
        Task<AnnouncementVM> GetAnnouncementByIdAsync(int announcementId);
        Task<AnnouncementVM> AddUpdateAnnouncementAsync(AnnouncementVM announcement);
        Task DeleteAnnouncementAsync(int announcementId);
    }
}