using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecommenceSystemCapstoneV2.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }
     
        public string Code { get; set; }

        public int CategoryId { get; set; }

        public double Price { get; set; }

    }

    public class CreateProductViewModel { 
        
        public string Code { get; set; }

        public int CategoryId { get; set; }

        public double Price { get; set; }

    }
}
