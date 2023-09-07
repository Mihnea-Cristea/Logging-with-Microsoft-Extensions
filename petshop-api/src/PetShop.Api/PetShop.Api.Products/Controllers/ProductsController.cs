using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetShop.Api.Entities;
using PetShop.Api.Model;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace PetShop.Api.Products.Controllers
{
    public class ProductsController : ApiController
    {
        private readonly PetShopContext _context;
        private readonly IConfiguration _config;
        private readonly ILogger _logger;

        public ProductsController(PetShopContext context, IConfiguration config, ILogger<ProductsController> logger)
        {
            _context = context;
            _config = config;
            _logger = logger;
        }

        [HttpGet]
        [Route("products")]
        public IHttpActionResult Get()
        {
            _logger.LogDebug("* GET /products called");

            var products = _context.Products.ToList();
            FixImageUrl(products);

            _logger.LogInformation($"** Returning {products.Count} products");
            return Ok(products);
        }

        [HttpGet]
        [Route("products/{productId}")]
        public IHttpActionResult GetById(string productId)
        {
            _logger.LogDebug($"* GET /products/{productId} called");

            var product = _context.Products.FirstOrDefault(x => x.ProductId == productId);
            if (product == null)
            {
                _logger.LogInformation($"** Product not found - productId: {productId}");
                return NotFound();
            }

            FixImageUrl(new List<Product> { product });

            _logger.LogInformation($"** Returning product - productId: {productId}");
            return Ok(product);
        }

        [HttpGet]
        [Route("products/category/{categoryId}")]
        public IHttpActionResult GetByCategory(string categoryId)
        {
            _logger.LogDebug($"* GET /products/{categoryId} called");

            var products = _context.Products.Where(x => x.CategoryId == categoryId).ToList();
            if (!products.Any())
            {
                _logger.LogInformation($"** Products not found - categoryId: {categoryId}");
                return NotFound();
            }

            FixImageUrl(products);

            _logger.LogInformation($"** Returning {products.Count} product(s) - categoryId: {categoryId}");
            return Ok(products);
        }

        private void FixImageUrl(List<Product> products)
        {
            var urlRoot = $"{_config["PetShop:Web:Scheme"]}://{_config["PetShop:Web:Domain"]}";
            products.ForEach(x => x.ImageUrl = $"{urlRoot}{x.ImageUrl.TrimStart('~')}");
        }
    }
}
