
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Models
{
    public class Product :BaseModel
    {
        public string Code { get; set; }

        public int CategoryId { get; set; }

        public double Price { get; set; }


        public ICollection<ProductRecommenceHobby> ProductRecommenceHobbies { get; set; }

        public ICollection<ProductRecommencePrice> ProductRecommencePrices { get; set; }

    }
}
