using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_app.models
{
    public class CalendarEvent
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;
    }
}