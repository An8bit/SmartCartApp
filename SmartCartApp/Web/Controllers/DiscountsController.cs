using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Models.DTO;
using Web.Repositories.Interfaces.IServices;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountsController : ControllerBase
    {
     private readonly IDiscountService _discountService;

        public DiscountsController(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DiscountDTO>>> GetDiscounts()
        {
            return Ok(await _discountService.GetAllDiscountsAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DiscountDTO>> GetDiscount(int id)
        {
            var discount = await _discountService.GetDiscountByIdAsync(id);

            if (discount == null)
                return NotFound();

            return Ok(discount);
        }

        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<DiscountDTO>>> GetActiveDiscounts()
        {
            return Ok(await _discountService.GetActiveDiscountsAsync());
        }

        [HttpGet("product/{productId}")]
        public async Task<ActionResult<IEnumerable<DiscountDTO>>> GetDiscountsByProduct(int productId)
        {
            return Ok(await _discountService.GetDiscountsByProductIdAsync(productId));
        }

        [HttpPost]
        public async Task<ActionResult<DiscountDTO>> CreateDiscount(CreateDiscountDTO discountDto)
        {
            var createdDiscount = await _discountService.CreateDiscountAsync(discountDto);
            return CreatedAtAction(nameof(GetDiscount), new { id = createdDiscount.Id }, createdDiscount);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDiscount(int id, UpdateDiscountDTO discountDto)
        {
            var updatedDiscount = await _discountService.UpdateDiscountAsync(id, discountDto);

            if (updatedDiscount == null)
                return NotFound();

            return Ok(updatedDiscount);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDiscount(int id)
        {
            var result = await _discountService.DeleteDiscountAsync(id);

            if (!result)
                return NotFound();

            return NoContent();
        }

    }
}