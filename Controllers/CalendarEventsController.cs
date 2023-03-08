using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using dotnet_app.Data;
using dotnet_app.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace dotnet_app.Controllers
{
    [Route("[controller]")]
    public class CalendarEventsController : Controller
    {
        private readonly DataContext _context;
        private readonly UserManager<UserModel> _userManager;

        public CalendarEventsController(DataContext context, UserManager<UserModel> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CalendarEventModel>>> GetCalendarEvents()
        {
            var events = await _context.CalendarEvents.ToListAsync();
            return Ok(events);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CalendarEventModel>> GetCalendarEvent(int id)
        {
            var calendarEvent = await _context.CalendarEvents.FindAsync(id);

            if (calendarEvent == null)
            {
                return NotFound();
            }

            return Ok(calendarEvent);
        }

        [HttpPost]
        public async Task<ActionResult<CalendarEventModel>> CreateCalendarEvent(CalendarEventModel calendarEvent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Retrieve the user IDs from the calendar event object
            var userIds = calendarEvent.Users.Select(u => u.Id).ToList();

            // Get the users associated with the provided user IDs
            var users = await _userManager.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();

            // Associate the users with the calendar event
            calendarEvent.Users = users;

            // Add the event to the database
            _context.CalendarEvents.Add(calendarEvent);
            await _context.SaveChangesAsync();

            return Ok(calendarEvent);
        }
    }


}