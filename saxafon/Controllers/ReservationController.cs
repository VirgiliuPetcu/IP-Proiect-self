using Microsoft.AspNetCore.Mvc;
using saxafon.DTO;
using saxafon.Services;

namespace saxafon.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly ReservationService reservationService;
        public ReservationController(ReservationService _reservationService)
        {
            reservationService = _reservationService;
        }


        [HttpGet]

        public async Task<ActionResult<IEnumerable<ReservationFullDTO>>> getAllReservationApi() {
            var result = await  reservationService.GetAllReservations();
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReservationFullDTO>>> getReservationByIdApi(int id) {
            var result = await reservationService.GetReservationById(id);
            if (result == null) {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReservationFullDTO>>> getReservationByResourceIdApi(int resourceId)
        {
            var result = await reservationService.GetReservationByResourceId(resourceId);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReservationFullDTO>>> geAlltReservationByUserIdApi(int userId)
        {
            var result = await reservationService.GetAllReservationByUserId(userId);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReservationViewDTO>>> GetCurrentReservationByUserIdApi(int userId)
        {
            var result = await reservationService.GetCurrentReservationByUserId(userId);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<ReservationCancelResponseDTO>> CancelReservationApi(ReservationCancelDTO reservation) { 
            
            var result = await reservationService.CancelReservation(reservation);
            if (result == null)
            {
                return this.NotFound();
            }
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<ReservationViewDTO>> ShowCurrentWeekReservationsApi(int resourceId)
        {
            var result = await reservationService.ShowCurrentWeekReservations(resourceId);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        

        [HttpPost]
        public async Task<ActionResult<ReservationFullDTO>> CreateReservationApi(ReservationCreateDTO reservation) {
            var result = await reservationService.CreateReservation(reservation);
            if (result == null) {
                return this.Problem();
            }
            return CreatedAtAction(nameof(getReservationByIdApi), new { id = result.id }, result);

        }

  





    }
}
