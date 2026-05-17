# EventMS — Event Ticketing & Attendee Management System
### Advanced Programming with .NET — Course Project

---

## Project Overview

A full **ASP.NET Core MVC** application for managing seminars, workshops, and concerts.

### 3-Layer Architecture
```
EventMS.Models   →  Entities, DTOs, Enums  (no dependencies)
EventMS.DAL      →  DbContext, Repositories  (depends on Models)
EventMS.BLL      →  Services, Business Logic  (depends on DAL + Models)
EventMS.Web      →  Controllers, Views, UI    (depends on BLL + Models)
```

---

## ✅ Features Implemented

### Feature 1 — Unique Booking ID + Check-In
- Every registration generates a unique ID like **`EVT-20250515-A3F7`**
- Format: `EVT-{EventDate}-{4 random hex chars}`
- Organizer opens **Check-In Portal** → enters Booking ID → attendee is checked in
- Duplicate check-in shows a warning with timestamp

### Feature 2 — Automatic Sold-Out Status
- Every event has `MaxCapacity`
- On each registration, backend counts current registrations
- If `count >= MaxCapacity` → event `Status` auto-changes to **SoldOut**
- "Register Now" button disappears and shows **"Sold Out"** instead
- If a registration is cancelled, the seat is freed and status reverts to **Upcoming**

### Feature 3 — Date-based Event Filtering
Dashboard has 3 buttons using LINQ:
```csharp
// Upcoming
.Where(e => e.EventDate.Date > DateTime.Today)

// Today's
.Where(e => e.EventDate.Date == DateTime.Today)

// Past
.Where(e => e.EventDate.Date < DateTime.Today)
```

### Additional Features
- Full CRUD for Events and Registrations
- Advanced Search (title, category, venue)
- Attendee report per event with check-in stats
- Revenue tracking
- Dashboard analytics with stats cards

---

## 🛠️ Setup Instructions

### Prerequisites
- Visual Studio 2022 (or VS Code with C# extension)
- .NET 8 SDK
- SQL Server or SQL Server LocalDB (comes with Visual Studio)

### Steps

**1. Open the solution**
```
Open EventMS.sln in Visual Studio
```

**2. Set connection string** in `EventMS.Web/appsettings.json`:
```json
"DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=EventMSDb;Trusted_Connection=True;"
```

**3. Apply migrations** — Open Package Manager Console:
```powershell
# Select EventMS.DAL as Default Project in PMC
Add-Migration InitialCreate
Update-Database
```

Or with CLI:
```bash
cd EventMS.DAL
dotnet ef database update --startup-project ../EventMS.Web
```

**4. Set Startup Project**
Right-click `EventMS.Web` → Set as Startup Project

**5. Run**
Press `F5` or `Ctrl+F5`

---

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

## Key Design Patterns Used

| Pattern | Where |
|---------|-------|
| Repository Pattern | DAL layer — `IEventRepository`, `IRegistrationRepository` |
| Service Layer (BLL) | Business rules separated from UI |
| Dependency Injection | `Program.cs` — all services registered as `Scoped` |
| DTO Pattern | `DashboardStatsDto`, `AttendeeReportDto` — data shaping |
| Async/Await | All DB calls are fully async |
| LINQ | Filtering, grouping, projections throughout |
