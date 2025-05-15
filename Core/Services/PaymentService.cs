global using Product = Domain.Entities.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Domain.Entities.Order_Entities;
using Domain.Exceptions;
using Microsoft.Extensions.Configuration;
using Services.Abstraction;
using Services.Specifications;
using Shared.Dtos;
using Stripe;
using Stripe.Forwarding;

namespace Services
{
    public class PaymentService(
        IBasketRepository basketRepository,
        IConfiguration configuration,
        IUnitOfWork unitOfWork
        ,IMapper mapper) : IPaymentService
    {
        public async Task<BasketDto> CreateOrUpdatePaymentIntentAsync(string basketId)
        {
            // Configure Stripe Api Key Using The Secret Key => AppSettings
            StripeConfiguration.ApiKey = configuration.GetSection("StripeSettings")["SecretKey"];

            // Retrieve Basket By Id
            var basket = await basketRepository.GetBasketAsync(basketId)
                ?? throw new BasketNotFoundException(basketId);

            foreach (var item in basket.Items)
            {
                // Get Product Price From DB ==> Validate The Price that Send By User [Correctly]
                var product = await unitOfWork.GetRepository<Product, int>()
                    .GetByIdAsync(item.Id) ?? throw new ProductNotFoundException(item.Id);

                // Update item.price at The Basket [From Product Table In DB]
                item.Price = product.Price;
            }


            // Check On Delivery Method Is Selected Or Not
            if (!basket.DeliveryMethodId.HasValue)
                throw new Exception("No Delivery Method Was Selected :(");

            // Retrieve Delivery Method From DB
            var deliveryMethod = await unitOfWork.GetRepository<DeliveryMethod, int>()
                .GetByIdAsync(basket.DeliveryMethodId.Value)
                 ?? throw new DeliveryNotFoundException(basket.DeliveryMethodId.Value);

            basket.ShippingPrice = deliveryMethod.Price;

            // Calculate amount (price + shipping)
            var amount = (long)(basket.Items.Sum(i => i.Price * i.Quantity) + basket.ShippingPrice) * 100;
            var service = new PaymentIntentService();

            // If You Want To Create Or Update PaymentIntent
            if (string.IsNullOrWhiteSpace(basket.PaymentIntentId))
            {
                // Create
                var createOptions = new PaymentIntentCreateOptions
                {
                    Amount = amount,
                    Currency = "USD",
                    PaymentMethodTypes = new List<string> { "card" }
                };

                // Calling Stripe API from Service [Create Payment Intent]
                var paymentIntent = await service.CreateAsync(createOptions);

                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else
            {
                // Update
                // 1. Product Price Changed [Admin]
                // 2. User Changes Delivery Method
                // 3. Remove Any Item From Basket

                var updateOptions = new PaymentIntentUpdateOptions
                {
                    Amount = amount,
                };

                await service.UpdateAsync(basket.PaymentIntentId, updateOptions);
            }

            await basketRepository.UpdateBasketAsync(basket); // Price Of Items, Shipping Price

            return mapper.Map<BasketDto>(basket);

        }

        public async Task UpdateOrderPaymentStatus(string request, string header)
        {
            var endPointSecret = configuration.GetSection("StripeSettings")["EndPointSecret"];
                var stripeEvent = EventUtility.ConstructEvent(request,
                    header, endPointSecret , throwOnApiVersionMismatch:false);

            var paymentIntent = stripeEvent.Data.Object as PaymentIntent;

            // Handle the event
            switch (stripeEvent.Type)
            {
                case EventTypes.PaymentIntentPaymentFailed:
                    {
                        await UpdatePaymentFailed(paymentIntent!.Id);
                        break;
                    }

                case EventTypes.PaymentIntentSucceeded:
                    {
                        await UpdatePaymentSucceeded(paymentIntent!.Id);
                        break;
                    }

                default:
                    // Handle other event types
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                    break;
            }


        }

        private async Task UpdatePaymentFailed(string paymentIntentId)
        {
            var orderRepo = unitOfWork.GetRepository<Order, Guid>();
            var order = await orderRepo.GetByIdAsync(new OrderWithPaymentIntentSpecifications(paymentIntentId))
                ?? throw new Exception();

            order.PaymentStatus = OrderPaymentStatus.PaymentFailed;
            orderRepo.Update(order);
            await unitOfWork.SaveChangesAsync();
        }

        private async Task UpdatePaymentSucceeded(string paymentIntentId)
        {
            var orderRepo = unitOfWork.GetRepository<Order, Guid>();
            var order = await orderRepo.GetByIdAsync(new OrderWithPaymentIntentSpecifications(paymentIntentId))
                ?? throw new Exception();

            order.PaymentStatus = OrderPaymentStatus.PaymentReceived;
            orderRepo.Update(order);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
