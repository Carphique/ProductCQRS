using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ProductCQRS.CQRS.Command;
using ProductCQRS.CQRS.Query;
using ProductCQRS.Profiles;

namespace ProductCQRS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly AppSettingsProfile _appSettings;
        private readonly ILogger<ProductController> _logger;

        public ProductController(
            IMediator mediator,
            IOptions<AppSettingsProfile> options,
            ILogger<ProductController> logger)
        {
            _mediator = mediator;
            _appSettings = options.Value;
            _logger = logger;
        }

        [HttpGet("config")]
        public ActionResult GetConfig()
        {
            _logger.LogInformation("GetConfig called");

            return Ok(new
            {
                AppName = _appSettings.ApplicationName,
                MaxProducts = _appSettings.MaxProductsPerPage
            });
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("GetAll products called");

            var result = await _mediator.Send(new GetAllProductsQueryRequest());

            _logger.LogInformation("Products returned: {Count}", result.Data?.Count);

            return Ok(result);
        }

        [HttpPost("create-product")]
        public async Task<IActionResult> Create([FromBody] CreateProductCommandRequest request)
        {
            _logger.LogInformation("Create product called: {@Request}", request);

            var result = await _mediator.Send(request);

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Product creation failed: {Message}", result.Message);
                return BadRequest(result);
            }

            _logger.LogInformation("Product created successfully");

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            _logger.LogInformation("Get product by id endpoint called: {Id}", id);

            var result = await _mediator.Send(new GetProductByIdQueryRequest(id));

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Product not found: {Id}", id);
                return NotFound(result);
            }

            _logger.LogInformation("Product returned: {Id}", id);

            return Ok(result);
        }
    }
}