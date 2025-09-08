using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Domain.Entities.Order_Entities;

namespace Services.Specifications
{
    internal class OrderWithPaymentIntentSpecifications : Specifications<Order>
    {
        public OrderWithPaymentIntentSpecifications(string paymentIntentId) : base(order => order.PaymentIntentId == paymentIntentId)
        {
        }
    }
}
