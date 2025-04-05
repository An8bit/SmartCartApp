using AutoMapper;
using SmartCartApp.Models.Payment;
using Web.Models.Domain;
using Web.Models.DTO.PaymentDTOs;
using Web.Repositories.Contracts;
using Web.Repositories.Interfaces.IServices;
using Web.Stratery;

namespace Web.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        public readonly IMapper _mapper;
        public PaymentService(IPaymentRepository paymentRepository, IMapper mapper)
        {
            _paymentRepository = paymentRepository;
            _mapper = mapper;
        }
        public Task<Payment> GetPaymentByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Payment>> GetPaymentHistoryAsync()
        {
            var payments = _paymentRepository.GetAllAsync();
            return payments;
        }

        public Task<IEnumerable<Payment>> GetPaymentsByOrderIdAsync(int orderId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ProcessPayment(CreatePaymentDTO createPaymentDTO)
        {
            // Sử dụng Strategy Pattern để xử lý thanh toán
            IPaymentStrategy paymentStrategy;
            switch (createPaymentDTO.PaymentMethod.ToLower())
            {
                case "momo":
                    if (!createPaymentDTO.PaymentDetails.ContainsKey("phoneNumber") ||
                        !createPaymentDTO.PaymentDetails.ContainsKey("otp"))
                    {
                        throw new ArgumentException("Thiếu thông tin thanh toán MoMo");
                    }

                    paymentStrategy = new MoMoPayment(
                        createPaymentDTO.PaymentDetails["phoneNumber"],
                        createPaymentDTO.PaymentDetails["otp"]);
                    break;

                case "banking":
                    if (!createPaymentDTO.PaymentDetails.ContainsKey("bankName") ||
                        !createPaymentDTO.PaymentDetails.ContainsKey("accountNumber"))
                    {
                        throw new ArgumentException("Thiếu thông tin thanh toán Banking");
                    }

                    paymentStrategy = new BankingPayment(
                        createPaymentDTO.PaymentDetails["bankName"],
                        createPaymentDTO.PaymentDetails["accountNumber"]);
                    break;

                default:
                    throw new NotSupportedException($"Phương thức thanh toán '{createPaymentDTO.PaymentMethod}' không được hỗ trợ.");
            }



            var paymentContext = new PaymentContext(paymentStrategy);
            bool paymentResult = paymentContext.ProcessPayment(createPaymentDTO.Amount);

            var paymentDTO = new PaymentDTO
            {
                OrderId = createPaymentDTO.OrderId,
                PaymentMethod = paymentStrategy.GetPaymentMethod(),
                Amount = createPaymentDTO.Amount,
                Status = paymentResult ? "Success" : "Failed",
                PaymentDate = DateTime.UtcNow,
                TransactionId = paymentResult ? Guid.NewGuid().ToString() : null // Giả sử tạo ID giao dịch nếu thanh toán thành công
            };

            var payment = _mapper.Map<Payment>(paymentDTO);
             await _paymentRepository.AddPaymentAsync(payment);
            return true;
        }
    }
}
