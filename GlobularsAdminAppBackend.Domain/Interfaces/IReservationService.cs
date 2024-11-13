using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlobularsAdminAppBackend.Domain.DbModels;
using GlobularsAdminAppBackend.Domain;

namespace GlobularsAdminAppBackend.Domain
{
    public interface IReservationService
    {
        Task<List<ReservationVM>> GetAllReservationsAsync();
        Task<ReservationVM> GetReservationByIdAsync(int reservationId);
        Task<string> AddUpdateReservationAsync(ReservationVM reservation);
        Task DeleteReservationAsync(int reservationId);
        Task<List<ReservationVM>> GetUserReservationById(string userId);
    }
}