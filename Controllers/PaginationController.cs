using Microsoft.AspNetCore.Mvc;
using ProductCQRS.Services;

namespace ProductCQRS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaginationController : ControllerBasex
    {
        private readonly PaginationService _paginationService;

        public PaginationController(PaginationService paginationService)
        {
            _paginationService = paginationService;
        }

        [HttpGet("check-pagination")]
        public IActionResult CheckPagination()
        {
            return Ok(new
            {
                PageNumber = _paginationService.GetPageNumber(),
                ProductsPerPage = _paginationService.GetProductsPerPage()
            });
        }
    }
}