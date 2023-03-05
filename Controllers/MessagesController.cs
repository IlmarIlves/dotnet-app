using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using dotnet_app.Data;
using dotnet_app.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace dotnet_app.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MessagesController : Controller
    {
        private readonly DataContext _dbContext;

        public MessagesController(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessagesModel>>> Get()
        {
            return await _dbContext.Messages.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] MessagesModel message)
        {
            _dbContext.Messages.Add(message);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}