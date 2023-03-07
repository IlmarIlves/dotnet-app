using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using dotnet_app.Data;
using dotnet_app.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace dotnet_app.Controllers
{
    [Route("[controller]")]
    public class CalendarEventsController : Controller
    {
        private readonly DataContext _context;

        public CalendarEventsController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent(CalendarEventDto model)
        {
            var calendarEvent = new CalendarEvent
            {
                Title = model.Title,
                Date = model.Date,
                Users = new List<UserModel>()
            };

            foreach (var userId in model.UserIds)
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    return NotFound($"User with Id {userId} not found.");
                }
                calendarEvent.Users.Add(user);
            }

            _context.CalendarEvents.Add(calendarEvent);
            await _context.SaveChangesAsync();

            return Ok(new { id = calendarEvent.Id });
        }

        [HttpGet]
        public async Task<IActionResult> GetEvents(int userId)
        {
            var user = await _context.Users
                .Include(u => u.Events)
                    .ThenInclude(ce => ce.Users)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return NotFound($"User with Id {userId} not found.");
            }

            var events = user.Events.Select(e => new CalendarEventDto
            {
                Id = e.Id,
                Title = e.Title,
                Date = e.Date,
                UserIds = e.Users.Select(u => u.Id).ToList()
            });

            return Ok(events);
        }
    }

    public class CalendarEventDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public List<int?> UserIds { get; set; }
    }
}