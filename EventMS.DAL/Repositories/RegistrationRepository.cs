using EventMS.DAL.Data;
using EventMS.DAL.Interfaces;
using EventMS.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventMS.DAL.Repositories
{
    public class RegistrationRepository : IRegistrationRepository
    {
        private readonly EventMsdbContext _context;

        public RegistrationRepository(EventMsdbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Registration>> GetAllAsync()
            => await _context.Registrations
                .Include(r => r.Event)
                .OrderByDescending(r => r.RegisteredAt)
                .ToListAsync();

        public async Task<Registration?> GetByIdAsync(int id)
            => await _context.Registrations
                .Include(r => r.Event)
                .FirstOrDefaultAsync(r => r.Id == id);

        // Lookup by Booking ID (for Check-In feature)
        public async Task<Registration?> GetByBookingIdAsync(string bookingId)
            => await _context.Registrations
                .Include(r => r.Event)
                .FirstOrDefaultAsync(r => r.BookingId == bookingId);

        public async Task<IEnumerable<Registration>> GetByEventIdAsync(int eventId)
            => await _context.Registrations
                .Include(r => r.Event)
                .Where(r => r.EventId == eventId)
                .OrderByDescending(r => r.RegisteredAt)
                .ToListAsync();

        public async Task<int> CountByEventIdAsync(int eventId)
            => await _context.Registrations.CountAsync(r => r.EventId == eventId);

        public async Task AddAsync(Registration registration)
        {
            await _context.Registrations.AddAsync(registration);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Registration registration)
        {
            _context.Registrations.Update(registration);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var reg = await _context.Registrations.FindAsync(id);
            if (reg != null)
            {
                _context.Registrations.Remove(reg);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> EmailAlreadyRegisteredAsync(int eventId, string email)
            => await _context.Registrations
                .AnyAsync(r => r.EventId == eventId &&
                               r.AttendeeEmail.ToLower() == email.ToLower());
    }
}
