using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Models
{
    public class RecommencePrice : BaseModel
    {
        
        public int UserId { get; set; }
        public User User { get; set; }

        public IList<ProductRecommencePrice> ProductRecommencePrices { get; set; }
    }
}
