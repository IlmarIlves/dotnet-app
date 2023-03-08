using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using dotnet_app.models;

namespace dotnet_app.Dtos.User
{
    public class GetUserDto
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public List<MessagesModel>? messages { get; set; }
        public List<CalendarEventModel>? calendarEvents { get; set; }
    }
}