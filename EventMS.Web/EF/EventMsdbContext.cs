using System;
using System.Collections.Generic;
using EventMS.Web.EF.Tables;
using Microsoft.EntityFrameworkCore;

namespace EventMS.Web.EF;

public partial class EventMsdbContext : DbContext
{
    public EventMsdbContext()
    {
    }

    public EventMsdbContext(DbContextOptions<EventMsdbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<Registration> Registrations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DbConn");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Event>(entity =>
        {
            entity.Property(e => e.Category).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.OrganizerName).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.TicketPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Title).HasMaxLength(200);
            entity.Property(e => e.Venue).HasMaxLength(200);
        });

        modelBuilder.Entity<Registration>(entity =>
        {
            entity.Property(e => e.AmountPaid).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.AttendeeEmail).HasMaxLength(150);
            entity.Property(e => e.AttendeeName).HasMaxLength(100);
            entity.Property(e => e.AttendeePhone).HasMaxLength(50);
            entity.Property(e => e.BookingId).HasMaxLength(50);
            entity.Property(e => e.CheckInStatus).HasMaxLength(50);
            entity.Property(e => e.CheckInTime).HasColumnType("datetime");
            entity.Property(e => e.RegisteredAt).HasColumnType("datetime");

            entity.HasOne(d => d.Event).WithMany(p => p.Registrations)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Registrations_Events");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
