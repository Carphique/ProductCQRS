namespace ProductCQRS.Services
{
    public class ProductService
    {
        private readonly IConfiguration _configuration;
        public ProductService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public int GetMaxProductsPerPage()
        {
            return _configuration
                .GetValue<int>("AppSettings:MaxProductsPerPage");   
        }
        
    }
}
