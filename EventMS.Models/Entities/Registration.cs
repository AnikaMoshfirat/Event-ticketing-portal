using System.ComponentModel.DataAnnotations;
using EventMS.Models.Enums;

namespace EventMS.Models.Entities
{
    public class Registration
    {
        public int Id { get; set; }

        // ── Unique Booking ID (e.g. EVT-20240515-A3F7) ──────────────
        [Required, MaxLength(30)]
        [Display(Name = "Booking ID")]
        public string BookingId { get; set; } = string.Empty;

        // ── Foreign Keys ─────────────────────────────────────────────
        public int EventId { get; set; }
        public Event Event { get; set; } = null!;

        // ── Attendee Info ─────────────────────────────────────────────
        [Required, MaxLength(100)]
        [Display(Name = "Full Name")]
        public string AttendeeName { get; set; } = string.Empty;

        [Required, EmailAddress, MaxLength(150)]
        [Display(Name = "Email")]
        public string AttendeeEmail { get; set; } = string.Empty;

        [Phone, MaxLength(20)]
        [Display(Name = "Phone")]
        public string? AttendeePhone { get; set; }

        // ── Status ────────────────────────────────────────────────────
        [Display(Name = "Check-In Status")]
        public CheckInStatus CheckInStatus { get; set; } = CheckInStatus.NotCheckedIn;

        public DateTime? CheckInTime { get; set; }

        public DateTime RegisteredAt { get; set; } = DateTime.Now;

        public decimal AmountPaid { get; set; }
    }
}
