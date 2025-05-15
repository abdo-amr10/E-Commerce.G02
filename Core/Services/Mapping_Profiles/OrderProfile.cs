global using UserAddress = Domain.Entities.Identity.Address;
global using ShippingAddress = Domain.Entities.Order_Entities.Address;
using Shared.Order_Models;
using AutoMapper;
using Domain.Entities.Order_Entities;

namespace Services.Mapping_Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<UserAddress, AddressDto>().ReverseMap();

            CreateMap<ShippingAddress, AddressDto>().ReverseMap();

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductId, options => options.MapFrom(s => s.Product.ProductId))
                .ForMember(d => d.ProductName, options => options.MapFrom(s => s.Product.ProductName))
                .ForMember(d => d.PictureUrl, options => options.MapFrom(s => s.Product.PictureUrl));

            CreateMap<Order, OrderResult>()
                .ForMember(d => d.PaymentStatus, options => options.MapFrom(s => s.PaymentStatus.ToString()))
                .ForMember(d => d.DeliveryMethod, options => options.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(d => d.Total, options => options.MapFrom(s => s.SubTotal + s.DeliveryMethod.Price));


            CreateMap<DeliveryMethod, DeliveryMethodResult>()
                  .ForMember(d => d.Cost, options => options.MapFrom(s => s.Price ));
        }
    }
}
