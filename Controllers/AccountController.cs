using System.Threading.Tasks;
using dotnet_app.Dtos.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using dotnet_app.models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using dotnet_app.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace dotnet_app.Controllers
{
    [Route("[controller]")]
    public class AccountController : Controller
    {
        public static UserModel user = new UserModel();
        private readonly IConfiguration _configuration;
        private readonly DataContext _context;

        public AccountController(IConfiguration configuration, DataContext context)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("registerMain")]
        public async Task<IActionResult> Register(UserDto request)
        {
            UserModel user = new UserModel();

            // Generate a random salt value
            var salt = GenerateSalt();

            user.Username = request.Username;
            user.PasswordSalt = salt;
            user.PasswordHash = GenerateHash(request.Password, salt);


            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(UserDto request)
        {

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);


            if (user == null)
            {
                return BadRequest("User not found.");
            }

            if (!VerifyPassword(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Wrong password");
            }


            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var identity = new ClaimsIdentity(claims, "MyAuthType");

            var principal = new ClaimsPrincipal(identity);
            // Authenticate the user and create a ClaimsIdentity...

            // Store the identity in the session...
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            // Redirect to the home page...
            return RedirectToAction("Index", "Home");
        }

        [NonAction]
        public byte[] GenerateSalt()
        {
            var salt = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        [NonAction]
        public static byte[] GenerateHash(string password, byte[] salt)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] saltedPassword = new byte[passwordBytes.Length + salt.Length];
            Buffer.BlockCopy(passwordBytes, 0, saltedPassword, 0, passwordBytes.Length);
            Buffer.BlockCopy(salt, 0, saltedPassword, passwordBytes.Length, salt.Length);

            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(saltedPassword);
            }
        }

        [NonAction]
        public static bool VerifyPassword(string password, byte[] passwordHash, byte[] salt)
        {
            byte[] newHash = GenerateHash(password, salt);
            return SlowEquals(passwordHash, newHash);
        }

        public static bool SlowEquals(byte[] a, byte[] b)
        {
            uint diff = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
                diff |= (uint)(a[i] ^ b[i]);
            return diff == 0;
        }

    }
}