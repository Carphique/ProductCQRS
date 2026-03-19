using Microsoft.AspNetCore.Mvc;
using ProductCQRS.Model;

namespace ProductCQRS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IConfiguration configuration, ILogger<AuthController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost("check-role")]
        public IActionResult CheckRole([FromBody] AdminUser login)
        {
            _logger.LogInformation("CheckRole called for user: {Username}", login.Username);

            var users = _configuration
                .GetSection("AdminUsers")
                .Get<List<AdminUser>>();

            var user = users.FirstOrDefault(x =>
                x.Username == login.Username &&
                x.Password == login.Password);

            if (user == null)
            {
                _logger.LogWarning("Unauthorized login attempt: {Username}", login.Username);
                return Unauthorized("Invalid username or password");
            }

            _logger.LogInformation("User {Username} logged in with role {Role}", user.Username, user.Role);

            return Ok(new
            {
                Username = user.Username,
                Role = user.Role
            });
        }
    }
}