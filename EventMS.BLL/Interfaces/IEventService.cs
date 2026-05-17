using EventMS.Models.DTOs;
using EventMS.Models.Entities;

namespace EventMS.BLL.Interfaces
{
    public interface IEventService
    {
        Task<IEnumerable<Event>> GetAllEventsAsync();
        Task<Event?> GetEventByIdAsync(int id);
        Task<IEnumerable<Event>> GetUpcomingEventsAsync();
        Task<IEnumerable<Event>> GetPastEventsAsync();
        Task<IEnumerable<Event>> GetTodaysEventsAsync();
        Task<IEnumerable<Event>> SearchEventsAsync(string? title, string? category, string? venue);
        Task<(bool Success, string Message)> CreateEventAsync(Event ev);
        Task<(bool Success, string Message)> UpdateEventAsync(Event ev);
        Task<(bool Success, string Message)> DeleteEventAsync(int id);
        Task<DashboardStatsDto> GetDashboardStatsAsync();
    }
}
