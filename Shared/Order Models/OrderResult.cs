using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Order_Models
{
    public record OrderResult
    {
        public Guid Id { get; set; }
        public string UserEmail { get; set; }
        public AddressDto ShippingAddress { get; set; }
        public ICollection<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();   
        public string PaymentStatus { get; set; }
        public string DeliveryMethod { get; set; }
        public decimal SubTotal { get; set; }
        public string PaymentIntentId { get; set; } = string.Empty;
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public decimal Total { get; set; }
    }
}
