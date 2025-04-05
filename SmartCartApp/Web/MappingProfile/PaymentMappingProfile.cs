using AutoMapper;
using Web.Models.Domain;
using Web.Models.DTO.PaymentDTOs;

namespace Web.MappingProfile
{
    public class PaymentMappingProfile : Profile
    {
        public PaymentMappingProfile()
        {
            // Payment -> PaymentDTO
            CreateMap<Payment, PaymentDTO>();

            // PaymentDTO -> Payment
            CreateMap<PaymentDTO, Payment>();

            // CreatePaymentDTO -> Payment
            CreateMap<CreatePaymentDTO, Payment>()
                .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Pending"))
                .ForMember(dest => dest.TransactionId, opt => opt.Ignore());
        }
    }
}