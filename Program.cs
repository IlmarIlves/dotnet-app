using System.Text;
using dotnet_app.Data;
using dotnet_app.Dtos.User;
using dotnet_app.models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Needed for DataBase
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRazorPages();

// builder.Services.AddScoped<ICharacterService, CharacterService>();


builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer();

builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
    });

builder.Services.AddSession(options =>
{
    options.Cookie.Name = "MyApp.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "account",
        pattern: "/Account/{action=login}/{id?}",
        defaults: new { controller = "AccountController" });
});

app.MapRazorPages();

app.UseAuthorization();

app.UseSession();

app.MapControllers();

// app.MapPost("/register1", async (DataContext context, UserDto request) =>
// {
//     string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

//     UserModel user = new UserModel();
//     user.Username = request.Username;
//     user.PasswordHash = passwordHash;

//     context.Users.Add(user);
//     await context.SaveChangesAsync();
//     return Results.Ok(await context.Users.ToListAsync());
// });


app.Run();
