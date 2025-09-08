using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Order_Entities
{
    public class ProductInOrderItem
    {
        public ProductInOrderItem()
        {
            
        }
        public ProductInOrderItem(int productId, string pictureUrl, string productName)
        {
            ProductId = productId;
            PictureUrl = pictureUrl;
            ProductName = productName;
        }

        public int ProductId { get; set; }
        public string PictureUrl { get; set; }
        public string ProductName { get; set; }

    }
}
