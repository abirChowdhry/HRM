using HRM.DTOs;
using HRM.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("Signup")]
        public async Task<IActionResult> Signup(SignupVM signupVM)
        {
            var data = await _authService.Signup(signupVM);
            if (data == null)
            {
                return BadRequest(new MessageHelper { StatusCode = 400, Message = "Signup failed. Use a unique email and a password of at least 6 characters." });
            }

            return Ok(data);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            var data = await _authService.Login(loginVM);
            if (data == null)
            {
                return Unauthorized(new MessageHelper { StatusCode = 401, Message = "Invalid email or password." });
            }

            return Ok(data);
        }
    }
}
