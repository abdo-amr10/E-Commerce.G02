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

        private const int MaxPageSize = 10;

        private const int DefaultPageSize = 10;
        public int PageIndex { get; set; } = 1;
        private int pageSize = DefaultPageSize;

        public int PageSize
        {
            get => pageSize;
            set => pageSize = value > MaxPageSize? MaxPageSize : value; 
        }


    }
}
