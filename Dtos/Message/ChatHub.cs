using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_app.Data;
using dotnet_app.models;
using Microsoft.AspNetCore.SignalR;

namespace dotnet_app.Dtos.Message
{
    public class ChatHub : Hub
    {
        private readonly DataContext _dbContext;

        public ChatHub(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SendMessage(string text)
        {
            var message = new MessagesModel
            {
                // UserId = Context.User.Identity.Id,
                Text = text,
                SentAt = DateTime.UtcNow
            };

            _dbContext.Messages.Add(message);
            await _dbContext.SaveChangesAsync();

            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}