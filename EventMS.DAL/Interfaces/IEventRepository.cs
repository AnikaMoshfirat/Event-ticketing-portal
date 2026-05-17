using EventMS.Models.Entities;

namespace EventMS.DAL.Interfaces
{
    public interface IEventRepository
    {
        Task<IEnumerable<Event>> GetAllAsync();
        Task<Event?> GetByIdAsync(int id);
        Task<IEnumerable<Event>> GetUpcomingAsync();
        Task<IEnumerable<Event>> GetPastAsync();
        Task<IEnumerable<Event>> GetTodaysAsync();
        Task<IEnumerable<Event>> SearchAsync(string? title, string? category, string? venue);
        Task AddAsync(Event ev);
        Task UpdateAsync(Event ev);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
