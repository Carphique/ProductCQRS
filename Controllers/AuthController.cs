using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ProductCQRS.Model;
using ProductCQRS.Profiles;

namespace ProductCQRS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("check-role")]
        public IActionResult CheckRole([FromBody] AdminUser login)
        {
            var users = _configuration
                .GetSection("AdminUsers")
                .Get<List<AdminUser>>();

            var user = users.FirstOrDefault(x =>
                x.Username == login.Username &&
                x.Password == login.Password);

            if (user == null)
            {
                return Unauthorized("Invalid username or password");
            }

            return Ok(new
            {
                Username = user.Username,
                Role = user.Role
            });
        }
    }
}