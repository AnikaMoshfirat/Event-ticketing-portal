using System.ComponentModel.DataAnnotations;
using EventMS.Models.Enums;

namespace EventMS.Models.Entities
{
    public class Event
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        [Display(Name = "Event Title")]
        public string Title { get; set; } = string.Empty;

        [Required, MaxLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Required, MaxLength(200)]
        public string Venue { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Event Date & Time")]
        public DateTime EventDate { get; set; }

        [Required]
        [Display(Name = "Max Capacity")]
        [Range(1, 100000)]
        public int MaxCapacity { get; set; }

        [Required]
        [Display(Name = "Ticket Price (৳)")]
        [Range(0, 1000000)]
        public decimal TicketPrice { get; set; }

        [Required]
        public EventCategory Category { get; set; } = EventCategory.Seminar;

        public EventStatus Status { get; set; } = EventStatus.Upcoming;

        [Display(Name = "Organizer Name")]
        [MaxLength(150)]
        public string OrganizerName { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation
        public ICollection<Registration> Registrations { get; set; } = new List<Registration>();

        // ── Computed Properties ──────────────────────────
        public int RegisteredCount => Registrations?.Count ?? 0;
        public int AvailableSeats => MaxCapacity - RegisteredCount;
        public bool IsSoldOut => AvailableSeats <= 0;

        // Date helpers for LINQ filtering
        public bool IsUpcoming => EventDate.Date > DateTime.Today;
        public bool IsPast => EventDate.Date < DateTime.Today;
        public bool IsToday => EventDate.Date == DateTime.Today;
    }
}
