using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrescottAppBackend.Domain.DbModels;
using PrescottAppBackend.Domain;

namespace PrescottAppBackend.Domain
{
    public interface IReservationService
    {
        Task<List<ReservationVM>> GetAllReservationsAsync();
        Task<Reservation> GetReservationByIdAsync(int reservationId);
        // Task<Reservation> AddUpdateReservationAsync(ReservationVM reservation);
        // Task DeleteReservationAsync(int reservationId);
    }
}