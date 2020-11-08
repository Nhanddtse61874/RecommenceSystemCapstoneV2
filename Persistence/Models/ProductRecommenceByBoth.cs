using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Models
{
    public class ProductRecommenceByBoth : BaseModel
    {
        public ProductRecommenceByBoth()
        {
        }

        public ProductRecommenceByBoth(int recommenceByBothId, int productId)
        {
            ProductId = productId;
            RecommenceByBothId = recommenceByBothId;
        }
        public int ProductId { get; set; }

        public int RecommenceByBothId { get; set; }

        public Product Product { get; set; }

        public RecommenceByBoth RecommenceByBoth { get; set; }
    }
}
