using EventMS.BLL.Interfaces;
using EventMS.DAL.Interfaces;
using EventMS.Models.DTOs;
using EventMS.Models.Entities;
using EventMS.Models.Enums;

namespace EventMS.BLL.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepo;

        public EventService(IEventRepository eventRepo)
        {
            _eventRepo = eventRepo;
        }

        public async Task<IEnumerable<Event>> GetAllEventsAsync()
            => await _eventRepo.GetAllAsync();

        public async Task<Event?> GetEventByIdAsync(int id)
            => await _eventRepo.GetByIdAsync(id);

      
        public async Task<IEnumerable<Event>> GetUpcomingEventsAsync()
            => await _eventRepo.GetUpcomingAsync();

        public async Task<IEnumerable<Event>> GetPastEventsAsync()
            => await _eventRepo.GetPastAsync();

        public async Task<IEnumerable<Event>> GetTodaysEventsAsync()
            => await _eventRepo.GetTodaysAsync();

        public async Task<IEnumerable<Event>> SearchEventsAsync(string? title, string? category, string? venue)
            => await _eventRepo.SearchAsync(title, category, venue);

        public async Task<(bool Success, string Message)> CreateEventAsync(Event ev)
        {
           
            if (ev.EventDate < DateTime.Now)
                return (false, "Event date must be in the future.");

            if (ev.MaxCapacity < 1)
                return (false, "Capacity must be at least 1.");

            ev.Status = EventStatus.Upcoming;
            ev.CreatedAt = DateTime.Now;
            await _eventRepo.AddAsync(ev);
            return (true, $"Event '{ev.Title}' created successfully.");
        }

        public async Task<(bool Success, string Message)> UpdateEventAsync(Event ev)
        {
            if (!await _eventRepo.ExistsAsync(ev.Id))
                return (false, "Event not found.");

            await _eventRepo.UpdateAsync(ev);
            return (true, "Event updated successfully.");
        }

        public async Task<(bool Success, string Message)> DeleteEventAsync(int id)
        {
            var ev = await _eventRepo.GetByIdAsync(id);
            if (ev == null) return (false, "Event not found.");

            if (ev.Registrations.Any())
                return (false, $"Cannot delete '{ev.Title}' — it has {ev.Registrations.Count} registration(s).");

            await _eventRepo.DeleteAsync(id);
            return (true, "Event deleted successfully.");
        }

      
        public async Task<DashboardStatsDto> GetDashboardStatsAsync()
        {
            var allEvents = (await _eventRepo.GetAllAsync()).ToList();
            var today = DateTime.Today;

            var byCategory = allEvents
                .GroupBy(e => e.Category.ToString())
                .Select(g => new CategoryCountDto { Category = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .ToList();

            var recentEvents = allEvents
                .OrderByDescending(e => e.CreatedAt)
                .Take(5)
                .Select(e => new EventCardDto
                {
                    Id = e.Id,
                    Title = e.Title,
                    EventDate = e.EventDate,
                    Venue = e.Venue,
                    MaxCapacity = e.MaxCapacity,
                    RegisteredCount = e.Registrations.Count,
                    AvailableSeats = e.MaxCapacity - e.Registrations.Count,
                    TicketPrice = e.TicketPrice,
                    Status = e.Status,
                    Category = e.Category
                }).ToList();

            return new DashboardStatsDto
            {
                TotalEvents = allEvents.Count,
                UpcomingEvents = allEvents.Count(e => e.EventDate.Date > today),
                TodaysEvents = allEvents.Count(e => e.EventDate.Date == today),
                PastEvents = allEvents.Count(e => e.EventDate.Date < today),
                TotalRegistrations = allEvents.Sum(e => e.Registrations.Count),
                TotalCheckedIn = allEvents
                    .SelectMany(e => e.Registrations)
                    .Count(r => r.CheckInStatus == CheckInStatus.CheckedIn),
                TotalRevenue = allEvents
                    .SelectMany(e => e.Registrations)
                    .Sum(r => r.AmountPaid),
                RecentEvents = recentEvents,
                EventsByCategory = byCategory
            };
        }
    }
}
