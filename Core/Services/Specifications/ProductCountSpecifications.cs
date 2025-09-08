using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Domain.Entities;
using Shared;

namespace Services.Specifications
{
    public class ProductCountSpecifications : Specifications<Product>
    {
        public ProductCountSpecifications(ProductSpecParams parameters) : base(product =>
                        (!parameters.BrandId.HasValue || product.BrandId == parameters.BrandId.Value)
                        &&
                        (!parameters.TypeId.HasValue || product.TypeId == parameters.TypeId.Value)
                        &&
                        (string.IsNullOrWhiteSpace(parameters.Search) || product.Name.ToLower().Contains(parameters.Search.ToLower().Trim())))
        {
            


        }

    }
}
