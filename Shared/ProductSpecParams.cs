using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public enum ProductSortingOptions
    {
        NameAsc,
        NameDesc,
        PriceAsc,
        PriceDesc,
    }
    public class ProductSpecParams
    {
        public ProductSortingOptions? Sort { get; set; }
        public int? BrandId { get; set; }
        public int? TypeId { get; set; }

    }
}
