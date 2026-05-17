using EventMS.Models.DTOs;
using EventMS.Models.Entities;

namespace EventMS.BLL.Interfaces
{
    public interface IRegistrationService
    {
        Task<IEnumerable<Registration>> GetAllRegistrationsAsync();
        Task<Registration?> GetRegistrationByIdAsync(int id);
        Task<Registration?> GetByBookingIdAsync(string bookingId);
        Task<IEnumerable<Registration>> GetRegistrationsByEventAsync(int eventId);

       
        Task<(bool Success, string Message, string? BookingId)> RegisterAsync(int eventId, string name, string email, string? phone);

   
        Task<(bool Success, string Message, Registration? Registration)> CheckInAsync(string bookingId);

        
        Task<(bool Success, string Message)> CancelRegistrationAsync(int registrationId);

        Task<AttendeeReportDto> GetAttendeeReportAsync(int eventId);
    }
}
