using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_app.models
{
    public class UserModel
    {
        [Key]
        public int? Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public ICollection<MessagesModel> Messages { get; set; } = new List<MessagesModel>();
        public ICollection<CalendarEventModel> CalendarEvents { get; set; } = new List<CalendarEventModel>();
    }
}