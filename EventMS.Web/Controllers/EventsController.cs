using EventMS.BLL.Interfaces;
using EventMS.Models.Entities;
using EventMS.Models.Enums;
using Microsoft.AspNetCore.Mvc;

namespace EventMS.Web.Controllers
{
    public class EventsController : Controller
    {
        private readonly IEventService _eventService;
        private readonly IRegistrationService _regService;

        public EventsController(IEventService eventService, IRegistrationService regService)
        {
            _eventService = eventService;
            _regService = regService;
        }

  
        public async Task<IActionResult> Index(string? title, string? category, string? venue)
        {
            ViewBag.Title = title;
            ViewBag.Category = category;
            ViewBag.Venue = venue;
            ViewBag.Categories = Enum.GetNames(typeof(EventCategory));

            var events = (title != null || category != null || venue != null)
                ? await _eventService.SearchEventsAsync(title, category, venue)
                : await _eventService.GetAllEventsAsync();

            return View(events);
        }

        public async Task<IActionResult> Details(int id)
        {
            var ev = await _eventService.GetEventByIdAsync(id);
            if (ev == null) return NotFound();
            return View(ev);
        }

      
        public IActionResult Create()
        {
            ViewBag.Categories = Enum.GetValues(typeof(EventCategory));
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event ev)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = Enum.GetValues(typeof(EventCategory));
                return View(ev);
            }

            var (success, message) = await _eventService.CreateEventAsync(ev);
            if (!success)
            {
                ModelState.AddModelError("", message);
                ViewBag.Categories = Enum.GetValues(typeof(EventCategory));
                return View(ev);
            }

            TempData["Success"] = message;
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var ev = await _eventService.GetEventByIdAsync(id);
            if (ev == null) return NotFound();
            ViewBag.Categories = Enum.GetValues(typeof(EventCategory));
            ViewBag.Statuses = Enum.GetValues(typeof(EventStatus));
            return View(ev);
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Event ev)
        {
            if (id != ev.Id) return BadRequest();
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = Enum.GetValues(typeof(EventCategory));
                ViewBag.Statuses = Enum.GetValues(typeof(EventStatus));
                return View(ev);
            }

            var (success, message) = await _eventService.UpdateEventAsync(ev);
            if (!success)
            {
                ModelState.AddModelError("", message);
                ViewBag.Categories = Enum.GetValues(typeof(EventCategory));
                ViewBag.Statuses = Enum.GetValues(typeof(EventStatus));
                return View(ev);
            }

            TempData["Success"] = message;
            return RedirectToAction(nameof(Index));
        }

   
        public async Task<IActionResult> Delete(int id)
        {
            var ev = await _eventService.GetEventByIdAsync(id);
            if (ev == null) return NotFound();
            return View(ev);
        }

       
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var (success, message) = await _eventService.DeleteEventAsync(id);
            if (!success)
            {
                TempData["Error"] = message;
                return RedirectToAction(nameof(Delete), new { id });
            }

            TempData["Success"] = message;
            return RedirectToAction(nameof(Index));
        }

        // GET /Events/Attendees/5  — Attendee report
        public async Task<IActionResult> Attendees(int id)
        {
            var report = await _regService.GetAttendeeReportAsync(id);
            return View(report);
        }

        // GET /Events/CheckIn/5  — Check-In page for organizer
        public async Task<IActionResult> CheckIn(int id)
        {
            var ev = await _eventService.GetEventByIdAsync(id);
            if (ev == null) return NotFound();
            ViewBag.Event = ev;
            return View();
        }

        // POST /Events/CheckIn
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckIn(int eventId, string bookingId)
        {
            var ev = await _eventService.GetEventByIdAsync(eventId);
            ViewBag.Event = ev;
            ViewBag.BookingId = bookingId;

            var (success, message, registration) = await _regService.CheckInAsync(bookingId);

            if (success)
                TempData["CheckInSuccess"] = message;
            else
                TempData["CheckInError"] = message;

            ViewBag.CheckedInReg = registration;
            return View();
        }
    }
}
