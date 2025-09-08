using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Domain.Entities.Order_Entities;

namespace Services.Specifications
{
    internal class OrderWithIncludeSpecifications : Specifications<Order>
    {

        public OrderWithIncludeSpecifications(Guid id) : base(o=> o.Id == id) 
        {
            AddInclude(p => p.DeliveryMethod);
            AddInclude(p => p.OrderItems);
        }

        public OrderWithIncludeSpecifications(string email) : base(o=> o.UserEmail == email) 
        {
            AddInclude(p => p.DeliveryMethod);
            AddInclude(p => p.OrderItems);
            SetOrderBy(P => P.OrderDate);
        }
    }
}
