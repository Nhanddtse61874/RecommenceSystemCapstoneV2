using Persistence.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecommenceSystemCapstoneV2.ViewModels
{
    public class RecommenceByHobbyViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public IEnumerable<ProductViewModel> ProductRecommenceHobbies { get; set; }
    }
    public class CreateRecommenceByHobbyViewModel
    {
        public int UserId { get; set; }
        public IEnumerable<ProductViewModel> ProductRecommenceHobbies { get; set; }
    }
}
