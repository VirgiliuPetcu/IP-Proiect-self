using Shared.DTO;

using System.Net.Http.Json;

namespace IPFront.Service
{
    public class ReservationApiClient
    {


        private readonly HttpClient _httpClient;

        public ReservationApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<ReservationFullDTO>> getAllReservationAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<ReservationFullDTO>>("api/Reservation/getAll");
        }


        public async Task<ReservationFullDTO> getReservationByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<ReservationFullDTO>($"api/Reservation?id={id}");
        }

        public async Task<IEnumerable<ReservationFullDTO>> getReservationByResourceIdAsync(int resourceId)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<ReservationFullDTO>>($"api/Reservation?resourceId={resourceId}");
        }

        public async Task<IEnumerable<ReservationFullDTO>> getAllReservationByUserIdAsync(int userId)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<ReservationFullDTO>>($"api/Reservation?userId={userId}");
        }

        
        public async Task<IEnumerable<ReservationViewDTO>> getCurrentReservationByUserIdAsync(int userId)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<ReservationViewDTO>>($"api/Reservation/current/{userId}");
        }


        public async Task<ReservationCancelResponseDTO> CancelReservationAsync(int reservationId)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/Reservation/cancel", new { });
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ReservationCancelResponseDTO>();
        }

        public async Task<ReservationFullDTO> CreateReservationAsync(ReservationCreateDTO reservation)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/Reservation/create", reservation);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ReservationFullDTO>();
        }


        public async Task<IEnumerable<ReservationViewDTO>> ShowCurrentWeekReservationsAsync(int resourceId)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<ReservationViewDTO>>($"api/Reservation/thisWeek");
        }


    }
}
