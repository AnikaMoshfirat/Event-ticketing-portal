-- ============================================================
-- EventMS - Database Setup Script
-- Run this in SQL Server Management Studio (SSMS)
-- ============================================================

-- Step 1: Create Database
CREATE DATABASE EventMsdb;
GO

USE EventMsdb;
GO

-- Step 2: Events Table
CREATE TABLE Events (
    Id            INT IDENTITY(1,1) PRIMARY KEY,
    Title         NVARCHAR(200)     NOT NULL,
    Description   NVARCHAR(1000)    NULL,
    Venue         NVARCHAR(200)     NOT NULL,
    EventDate     DATETIME2         NOT NULL,
    MaxCapacity   INT               NOT NULL,
    TicketPrice   DECIMAL(10,2)     NOT NULL DEFAULT 0,
    Category      NVARCHAR(50)      NOT NULL DEFAULT 'Seminar',
    Status        NVARCHAR(50)      NOT NULL DEFAULT 'Upcoming',
    OrganizerName NVARCHAR(150)     NULL,
    CreatedAt     DATETIME2         NOT NULL DEFAULT GETDATE()
);
GO

-- Step 3: Registrations Table
CREATE TABLE Registrations (
    Id            INT IDENTITY(1,1)  PRIMARY KEY,
    BookingId     NVARCHAR(30)       NOT NULL UNIQUE,
    EventId       INT                NOT NULL,
    AttendeeName  NVARCHAR(100)      NOT NULL,
    AttendeeEmail NVARCHAR(150)      NOT NULL,
    AttendeePhone NVARCHAR(20)       NULL,
    CheckInStatus NVARCHAR(30)       NOT NULL DEFAULT 'NotCheckedIn',
    CheckInTime   DATETIME2          NULL,
    RegisteredAt  DATETIME2          NOT NULL DEFAULT GETDATE(),
    AmountPaid    DECIMAL(10,2)      NOT NULL DEFAULT 0,
    CONSTRAINT FK_Registrations_Events FOREIGN KEY (EventId)
        REFERENCES Events(Id) ON DELETE CASCADE
);
GO

-- Step 4: Sample Data
INSERT INTO Events (Title, Description, Venue, EventDate, MaxCapacity, TicketPrice, Category, Status, OrganizerName)
VALUES
('.NET Advanced Seminar',
 'A full-day seminar on advanced .NET 8 features.',
 'BUET Auditorium, Dhaka',
 DATEADD(DAY, 7, CAST(GETDATE() AS DATE)),
 50, 500, 'Seminar', 'Upcoming', 'CSE Department'),

('Web Development Workshop',
 'Hands-on workshop on ASP.NET Core MVC.',
 'BRAC University, Mohakhali',
 CAST(GETDATE() AS DATE),
 30, 300, 'Workshop', 'Upcoming', 'Tech Club BD'),

('AI & ML Conference',
 'Two-day conference on AI and Machine Learning.',
 'Bashundhara Convention City',
 DATEADD(DAY, -10, CAST(GETDATE() AS DATE)),
 200, 1000, 'Conference', 'Completed', 'AI Society BD'),

('Rock Night Concert 2025',
 'An epic night of live rock music.',
 'Army Stadium, Dhaka',
 DATEADD(DAY, 14, CAST(GETDATE() AS DATE)),
 500, 800, 'Concert', 'Upcoming', 'SoundWave BD');
GO

-- ============================================================
-- NOTE: Identity tables (AspNetUsers, AspNetRoles etc.) are
-- created automatically. Run from Package Manager Console:
--   Default Project = DAL
--   Update-Database
-- ============================================================
