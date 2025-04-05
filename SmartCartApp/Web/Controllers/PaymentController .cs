using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Models.DTO.PaymentDTOs;
using Web.Repositories.Interfaces.IServices;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("process")]
        public async Task<IActionResult> ProcessPayment([FromBody] CreatePaymentDTO request)
        {
            try
            {
                bool result = await _paymentService.ProcessPayment(
                    request
                );

                if (result)
                {
                    return Ok(new { Success = true, Message = "Thanh toán thành công" });
                }
                else
                {
                    return BadRequest(new { Success = false, Message = "Thanh toán thất bại" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = $"Lỗi: {ex.Message}" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPayments()
        {
            try
            {
                var payments = await _paymentService.GetPaymentHistoryAsync();
                return Ok(payments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = $"Lỗi: {ex.Message}" });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentById(int id)
        {
            try
            {
                var payment = await _paymentService.GetPaymentByIdAsync(id);

                if (payment == null)
                {
                    return NotFound(new { Success = false, Message = "Không tìm thấy thanh toán" });
                }

                return Ok(payment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = $"Lỗi: {ex.Message}" });
            }
        }

        [HttpGet("order/{orderId}")]
        public async Task<IActionResult> GetPaymentsByOrderId(int orderId)
        {
            try
            {
                var payments = await _paymentService.GetPaymentsByOrderIdAsync(orderId);
                return Ok(payments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = $"Lỗi: {ex.Message}" });
            }
        }
    }

   
}