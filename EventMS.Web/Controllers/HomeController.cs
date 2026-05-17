using EventMS.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EventMS.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEventService _eventService;

        public HomeController(IEventService eventService)
        {
            _eventService = eventService;
        }

        // GET /  — Dashboard
        public async Task<IActionResult> Index()
        {
            var stats = await _eventService.GetDashboardStatsAsync();
            return View(stats);
        }

        // GET /Home/Upcoming  — Feature 3: Date-based filter
        public async Task<IActionResult> Upcoming()
        {
            var events = await _eventService.GetUpcomingEventsAsync();
            ViewBag.Filter = "Upcoming";
            ViewBag.FilterLabel = "Upcoming Events";
            return View("FilteredEvents", events);
        }

        // GET /Home/Past  — Feature 3
        public async Task<IActionResult> Past()
        {
            var events = await _eventService.GetPastEventsAsync();
            ViewBag.Filter = "Past";
            ViewBag.FilterLabel = "Past Events";
            return View("FilteredEvents", events);
        }

        // GET /Home/Today  — Feature 3
        public async Task<IActionResult> Today()
        {
            var events = await _eventService.GetTodaysEventsAsync();
            ViewBag.Filter = "Today";
            ViewBag.FilterLabel = "Today's Events";
            return View("FilteredEvents", events);
        }
    }
}
