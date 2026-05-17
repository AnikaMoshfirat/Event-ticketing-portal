using EventMS.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EventMS.Web.Controllers
{
    public class RegistrationsController : Controller
    {
        private readonly IRegistrationService _regService;
        private readonly IEventService _eventService;

        public RegistrationsController(IRegistrationService regService, IEventService eventService)
        {
            _regService = regService;
            _eventService = eventService;
        }

        // GET /Registrations
        public async Task<IActionResult> Index()
        {
            var registrations = await _regService.GetAllRegistrationsAsync();
            return View(registrations);
        }

        // GET /Registrations/Register/5  (eventId)
        public async Task<IActionResult> Register(int eventId)
        {
            var ev = await _eventService.GetEventByIdAsync(eventId);
            if (ev == null) return NotFound();

            if (ev.IsSoldOut)
            {
                TempData["Error"] = $"Sorry! '{ev.Title}' is sold out.";
                return RedirectToAction("Details", "Events", new { id = eventId });
            }

            ViewBag.Event = ev;
            return View();
        }

        // POST /Registrations/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(int eventId, string name, string email, string? phone)
        {
            var ev = await _eventService.GetEventByIdAsync(eventId);
            ViewBag.Event = ev;

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email))
            {
                ModelState.AddModelError("", "Name and Email are required.");
                return View();
            }

            var (success, message, bookingId) = await _regService.RegisterAsync(eventId, name, email, phone);

            if (!success)
            {
                TempData["Error"] = message;
                return View();
            }

            TempData["Success"] = message;
            TempData["BookingId"] = bookingId;
            return RedirectToAction("Confirmation", new { bookingId });
        }

        // GET /Registrations/Confirmation?bookingId=EVT-xxx
        public async Task<IActionResult> Confirmation(string bookingId)
        {
            var reg = await _regService.GetByBookingIdAsync(bookingId);
            if (reg == null) return NotFound();
            return View(reg);
        }

        // POST /Registrations/Cancel/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var (success, message) = await _regService.CancelRegistrationAsync(id);
            TempData[success ? "Success" : "Error"] = message;
            return RedirectToAction(nameof(Index));
        }
    }
}
