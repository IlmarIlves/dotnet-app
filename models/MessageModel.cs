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
        public ICollection<UserModel> Users { get; set; } = new List<UserModel>();
    }
}