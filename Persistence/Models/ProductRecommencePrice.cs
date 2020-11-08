using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Models
{
    public class ProductRecommencePrice : BaseModel
    {
        public ProductRecommencePrice()
        {

        }

        public ProductRecommencePrice(int recommencePriceId, int productId)
        {
            ProductId = productId;
            RecommencePriceId = recommencePriceId;
        }
        public int ProductId { get; set; }

        public int RecommencePriceId { get; set; }

        public Product Product { get; set; }

        public RecommencePrice RecommencePrice { get; set; }
    }
}
