using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace dotnet_app.Pages
{
    public class Calendar : PageModel
    {
        private readonly ILogger<Calendar> _logger;

        public Calendar(ILogger<Calendar> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}