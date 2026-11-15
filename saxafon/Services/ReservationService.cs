using saxafon.Data;
using saxafon.DTO;
using Microsoft.EntityFrameworkCore;
using saxafon.Models;


namespace saxafon.Services
{

    public interface IReservationService {
         Task<IEnumerable<ReservationFullDTO>> GetAllReservations();
         Task<ReservationFullDTO> GetReservationById(int id);
         Task<IEnumerable<ReservationFullDTO>> GetReservationByResourceId(int resourceId);
         Task<IEnumerable<ReservationFullDTO>> GetAllReservationByUserId(int UserID);
        Task<IEnumerable<ReservationFullDTO>> GetCurrentReservationByUserId(int UserId);
        Task<ReservationCancelResponseDTO> CancelReservation(ReservationCancelDTO reservation);
        Task<IEnumerable<ReservationViewDTO>> ShowCurrentWeekReservations(int resoruceId);

        Task<ReservationFullDTO> CreateReservation(ReservationCreateDTO reservation);

    }


    public class ReservationService : IReservationService
    {

        private readonly AppDbContext context;
        public ReservationService(AppDbContext _context)
        {
            context = _context;
        }
        public async Task<IEnumerable<ReservationFullDTO>> GetAllReservations()
        {
            return await context.Reservations.Select(r => new ReservationFullDTO(
                r.Id,
                r.ResourceId,
                r.UserId,
                r.purpose,
                r.status_rezervare,
                r.start_time,
                r.end_time,
                r.created_at
            )).ToListAsync();
        }

        public async Task<ReservationFullDTO> GetReservationById(int id ) { 
            var result = context.Reservations.Where(r => r.Id == id).Select(r => new ReservationFullDTO(
                r.Id,
                r.ResourceId,
                r.UserId,
                r.purpose,
                r.status_rezervare,
                r.start_time,
                r.end_time,
                r.created_at
            ));
            return await result.FirstOrDefaultAsync();

        }
        public async Task<IEnumerable<ReservationFullDTO>> GetReservationByResourceId(int resourceId) { 
            var result = context.Reservations.Where(r => r.ResourceId == resourceId).Select(r => new ReservationFullDTO(
                r.Id,
                r.ResourceId,
                r.UserId,
                r.purpose,
                r.status_rezervare,
                r.start_time,
                r.end_time,
                r.created_at
            ));
            return await result.ToListAsync();
        }

        public async Task<IEnumerable<ReservationFullDTO>> GetAllReservationByUserId(int UserID)
        {
            var reservation = context.Reservations.Where(r => r.UserId == UserID).Select(e => new ReservationFullDTO(
                e.Id,
                e.ResourceId,
                e.UserId,
                e.purpose,
                e.status_rezervare,
                e.start_time,
                e.end_time,
                e.created_at
            ));

            return await reservation.ToListAsync();
        }

        public async Task<IEnumerable<ReservationFullDTO>> GetCurrentReservationByUserId(int UserId)
        {
            var reservations = context.Reservations.Where(r => r.end_time > DateTime.UtcNow && r.UserId == UserId).Select(e => new ReservationFullDTO(
                e.Id,
                e.ResourceId,
                e.UserId,
                e.purpose,
                e.status_rezervare,
                e.start_time,
                e.end_time,
                e.created_at
            ));

            return await reservations.ToListAsync();
        }

        public async Task<ReservationCancelResponseDTO> CancelReservation(ReservationCancelDTO reservation) {

           
            var query = await context.Reservations.Where(r => r.Id == reservation.id && r.UserId == reservation.userId).FirstOrDefaultAsync();

            if (query == null)
            {
                throw new KeyNotFoundException("Reservation not found.");
            }

            query.status_rezervare = ReservationStatus.CANCELED;
            query.canceled_at = DateTime.UtcNow;
            await context.SaveChangesAsync();

            return new ReservationCancelResponseDTO(
                query.Id,
                query.canceled_at ?? DateTime.UtcNow
            );
        }


        public async Task<IEnumerable<ReservationViewDTO>> ShowCurrentWeekReservations(int resoruceId) {
            int days = (int)(DateTime.Today.DayOfWeek + 6) % 7;
            DateTime StartofWeek = DateTime.Now.Subtract(TimeSpan.FromDays(days)).Date;
            DateTime EndnofWeek = DateTime.Now.AddDays((days % 6) + 1 );

            var query = context.Reservations.Where(r => r.ResourceId == resoruceId && r.start_time >= StartofWeek && r.end_time <= EndnofWeek).Select(e => new ReservationViewDTO(
                e.Id,
                e.ResourceId,
                e.start_time,
                e.end_time
            ));

            return await query.ToListAsync();
        }

        public async Task<ReservationFullDTO> CreateReservation(ReservationCreateDTO reservation)
        {
            if (reservation.end_time <= reservation.start_time) { 
                throw new ArgumentException("End time must be greater than start time.");
            }
            var isOverlap = await  context.Reservations.AnyAsync(r => r.ResourceId == reservation.resourceId  && r.start_time < reservation.end_time && r.end_time > reservation.start_time && r.status_rezervare != ReservationStatus.CANCELED);

            if (isOverlap) { 
                throw new InvalidOperationException("The reservation time overlaps with an existing reservation.");
            }

            var query = context.Reservations.Add(new Reservations
            {

                Id = 0,
                ResourceId = reservation.resourceId,
                UserId = reservation.userId,
                purpose = reservation.prupose,
                status_rezervare = ReservationStatus.PENDING,
                start_time = reservation.start_time,
                end_time = reservation.end_time,
                created_at = DateTime.UtcNow,
                canceled_at = null
            });
            await context.SaveChangesAsync();

            return new ReservationFullDTO(
                 query.Entity.Id,
                 query.Entity.ResourceId,
                 query.Entity.UserId,
                 query.Entity.purpose,
                 query.Entity.status_rezervare,
                 query.Entity.start_time,
                 query.Entity.end_time,
                 query.Entity.created_at
             );




        }
    }
}
