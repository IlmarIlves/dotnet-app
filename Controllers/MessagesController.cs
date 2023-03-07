using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using dotnet_app.Data;
using dotnet_app.Dtos.Message;
using dotnet_app.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace dotnet_app.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public MessagesController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(MessageCreateDto messageCreateDto)
        {
            var sender = await _context.Users.FindAsync(messageCreateDto.SenderId);
            if (sender == null)
            {
                return BadRequest("Invalid sender ID");
            }

            var recipient = await _context.Users.FindAsync(messageCreateDto.RecipientId);
            if (recipient == null)
            {
                return BadRequest("Invalid recipient ID");
            }

            var message = _mapper.Map<MessagesModel>(messageCreateDto);
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return Ok(message);
        }

        [HttpGet]
        public async Task<IActionResult> GetMessagesForUser(int userId)
        {
            var messages = await _context.Messages
                .Include(m => m.Sender)
                .Include(m => m.Recipient)
                .Where(m => m.RecipientId == userId || m.SenderId == userId)
                .ToListAsync();

            var messageDtos = _mapper.Map<List<MessageDto>>(messages);
            return Ok(messageDtos);
        }
    }
}