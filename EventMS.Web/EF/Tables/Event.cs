using System;
using System.Collections.Generic;

namespace EventMS.Web.EF.Tables;

public partial class Event
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Venue { get; set; } = null!;

    public DateTime EventDate { get; set; }

    public int MaxCapacity { get; set; }

    public decimal TicketPrice { get; set; }

    public string Category { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string OrganizerName { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Registration> Registrations { get; set; } = new List<Registration>();
}
