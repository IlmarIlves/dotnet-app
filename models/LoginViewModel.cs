namespace dotnet_app.Pages
{
    public class LoginViewModel
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}