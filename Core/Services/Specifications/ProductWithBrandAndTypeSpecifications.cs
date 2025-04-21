using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Domain.Entities;

namespace Services.Specifications
{
    public class ProductWithBrandAndTypeSpecifications: Specifications<Product>
    {
        public ProductWithBrandAndTypeSpecifications(int id) : base(p=> p.Id==id)
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);
        }

        public ProductWithBrandAndTypeSpecifications() : base()
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);
        }
    }
}
