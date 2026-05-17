using EventMS.DAL.Data;
using EventMS.DAL.Interfaces;
using EventMS.Models.Entities;
using EventMS.Models.Enums;
using Microsoft.EntityFrameworkCore;


namespace EventMS.DAL.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly EventMsdbContext _context;

        public EventRepository(EventMsdbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Event>> GetAllAsync()
            => await _context.Events
                .Include(e => e.Registrations)
                .OrderBy(e => e.EventDate)
                .ToListAsync();

        public async Task<Event?> GetByIdAsync(int id)
            => await _context.Events
                .Include(e => e.Registrations)
                .FirstOrDefaultAsync(e => e.Id == id);

        // ── Date-based Filtering (Feature 3) ──────────────────────────
        public async Task<IEnumerable<Event>> GetUpcomingAsync()
            => await _context.Events
                .Include(e => e.Registrations)
                .Where(e => e.EventDate.Date > DateTime.Today)
                .OrderBy(e => e.EventDate)
                .ToListAsync();

        public async Task<IEnumerable<Event>> GetPastAsync()
            => await _context.Events
                .Include(e => e.Registrations)
                .Where(e => e.EventDate.Date < DateTime.Today)
                .OrderByDescending(e => e.EventDate)
                .ToListAsync();

        public async Task<IEnumerable<Event>> GetTodaysAsync()
            => await _context.Events
                .Include(e => e.Registrations)
                .Where(e => e.EventDate.Date == DateTime.Today)
                .OrderBy(e => e.EventDate)
                .ToListAsync();

        public async Task<IEnumerable<Event>> SearchAsync(string? title, string? category, string? venue)
        {
            var query = _context.Events.Include(e => e.Registrations).AsQueryable();

            if (!string.IsNullOrWhiteSpace(title))
                query = query.Where(e => e.Title.Contains(title));

            if (!string.IsNullOrWhiteSpace(category) && category != "-- All Categories --")
            {
                if (Enum.TryParse<EventCategory>(category, out var selectedCategory))
                {
                    query = query.Where(e => e.Category == selectedCategory);
                }
            }

            if (!string.IsNullOrWhiteSpace(venue))
                query = query.Where(e => e.Venue.Contains(venue));

            return await query.OrderBy(e => e.EventDate).ToListAsync();
        }

        public async Task AddAsync(Event ev)
        {
            await _context.Events.AddAsync(ev);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Event ev)
        {
            _context.Events.Update(ev);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var ev = await _context.Events.FindAsync(id);
            if (ev != null)
            {
                _context.Events.Remove(ev);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
            => await _context.Events.AnyAsync(e => e.Id == id);
    }
}
