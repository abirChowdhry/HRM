using HRM.DTOs;

namespace HRM.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseVM?> Signup(SignupVM signupVM);
        Task<AuthResponseVM?> Login(LoginVM loginVM);
    }
}
