using EventMS.Models.Enums;

namespace EventMS.Models.DTOs
{
    // Dashboard summary stats
    public class DashboardStatsDto
    {
        public int TotalEvents { get; set; }
        public int UpcomingEvents { get; set; }
        public int TodaysEvents { get; set; }
        public int PastEvents { get; set; }
        public int TotalRegistrations { get; set; }
        public int TotalCheckedIn { get; set; }
        public decimal TotalRevenue { get; set; }
        public List<EventCardDto> RecentEvents { get; set; } = new();
        public List<CategoryCountDto> EventsByCategory { get; set; } = new();
    }

    // Compact event card for dashboard lists
    public class EventCardDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public string Venue { get; set; } = string.Empty;
        public int MaxCapacity { get; set; }
        public int RegisteredCount { get; set; }
        public int AvailableSeats { get; set; }
        public decimal TicketPrice { get; set; }
        public EventStatus Status { get; set; }
        public EventCategory Category { get; set; }
        public bool IsSoldOut => AvailableSeats <= 0;
    }

    public class CategoryCountDto
    {
        public string Category { get; set; } = string.Empty;
        public int Count { get; set; }
    }

    // Check-In request (Organizer enters BookingId)
    public class CheckInDto
    {
        public string BookingId { get; set; } = string.Empty;
        public int EventId { get; set; }
    }

    // Attendee report per event
    public class AttendeeReportDto
    {
        public string EventTitle { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public int TotalRegistered { get; set; }
        public int TotalCheckedIn { get; set; }
        public decimal TotalRevenue { get; set; }
        public List<RegistrationRowDto> Attendees { get; set; } = new();
    }

    public class RegistrationRowDto
    {
        public string BookingId { get; set; } = string.Empty;
        public string AttendeeName { get; set; } = string.Empty;
        public string AttendeeEmail { get; set; } = string.Empty;
        public string? AttendeePhone { get; set; }
        public DateTime RegisteredAt { get; set; }
        public CheckInStatus CheckInStatus { get; set; }
        public DateTime? CheckInTime { get; set; }
        public decimal AmountPaid { get; set; }
    }
}
