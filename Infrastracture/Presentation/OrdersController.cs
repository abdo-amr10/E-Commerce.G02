using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction;
using Shared.Order_Models;

namespace Presentation
{
    [Authorize]
    public class OrdersController(IServiceManger serviceManger) : ApiController
    {
        [HttpPost]
        public async Task<ActionResult<OrderResult>> Create(OrderRequest request)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var order = await serviceManger.OrderService.CreateOrderAsync(request, email);
            return Ok(order);
        }

        [HttpGet]
        public async Task<ActionResult<OrderResult>> GetOrders()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var order = await serviceManger.OrderService.GetOrderByEmailAsync(email);
            return Ok(order);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderResult>> GetOrders(Guid id)
        {
            var order = await serviceManger.OrderService.GetOrderByIdAsync(id);
            return Ok(order);
        }

        [AllowAnonymous]

        [HttpGet("DeliveryMethod")]
        public async Task<ActionResult<DeliveryMethodResult>> GetDeliveryMethod()
        {
            var methods = await serviceManger.OrderService.GetDeliveryMethodAsync();
            return Ok(methods);
        }
    }
}
