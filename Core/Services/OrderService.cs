using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Domain.Entities.Order_Entities;
using Domain.Exceptions;
using Services.Abstraction;
using Shared.Order_Models;

namespace Services
{
    internal class OrderService
        (
        IMapper mapper,
        IBasketRepository basketRepository,
        IUnitOfWork unitOfWork
        ) : IOrderService
    {
        public async Task<OrderResult> CreateOrderAsync(OrderRequest request, string userEmail)
        {
            var address = mapper.Map<ShippingAddress>( request.ShippingAddress);

            var basket = await basketRepository.GetBasketAsync(request.BasketId) 
                ?? throw new BasketNotFoundException(request.BasketId);
            var orderItems = new List<OrderItem>();

            foreach (var item in basket.Items)
            {
                var product = await unitOfWork.GetRepository<Product , int>().GetByIdAsync(item.Id)
                    ?? throw new ProductNotFoundException(item.Id);

                orderItems.Add(CreateOrderItem(item, product));
            }

            var deliveryMethod = await unitOfWork.GetRepository<DeliveryMethod, int>()
                                           .GetByIdAsync(request.DeliveryMethodId)
                                           ?? throw new DeliveryNotFoundException(request.DeliveryMethodId) ;

            var subTotal = orderItems.Sum(item => item.Price * item.Quantity);

            var order = new Order(userEmail, address, orderItems, deliveryMethod, subTotal);

            // 6. Save To DB
            await unitOfWork.GetRepository<Order, Guid>()
                .AddAsync(order);

            await unitOfWork.SaveChangesAsync();

            // Map<Order, OrderResult> & Return
            return mapper.Map<OrderResult>(order);


        }

        private OrderItem CreateOrderItem(BasketItems item, Product product)
        => new OrderItem(new ProductInOrderItem(product.Id, product.Name, product.PictureUrl),
            item.Price, item.Quantity);

        public Task<IEnumerable<DeliveryMethodResult>> GetDeliveryMethodAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<OrderResult>> GetOrderByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<OrderResult> GetOrderByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
