using Microsoft.AspNetCore.Mvc;
using ProductCQRS.Services;

namespace ProductCQRS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaginationController : ControllerBase
    {
        private readonly PaginationService _paginationService;
        private readonly ILogger<PaginationController> _logger;

        public PaginationController(
            PaginationService paginationService,
            ILogger<PaginationController> logger)
            {
                _paginationService = paginationService;
                _logger = logger;
            }

        [HttpGet("check-pagination")]
        public IActionResult CheckPagination()
        {
            _logger.LogInformation("CheckPagination endpoint called");

            var result = new
            {
                PageNumber = _paginationService.GetPageNumber(),
                ProductsPerPage = _paginationService.GetProductsPerPage()
            };

            _logger.LogInformation("Pagination result: {@Result}", result);

            return Ok(result);
        }
    }
}