using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using dotnet_app.Data;
using dotnet_app.Dtos.Message;
using dotnet_app.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace dotnet_app.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly DataContext _context;

        public MessagesController(DataContext context, UserManager<UserModel> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("getMessages")]
        public async Task<ActionResult<IEnumerable<MessagesModel>>> GetMessages()
        {
            var messages = await _context.Messages.ToListAsync();
            return Ok(messages);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MessagesModel>> GetMessage(int id)
        {
            var message = await _context.Messages.FindAsync(id);

            if (message == null)
            {
                return NotFound();
            }

            return Ok(message);
        }

        [HttpPost("createMessages")]
        public async Task<ActionResult<MessagesModel>> CreateMessage([FromBody] MessagesModel message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Get the user who created the message
            var user = await _userManager.GetUserAsync(HttpContext.User);

            // Create a new UserModel object for the user
            var userModel = new UserModel
            {
                Id = user.Id,
                Username = user.Username
            };

            // Add the UserModel object to the message's user collection
            message.Users = new List<UserModel> { userModel };

            // Add the message to the database
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return Ok(message);
        }
    }
}