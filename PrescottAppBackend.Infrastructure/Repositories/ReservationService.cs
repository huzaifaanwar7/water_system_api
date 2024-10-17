using Microsoft.EntityFrameworkCore;
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


        public async Task<List<ReservationVM>> GetAllReservationsAsync()
        {
            var reservationVMs = await (from res in _dbContext.Reservations
                                        join a in _dbContext.Amenities on res.AmenityId equals a.Id
                                        join b in _dbContext.Buildings on res.BuildingId equals b.Id
                                        join u in _dbContext.Users on res.CreatedBy equals u.Id
                                        select new ReservationVM()
                                        {
                                            Amenities = a,
                                            Building = b,
                                            BuildingId = res.BuildingId,
                                            AmenityId = res.AmenityId,
                                            Reason = res.Reason,
                                            FromDate = res.FromDate,
                                            ToDate = res.ToDate,
                                            CreatedBy = res.CreatedBy,
                                            CreatedAt = res.CreatedAt,
                                            UpdatedBy = res.UpdatedBy,
                                            UpdatedAt = res.UpdatedAt,
                                        }).ToListAsync();
            return reservationVMs;
        }
        public async Task<Reservation> GetReservationByIdAsync(int reservationId)
        {
            var reservation = await _dbContext.Reservations.Where(reservation => reservation.Id == reservationId).FirstOrDefaultAsync();
            return reservation;
        }
        // public async Task<Reservation> AddUpdateReservationAsync(ReservationVM reservation)
        // {

        // }
        // public async Task DeleteReservationAsync(int reservationId)
        // {

        // }
    }
}