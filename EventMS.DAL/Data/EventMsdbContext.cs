using EventMS.Models.Entities;
using EventMS.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace EventMS.DAL.Data
{
    public class EventMsdbContext : DbContext
    {
        public EventMsdbContext(DbContextOptions<EventMsdbContext> options) : base(options) { }

        public DbSet<Event> Events { get; set; }
        public DbSet<Registration> Registrations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Event config
            modelBuilder.Entity<Event>(e =>
            {
                e.Property(x => x.TicketPrice).HasColumnType("decimal(10,2)");
                e.Property(x => x.Status).HasConversion<string>();
                e.Property(x => x.Category).HasConversion<string>();
            });

            // Registration config
            modelBuilder.Entity<Registration>(e =>
            {
                e.HasIndex(r => r.BookingId).IsUnique();
                e.Property(r => r.AmountPaid).HasColumnType("decimal(10,2)");
                e.Property(r => r.CheckInStatus).HasConversion<string>();

                e.HasOne(r => r.Event)
                 .WithMany(ev => ev.Registrations)
                 .HasForeignKey(r => r.EventId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // ── Seed Data ──────────────────────────────────────────────
            var today = DateTime.Today;

            modelBuilder.Entity<Event>().HasData(
                new Event
                {
                    Id = 1,
                    Title = ".NET Advanced Programming Seminar",
                    Description = "A full-day seminar covering advanced .NET 8 features including minimal APIs, EF Core 8, and Blazor.",
                    Venue = "BUET Auditorium, Dhaka",
                    EventDate = today.AddDays(7),
                    MaxCapacity = 50,
                    TicketPrice = 500,
                    Category = EventCategory.Seminar,
                    Status = EventStatus.Upcoming,
                    OrganizerName = "CSE Department",
                    CreatedAt = today
                },
                new Event
                {
                    Id = 2,
                    Title = "Web Development Workshop",
                    Description = "Hands-on workshop on building full-stack web apps with ASP.NET Core and React.",
                    Venue = "BRAC University, Mohakhali",
                    EventDate = today,
                    MaxCapacity = 30,
                    TicketPrice = 300,
                    Category = EventCategory.Workshop,
                    Status = EventStatus.Upcoming,
                    OrganizerName = "Tech Club BD",
                    CreatedAt = today.AddDays(-3)
                },
                new Event
                {
                    Id = 3,
                    Title = "AI & Machine Learning Conference",
                    Description = "Two-day conference on the latest in AI, ML, and data science in Bangladesh.",
                    Venue = "Bashundhara International Convention City",
                    EventDate = today.AddDays(-10),
                    MaxCapacity = 200,
                    TicketPrice = 1000,
                    Category = EventCategory.Conference,
                    Status = EventStatus.Completed,
                    OrganizerName = "AI Society BD",
                    CreatedAt = today.AddDays(-30)
                },
                new Event
                {
                    Id = 4,
                    Title = "Rock Night Concert 2025",
                    Description = "An epic night of live rock music featuring top Bangladeshi bands.",
                    Venue = "Army Stadium, Dhaka",
                    EventDate = today.AddDays(14),
                    MaxCapacity = 500,
                    TicketPrice = 800,
                    Category = EventCategory.Concert,
                    Status = EventStatus.Upcoming,
                    OrganizerName = "SoundWave BD",
                    CreatedAt = today.AddDays(-5)
                }
            );
        }
    }
}
