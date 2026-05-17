using System;
using System.Collections.Generic;

namespace EventMS.Web.EF.Tables;

public partial class Registration
{
    public int Id { get; set; }

    public string BookingId { get; set; } = null!;

    public int EventId { get; set; }

    public string AttendeeName { get; set; } = null!;

    public string AttendeeEmail { get; set; } = null!;

    public string AttendeePhone { get; set; } = null!;

    public string? CheckInStatus { get; set; }

    public DateTime? CheckInTime { get; set; }

    public DateTime? RegisteredAt { get; set; }

    public decimal? AmountPaid { get; set; }

    public virtual Event Event { get; set; } = null!;
}
