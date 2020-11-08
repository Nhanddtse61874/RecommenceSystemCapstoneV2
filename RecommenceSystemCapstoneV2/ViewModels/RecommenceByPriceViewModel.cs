using Persistence.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecommenceSystemCapstoneV2.ViewModels
{
    public class RecommenceByPriceViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public IList<ProductViewModel> ProductRecommencePrices { get; set; }
    }

    public class CreateRecommenceByPriceViewModel
    {
        public int UserId { get; set; }
        public IEnumerable<ProductViewModel> ProductRecommencePrices { get; set; }
    }
}
