namespace HRM.DTOs
{
    public class SignupVM
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginVM
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class AuthResponseVM
    {
        public long UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
