namespace EventMS.Models.Enums
{
    public enum EventStatus
    {
        Upcoming,
        Ongoing,
        Completed,
        Cancelled,
        SoldOut        // Auto-set when seats fill up
    }

    public enum EventCategory
    {
        Seminar,
        Workshop,
        Concert,
        Conference,
        Webinar,
        Other
    }

    public enum CheckInStatus
    {
        NotCheckedIn,
        CheckedIn
    }
}
