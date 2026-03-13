using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductCQRS.CQRS.Command;
using ProductCQRS.CQRS.Query;

namespace ProductCQRS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllProductsQueryRequest());
            return Ok(result);
        }

        [HttpPost("create-product")]
        public async Task<IActionResult> Create([FromBody] CreateProductCommandRequest request)
        {
            var result = await _mediator.Send(request);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}