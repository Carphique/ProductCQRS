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

        public ProductController(IMediator mediator, IOptions<AppSettingsProfile> options)
        {
            _mediator = mediator;
            _appSettings = options.Value;
            
        }

        [HttpGet("config")]
        public ActionResult GetConfig() {
            return Ok(new
            {
                AppName = _appSettings.ApplicationName,
                MaxProduts = _appSettings.MaxProductsPerPage,
            });
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