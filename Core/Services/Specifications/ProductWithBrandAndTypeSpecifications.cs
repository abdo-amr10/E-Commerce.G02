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
    public class ProductWithBrandAndTypeSpecifications: Specifications<Product>
    {
        public ProductWithBrandAndTypeSpecifications(int id) : base(p=> p.Id==id)
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);
        }

        public ProductWithBrandAndTypeSpecifications(ProductSpecParams parameters) : base(product=>
        (!parameters.BrandId.HasValue || product.BrandId== parameters.BrandId.Value)
        &&
        (!parameters.TypeId.HasValue || product.TypeId == parameters.TypeId.Value))
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);
            if (parameters.Sort is not null)
            {
                switch (parameters.Sort)
                {
                    case ProductSortingOptions.PriceAsc:
                        SetOrderBy(p => p.Price);
                        break;
                    case ProductSortingOptions.PriceDesc:
                        SetOrderByDescending(p => p.Price);
                        break;
                    case ProductSortingOptions.NameDesc:
                        SetOrderByDescending(p => p.Name);
                        break;
                    default:
                        SetOrderBy(p => p.Name);
                        break;
                }
            }
        }
    }
}
