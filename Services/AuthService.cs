using ProductCQRS.Model;

namespace ProductCQRS.Services
{
    public class AuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public AdminUser? CheckUser(string username, string password)
        {
            var users = _configuration
                .GetSection("AdminUsers")
                .Get<List<AdminUser>>();

            var user = users.FirstOrDefault(x =>
                x.Username == username &&
                x.Password == password);

            return user;
        }
    }
}