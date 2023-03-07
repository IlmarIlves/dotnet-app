using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_app.models
{
    public class MessagesModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public int SenderId { get; set; }
        public int RecipientId { get; set; }
        public UserModel Sender { get; set; }
        public UserModel Recipient { get; set; }
    }
}