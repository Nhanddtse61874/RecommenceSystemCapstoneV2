using Persistence.Models;
using Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Services
{
    public interface IRecommencePriceService
    {
        double GetAvaragePrice(IList<Product> products);

        IList<Product> Distance(Recommence recommence);

        IList<Product> RecommendByPriceAvarageGetListProducts(Recommence recommence);

        void Create(RecommencePrice newModel);

        void Update(RecommencePrice modifiedmodel);

        void LoadAndUpdate(RecommencePrice recommence);

        RecommencePrice CheckAndLoadFromDb(int userLoggingId);

        IList<Product> ListDistinct(IList<Product> list1, IList<Product> list2);
    }

    public class RecommencePriceService : ServiceBase, IRecommencePriceService
    {
        private readonly IRepository<RecommencePrice> _repo;
        private readonly IProductService _productService;
        private readonly IUserService _userService;
        public RecommencePriceService(IUnitOfWork unitOfWork,
            IProductService productService, IUserService userService) : base(unitOfWork)
        {
            _repo = _unitOfWork.Repository<RecommencePrice>();
            _productService = productService;
            _userService = userService;
        }

        //get avarage price of list products
        public double GetAvaragePrice(IList<Product> list)
        {
            double total = 0;
            foreach (var item in list)
            {
                total = total + item.Price;
            }
            double avaragePrice = total / list.Count;

            return avaragePrice;
        }

        //------------------------------------------------------------------------------------------
        //find distance of avarage price to another users
        public IList<Product> Distance(Recommence recommence)
        {
            IList<double> list = new List<double>();

            //find avarage by user Logging
            var avaragePrice = GetAvaragePrice(recommence.ProductCodeOfUserLogging.ToList());
            Dictionary < List < Product > ,double > dic = new Dictionary<List<Product>, double>();
            //Distance logging user and other users
            foreach (var item in recommence.ListDataProductCodes)
            {
                var avaragePriceData = GetAvaragePrice(item);
                var distance = Math.Sqrt(avaragePrice * avaragePrice - avaragePriceData * avaragePriceData);
                list.Add(distance);
            }

            IEnumerable<Product> listProducts = new List<Product>();
            // take users have nearest average price and return list users 
            for (int i = 0; i < list.Count; i++)
            {
                dic.Add( recommence.ListDataProductCodes[i], list[i]);
            }
            var products = dic.OrderBy(x => x.Value).Select(x => x.Key);

            //take list products orderby distance
            foreach (var item in products)
            {
                if (listProducts.Count() <= 10)
                {
                    var listAfterDistinct = ListDistinct(recommence.ProductCodeOfUserLogging.ToList(), item);
                    listProducts = listProducts.Concat(listAfterDistinct);
                }
                else break;
            }
           
            return listProducts.Distinct().ToList();
        }

        //find products to recommend after select list history purchased
        public IList<Product> ListDistinct(IList<Product> list1, IList<Product> list2)
        {
            int count = 0;
            IList<Product> resultList = new List<Product>();
            foreach (var item in list2)
            {
                foreach (var item1 in list1)
                {
                    if (item.Code == item1.Code)
                    {
                        count++;
                    }
                    else
                    {
                        resultList.Add(item);
                    }
                }
            }
            //param K
            if (count > 5)
            {
                return resultList;
            }

            return null;
           
        }
        //----------------------------------------------------------------------------------------
        //find output from data from client => user and list products to recommend
        public IList<Product> RecommendByPriceAvarageGetListProducts (Recommence recommence)
        {
            int cate1 = 0;
            int cate2 = 0;
            var userLoginAvarePrice = GetAvaragePrice(recommence.ProductCodeOfUserLogging.ToList());

            //get all products of users have same avarage price from list data
            var resultProducts = Distance(recommence).Distinct();

            IList<Recommence> result = new List<Recommence>();

            var listCategory = recommence.ProductCodeOfUserLogging.Select(x => x.CategoryId).ToList();
            //find 2 selected nearest catagory
            if (listCategory.Count >= 2)
            {
                cate1 = listCategory[listCategory.Count - 1];
                cate2 = listCategory[listCategory.Count - 2];
            }
            
            //get final list product after query with conditions
            //var resultAfter = resultProducts.Where(x => x.CategoryId == cate1 || x.CategoryId == cate2);

            return resultProducts.Distinct().ToList();
        }


        //----------------------------------------------------------------------
        // check data of user exist ? if it's existed get it to next method
        public RecommencePrice CheckAndLoadFromDb(int userLoggingId)
        {
            var result = _repo.Get(x => x.User.Id == userLoggingId, x => x.ProductRecommencePrices);
            return result;
        }

        //Load exist data or new for new user logging
        public void LoadAndUpdate(RecommencePrice newModel)
        {

            var result = CheckAndLoadFromDb(newModel.UserId);
            if (result == null)
            {
                Create(newModel);
            }
            else
            {
                Update(newModel);
            }
        }

       
        //--------------------------------------------------------------------------------
        //create new data for new user
        public void Create(RecommencePrice newModel)
        {
            _repo.Create(newModel);
            _unitOfWork.Save();
        }


        //------------------------------------------------------------------------------
        //update existed data for old user
        public void Update(RecommencePrice modifiedmodel)
        {
            var updateModel = _repo.Get(x => x.UserId == modifiedmodel.UserId);
            updateModel.ProductRecommencePrices = modifiedmodel.ProductRecommencePrices;
            _repo.Update(updateModel);
            _unitOfWork.Save();
        }

       
    }
}
