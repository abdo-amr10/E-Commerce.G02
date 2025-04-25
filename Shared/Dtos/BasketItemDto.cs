using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public record BasketItemDto
    {
        public int Id { get; init; }
        public string ProductName { get; init; }
        public string PictureUrl { get; init; }
        public string Category { get; init; }
        public string Brand { get; init; }

        [Range(1, 99, ErrorMessage = "Quantity Must Be At Least 1 Item")]
        public int Quantity { get; init; }

        [Range(1, double.MaxValue, ErrorMessage ="Price Must Be More Than 0 !")]
        public decimal Price { get; init; }
    }
}
