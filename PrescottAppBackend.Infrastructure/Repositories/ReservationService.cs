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
                                            Id = res.Id,
                                            BuildingId = res.BuildingId,
                                            AmenityId = res.AmenityId,
                                            Reason = res.Reason,
                                            FromDate = res.FromDate,
                                            ToDate = res.ToDate,
                                            CreatedBy = res.CreatedBy,
                                            CreatedAt = res.CreatedAt,
                                            UpdatedBy = res.UpdatedBy,
                                            UpdatedAt = res.UpdatedAt,
                                            UserVM = u
                                        }).ToListAsync();
            return reservationVMs;
        }
        public async Task<ReservationVM> GetReservationByIdAsync(int reservationId)
        {
            var reservation = await _dbContext.Reservations.Where(reservation => reservation.Id == reservationId).FirstOrDefaultAsync();
            return CustomMapper.Map<Reservation, ReservationVM>(reservation);
        }
        public async Task<string> AddUpdateReservationAsync(ReservationVM reservationVM)
        {
            if (reservationVM.Id == 0)
            {
                var isExists = await _dbContext.Reservations.AnyAsync(r => r.BuildingId == reservationVM.BuildingId && r.AmenityId == reservationVM.AmenityId && (r.FromDate <= reservationVM.ToDate && r.ToDate >= reservationVM.FromDate));
                if (!isExists)
                {
                    var reservation = new Reservation()
                    {
                        BuildingId = reservationVM.BuildingId,
                        AmenityId = reservationVM.AmenityId,
                        Reason = reservationVM.Reason,
                        FromDate = reservationVM.FromDate,
                        ToDate = reservationVM.ToDate,
                        CreatedBy = reservationVM.CreatedBy,
                        CreatedAt = DateTime.Now,
                        IsDeleted = false
                    };

                    await _dbContext.Reservations.AddAsync(reservation);
                    await _dbContext.SaveChangesAsync();

                    return "Created";
                }
                else
                {
                    return "Already Exists";
                }
            }
            else
            {
                var isExists = await _dbContext.Reservations.AnyAsync(r => r.Id != reservationVM.Id && r.BuildingId == reservationVM.BuildingId && r.AmenityId == reservationVM.AmenityId && (r.FromDate <= reservationVM.ToDate && r.ToDate >= reservationVM.FromDate));
                if (!isExists)
                {
                    var reservation = await _dbContext.Reservations.FirstOrDefaultAsync(r => r.Id == reservationVM.Id);
                    if (reservation != null)
                    {
                        reservation.BuildingId = reservationVM.BuildingId;
                        reservation.AmenityId = reservationVM.AmenityId;
                        reservation.Reason = reservationVM.Reason;
                        reservation.FromDate = reservationVM.FromDate;
                        reservation.ToDate = reservationVM.ToDate;
                        reservation.UpdatedBy = reservationVM.UpdatedBy;
                        reservation.UpdatedAt = DateTime.Now;
                        reservation.IsDeleted = false;

                        _dbContext.Reservations.Update(reservation);
                        await _dbContext.SaveChangesAsync();

                        return "Updated";
                    }
                    else
                    {
                        return "Not Found";
                    }
                }
                else
                {
                    return "Already Exists";
                }
            }
        }
        public async Task DeleteReservationAsync(int reservationId)
        {
            var reservation = await _dbContext.Reservations.FindAsync(reservationId);
            if (reservation != null)
            {
                _dbContext.Reservations.Remove(reservation);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}