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

        public ProductWithBrandAndTypeSpecifications(string? sort, int? brandId, int? typeId) : base(product=>
        (!brandId.HasValue || product.BrandId==brandId.Value)
        &&
        (!typeId.HasValue || product.TypeId==typeId.Value))
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);
            if (!string.IsNullOrWhiteSpace(sort))
            {
                switch (sort.ToLower().Trim())
                {
                    case "priceasc":
                        SetOrderBy(p => p.Price);
                        break;
                    case "pricedesc":
                        SetOrderByDescending(p => p.Price);
                        break;
                    case "namedesc":
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
