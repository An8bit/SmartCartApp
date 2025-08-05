using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Models.DTO.ProductDTOs;
using Web.Repositories.Contracts;
using Web.Repositories.Interfaces.IServices;
using Web.Repositories.Interfaces.Service;
using Web.Services;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IDiscountService _discountService;
        private readonly IPriceService _priceService;

        public ProductsController(IProductService productService, IDiscountService discountService, IPriceService priceService)
        {
            _productService = productService;
            _discountService = discountService;
            _priceService = priceService;
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetProducts([FromQuery] ProductFilterOptions options)
        {
            var products = await _productService.GetFilteredProductsAsync(options);
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();

            // Get price information with discounts
            var priceInfo = await _priceService.CalculateProductPriceAsync(id);
            
            // tính trả về 
            product.OriginalPrice = priceInfo.OriginalPrice;
            product.DiscountedPrice = priceInfo.FinalPrice;
            product.DiscountPercentage = priceInfo.DiscountPercentage;

            return Ok(product);
        }

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetProductsByCategory(int categoryId)
        {
            var products = await _productService.GetProductsByCategoryAsync(categoryId);
            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto productDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdProduct = await _productService.CreateProductAsync(productDto);
            return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.ProductId }, createdProduct);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDto productDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _productService.UpdateProductAsync(id, productDto);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                await _productService.DeleteProductAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }
        [HttpGet("{id}/price")]
        public async Task<ActionResult<ProductPriceInfoViewModel>> GetProductPriceWithDiscount(int id)
        {
            try
            {
                var priceInfo = await _priceService.CalculateProductPriceAsync(id);
                return Ok(priceInfo);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }
        [HttpGet("discounted")]
        public async Task<ActionResult<IEnumerable<ProductPriceInfoViewModel>>> GetDiscountedProducts()
        {
            var discountedProducts = await _priceService.GetProductsOnSaleAsync();
            return Ok(discountedProducts);
        }
    }
}
