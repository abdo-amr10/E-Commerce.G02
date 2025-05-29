using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Order_Models;

namespace Services.Abstraction
{
    public interface IOrderService
    {
        // Get Order By Id => OrderResult (Guid id)
        public Task<OrderResult> GetOrderByIdAsync(Guid id);

        // Get All Orders For User By His Email => IEnumerable<OrderResult> (string email)
        public Task<IEnumerable<OrderResult>> GetOrderByEmailAsync(string email);

        // Create Order => OrderResult (OrderRequest(BasketId, ShippingAddress), string email)
        public Task<OrderResult> CreateOrderAsync(OrderRequest request, string userEmail);

        // Get All Delivery Methods => IEnumerable<DeliveryMethodResult>
        public Task<IEnumerable<DeliveryMethodResult>> GetDeliveryMethodAsync();

    }
}
