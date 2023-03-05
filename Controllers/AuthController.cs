using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using dotnet_app.Data;
using dotnet_app.Dtos.User;
using dotnet_app.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace dotnet_app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static UserModel user = new UserModel();
        private readonly IConfiguration _configuration;

        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public AuthController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        // [HttpPost("register")]
        // public ActionResult<UserModel> Register(UserDto request)
        // {
        //     string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        //     user.Username = request.Username;
        //     user.PasswordHash = passwordHash;

        //     return Ok(user);
        // }

        // [HttpPost("login")]
        // public ActionResult<UserModel> Login(UserDto request)
        // {
        //     if (user.Username != request.Username)
        //     {
        //         return BadRequest("User not found.");
        //     }

        //     if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        //     {
        //         return BadRequest("Wrong password");
        //     }

        //     string token = CreateToken(user);

        //     return Ok(token);
        // }
        [NonAction]

        public string CreateToken(UserModel user)
        {
            List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.Name, user.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddDays(1), signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}