using EventMS.BLL.Interfaces;
using EventMS.DAL.Interfaces;
using EventMS.Models.DTOs;
using EventMS.Models.Entities;
using EventMS.Models.Enums;

namespace EventMS.BLL.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IRegistrationRepository _regRepo;
        private readonly IEventRepository _eventRepo;

        public RegistrationService(IRegistrationRepository regRepo, IEventRepository eventRepo)
        {
            _regRepo = regRepo;
            _eventRepo = eventRepo;
        }

        public async Task<IEnumerable<Registration>> GetAllRegistrationsAsync()
            => await _regRepo.GetAllAsync();

        public async Task<Registration?> GetRegistrationByIdAsync(int id)
            => await _regRepo.GetByIdAsync(id);

        public async Task<Registration?> GetByBookingIdAsync(string bookingId)
            => await _regRepo.GetByBookingIdAsync(bookingId);

        public async Task<IEnumerable<Registration>> GetRegistrationsByEventAsync(int eventId)
            => await _regRepo.GetByEventIdAsync(eventId);


        public async Task<(bool Success, string Message, string? BookingId)> RegisterAsync(
            int eventId, string name, string email, string? phone)
        {
            var ev = await _eventRepo.GetByIdAsync(eventId);
            if (ev == null)
                return (false, "Event not found.", null);


            int currentCount = await _regRepo.CountByEventIdAsync(eventId);
            if (currentCount >= ev.MaxCapacity)
            {

                ev.Status = EventStatus.SoldOut;
                await _eventRepo.UpdateAsync(ev);
                return (false, $"Sorry! '{ev.Title}' is sold out. No seats available.", null);
            }


            if (ev.EventDate < DateTime.Now)
                return (false, "Cannot register for a past event.", null);


            bool alreadyRegistered = await _regRepo.EmailAlreadyRegisteredAsync(eventId, email);
            if (alreadyRegistered)
                return (false, $"'{email}' is already registered for this event.", null);


            string bookingId = GenerateBookingId();

            var registration = new Registration
            {
                BookingId = bookingId,
                EventId = eventId,
                AttendeeName = name,
                AttendeeEmail = email,
                AttendeePhone = phone,
                RegisteredAt = DateTime.Now,
                CheckInStatus = CheckInStatus.NotCheckedIn,
                AmountPaid = ev.TicketPrice
            };

            await _regRepo.AddAsync(registration);

  
            int newCount = await _regRepo.CountByEventIdAsync(eventId);
            if (newCount >= ev.MaxCapacity)
            {
                ev.Status = EventStatus.SoldOut;
                await _eventRepo.UpdateAsync(ev);
            }

            return (true,
                $"Registration successful! Your Booking ID is: {bookingId}. Keep it safe for check-in.",
                bookingId);
        }


        public async Task<(bool Success, string Message, Registration? Registration)> CheckInAsync(string bookingId)
        {
            var reg = await _regRepo.GetByBookingIdAsync(bookingId.Trim().ToUpper());
            if (reg == null)
                return (false, $"No registration found for Booking ID: '{bookingId}'.", null);

            if (reg.CheckInStatus == CheckInStatus.CheckedIn)
                return (false,
                    $"'{reg.AttendeeName}' is already checked in at {reg.CheckInTime:hh:mm tt}.",
                    reg);

            reg.CheckInStatus = CheckInStatus.CheckedIn;
            reg.CheckInTime = DateTime.Now;
            await _regRepo.UpdateAsync(reg);

            return (true,
                $"✓ Check-in successful! Welcome, {reg.AttendeeName}!",
                reg);
        }

        public async Task<(bool Success, string Message)> CancelRegistrationAsync(int registrationId)
        {
            var reg = await _regRepo.GetByIdAsync(registrationId);
            if (reg == null) return (false, "Registration not found.");

            if (reg.CheckInStatus == CheckInStatus.CheckedIn)
                return (false, "Cannot cancel a registration that has already checked in.");

          
            var ev = await _eventRepo.GetByIdAsync(reg.EventId);
            await _regRepo.DeleteAsync(registrationId);

            if (ev != null && ev.Status == EventStatus.SoldOut)
            {
                ev.Status = EventStatus.Upcoming;
                await _eventRepo.UpdateAsync(ev);
            }

            return (true, "Registration cancelled successfully.");
        }


        public async Task<AttendeeReportDto> GetAttendeeReportAsync(int eventId)
        {
            var ev = await _eventRepo.GetByIdAsync(eventId);
            var registrations = (await _regRepo.GetByEventIdAsync(eventId)).ToList();

            return new AttendeeReportDto
            {
                EventTitle = ev?.Title ?? "Unknown",
                EventDate = ev?.EventDate ?? DateTime.MinValue,
                TotalRegistered = registrations.Count,
                TotalCheckedIn = registrations.Count(r => r.CheckInStatus == CheckInStatus.CheckedIn),
                TotalRevenue = registrations.Sum(r => r.AmountPaid),
                Attendees = registrations.Select(r => new RegistrationRowDto
                {
                    BookingId = r.BookingId,
                    AttendeeName = r.AttendeeName,
                    AttendeeEmail = r.AttendeeEmail,
                    AttendeePhone = r.AttendeePhone,
                    RegisteredAt = r.RegisteredAt,
                    CheckInStatus = r.CheckInStatus,
                    CheckInTime = r.CheckInTime,
                    AmountPaid = r.AmountPaid
                }).ToList()
            };
        }


        private static int _bookingCounter = 1;

        private static string GenerateBookingId()
        {
            return "A" + (_bookingCounter++).ToString("D2");
        }
    }
}
