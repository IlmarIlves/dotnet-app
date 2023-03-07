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
        public List<MessagesModel> SentMessages { get; set; }
        public List<MessagesModel> ReceivedMessages { get; set; }
        public ICollection<CalendarEvent> Events { get; set; }
    }
}