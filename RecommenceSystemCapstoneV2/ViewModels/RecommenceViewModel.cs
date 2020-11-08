using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecommenceSystemCapstoneV2.ViewModels
{
    public class RecommenceViewModel
    {
        public int UserCode { get; set; }

        public ICollection<ProductViewModel> ProductCodeOfUserLogging { get; set; }

        public IList<List<ProductViewModel>> ListDataProductCodes { get; set; }
    }
}
