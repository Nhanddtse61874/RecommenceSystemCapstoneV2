using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Models
{
    public class RecommenceByBoth : BaseModel
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public IList<ProductRecommenceByBoth> ProductRecommenceByBoths { get; set; }
    }
}
