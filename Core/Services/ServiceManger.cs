using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Services.Abstraction;
using Shared;

namespace Services
{
    public class ServiceManger : IServiceManger
    {
        private readonly Lazy<IProductService> _productService;
        private readonly Lazy<IBasketService> _basketService;
        private readonly Lazy<IAuthenticationService> _authenticationService;
        private readonly Lazy<IOrderService> _orderService;
        private readonly Lazy<IPaymentService> _paymentService;

        public ServiceManger(IUnitOfWork unitOfWork , IMapper mapper, IBasketRepository basketRepository , UserManager<User> userManager , IOptions<JwtOptions> options , IConfiguration configuration)
        {
            _productService = new Lazy<IProductService>(() => new ProductService(unitOfWork, mapper));
            _basketService = new Lazy<IBasketService>(() => new BasketService(basketRepository, mapper));
            _authenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(userManager , options , mapper));
            _orderService = new Lazy<IOrderService>(()=> new OrderService(mapper , basketRepository , unitOfWork));
            _paymentService = new Lazy<IPaymentService>(()=> new PaymentService(basketRepository, configuration, unitOfWork  , mapper));
            {

            };
        }
        public IProductService ProductService => _productService.Value;

        public IBasketService BasketService => _basketService.Value;

        public IAuthenticationService AuthenticationService => _authenticationService.Value;

        public IOrderService OrderService => _orderService.Value;

        public IPaymentService PaymentService => _paymentService.Value;

    }
}
