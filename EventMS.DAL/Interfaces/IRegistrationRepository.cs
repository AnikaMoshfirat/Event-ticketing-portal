using EventMS.Models.Entities;

namespace EventMS.DAL.Interfaces
{
    public interface IRegistrationRepository
    {
        Task<IEnumerable<Registration>> GetAllAsync();
        Task<Registration?> GetByIdAsync(int id);
        Task<Registration?> GetByBookingIdAsync(string bookingId);
        Task<IEnumerable<Registration>> GetByEventIdAsync(int eventId);
        Task<int> CountByEventIdAsync(int eventId);
        Task AddAsync(Registration registration);
        Task UpdateAsync(Registration registration);
        Task DeleteAsync(int id);
        Task<bool> EmailAlreadyRegisteredAsync(int eventId, string email);
    }
}
