# EventMS — Event Ticketing & Attendee Management System
### Advanced Programming with .NET — Course Project

## 🗂️ Project Structure

```
EventMS/
├── EventMS.sln
├── EventMS.Models/
│   ├── Entities/
│   │   ├── Event.cs
│   │   └── Registration.cs
│   ├── DTOs/
│   │   └── EventDtos.cs        ← DashboardStatsDto, AttendeeReportDto, etc.
│   └── Enums/
│       └── EventEnums.cs       ← EventStatus, EventCategory, CheckInStatus
│
├── EventMS.DAL/
│   ├── Data/
│   │   └── EventDbContext.cs   ← EF Core + Seed Data
│   ├── Interfaces/
│   │   ├── IEventRepository.cs
│   │   └── IRegistrationRepository.cs
│   ├── Repositories/
│   │   ├── EventRepository.cs
│   │   └── RegistrationRepository.cs
│   └── Migrations/
│
├── EventMS.BLL/
│   ├── Interfaces/
│   │   ├── IEventService.cs
│   │   └── IRegistrationService.cs
│   └── Services/
│       ├── EventService.cs         ← Business rules, dashboard stats
│       └── RegistrationService.cs  ← BookingId generation, Check-In, Sold-Out logic
│
└── EventMS.Web/
    ├── Controllers/
    │   ├── HomeController.cs       ← Dashboard + date filters
    │   ├── EventsController.cs     ← CRUD + CheckIn + Attendees
    │   └── RegistrationsController.cs
    ├── Views/
    │   ├── Home/
    │   │   ├── Index.cshtml        ← Dashboard with 3 filter buttons
    │   │   └── FilteredEvents.cshtml
    │   ├── Events/
    │   │   ├── Index.cshtml, Details.cshtml, Create.cshtml
    │   │   ├── Edit.cshtml, Delete.cshtml
    │   │   ├── CheckIn.cshtml      ← Check-In portal
    │   │   └── Attendees.cshtml    ← Attendee report
    │   ├── Registrations/
    │   │   ├── Index.cshtml
    │   │   ├── Register.cshtml
    │   │   └── Confirmation.cshtml ← Shows Booking ID
    │   └── Shared/
    │       └── _Layout.cshtml
    ├── Program.cs                  ← DI registration
    └── appsettings.json
```

---

