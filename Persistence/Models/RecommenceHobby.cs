using System.Collections.Generic;

namespace Persistence.Models
{
    public class RecommenceHobby : BaseModel
    {
        public int UserId { get; set; }

        public IList<ProductRecommenceHobby> ProductRecommenceHobbies { get; set; }
    }
}
