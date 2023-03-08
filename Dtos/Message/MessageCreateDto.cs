using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_app.Dtos.Message
{
    public class MessageCreateDto
    {
        [Required]
        public string Content { get; set; }

        [Required]
        public int SenderId { get; set; }

        [Required]
        public List<int> RecipientIds { get; set; }
    }
}