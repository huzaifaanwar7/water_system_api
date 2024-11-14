using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlobularsAdmin.Domain.DbModels;
using GlobularsAdmin.Domain;

namespace GlobularsAdmin.Domain
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