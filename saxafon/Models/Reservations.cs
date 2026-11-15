using System.ComponentModel.DataAnnotations;

namespace saxafon.Models
{
    public enum ReservationStatus { 
        CONFIRMED,
        CANCELED,
        REJECTED,
        PENDING,
        
    
    }
    public class Reservations
    {
        [Key]
        public int Id { get; set;  }
        public Resources Resource { get; set; } = null!;
        public int ResourceId { get; set; } 
        public int UserId { get; set; }
        public User user { get; set; } = null!;

        [MaxLength(300)]
        public string? purpose { get; set; }
        public ReservationStatus status_rezervare { get; set; }
        public DateTime start_time { get; set; } 
        public DateTime end_time { get; set; }
        public DateTime created_at { get; set; }
        public DateTime? canceled_at { get; set; }

       

    }
}
