using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Models
{
    public class ProductRecommenceHobby : BaseModel
    {
        public ProductRecommenceHobby()
        {
        }

        public ProductRecommenceHobby(int recommenceHobbyId, int productId)
        {
            ProductId = productId;
            RecommenceHobbyId = recommenceHobbyId;
        }
        public int ProductId { get; set; }

        public int RecommenceHobbyId { get; set; }

        public Product Product { get; set; }

        public RecommenceHobby RecommenceHobby { get; set; }
    }   
}
