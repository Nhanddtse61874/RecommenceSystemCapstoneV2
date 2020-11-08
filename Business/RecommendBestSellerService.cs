using Persistence.Models;
using Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Services
{
    public interface IRecommendBestSellerService
    {
        IList<string> RecommendByBestSeller(Recommence model);
    }
    public class RecommendBestSellerService : ServiceBase, IRecommendBestSellerService
    {
        private readonly IRepository<Recommence> _repo;

        public RecommendBestSellerService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _repo = _unitOfWork.Repository<Recommence>();
        }
        public IList<string> RecommendByBestSeller(Recommence model)
        {
            IEnumerable<Product> listProducts = new List<Product>();
            foreach (var item in model.ListDataProductCodes)
            {
                listProducts = listProducts.Concat(item);
            }
            var list = from Product in listProducts
                       group Product by Product into groupProduct
                       select new
                       {
                           product = groupProduct.Key,
                           count = groupProduct.Count()
                       };
            var result = list.OrderByDescending(x => x.count).Select(x => x.product.Code);
            
            return result.ToList();
            ///
        }
    }
}
