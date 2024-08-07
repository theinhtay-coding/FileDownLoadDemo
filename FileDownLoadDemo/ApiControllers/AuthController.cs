using FileDownLoadDemo.Services;
using JWTAuthDemo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FileDownLoadDemo.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            var user = _authService.Authenticate(model.Email, model.Password);

            if (user == null)
                return Unauthorized(new { message = "Email or password is incorrect" });

            var token = _authService.GenerateJwtToken(user);

            return Ok(new { Token = token });
        }
    }
}
