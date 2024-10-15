using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrescottAppBackend.Domain;
using PrescottAppBackend.Domain.DbModels;


namespace PrescottAppBackend.Infrastructure
{
    public class ReservationService : IReservationService
    {
        private readonly PrescottContext _dbContext;

        public ReservationService(PrescottContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }


        // public async Task<List<ReservationVM>> GetAllReservationsAsync() {

        // }
        // public async Task<Reservation> GetReservationByIdAsync(int reservationId) {

        // }
        // public async Task<Reservation> AddUpdateReservationAsync(ReservationVM reservation) {

        // }
        // public async Task DeleteReservationAsync(int reservationId) {

        // }
    }
}