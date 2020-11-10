using Persistence.Models;
using Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//
namespace Business.Services
{
    public interface IRecommenceHobbyService
    {
        IList<Product> ListDistinct(IList<Product> list1, IList<Product> list2);

        IList<Product> RecommenceByHobbyGetListProduct(Recommence recommence);

        void LoadAndUpdate(RecommenceHobby recommenceHobby);

        void Create(RecommenceHobby newModel);

        void Update(RecommenceHobby modifiedmodel);

        RecommenceHobby CheckAndLoadFromDb(int userLoggingId);

    }
    public class RecommenceHobbyService : ServiceBase, IRecommenceHobbyService
    {
        private readonly IRepository<RecommenceHobby> _repo;
       
        public RecommenceHobbyService(IUnitOfWork unitOfWork,
           IProductService productService, IUserService userService) : base(unitOfWork)
        {
            _repo = _unitOfWork.Repository<RecommenceHobby>();
           
        }

        //-----------------------------------------------------------------------------
        //find product from 2 list of user
        public IList<Product> ListDistinct(IList<Product> list1, IList<Product> list2)
        {
            int count = 0;
            IList<Product> resultList = new List<Product>();
            foreach (var item in list2)
            {
                foreach (var item1 in list1)
                {
                    if(item.Code == item1.Code)
                    {
                        count++;
                    }
                    else
                    {
                        resultList.Add(item);
                    }
                }
            }
            ///////////////
            //param K   /////
            if(count > 3)
            {
                return resultList.Distinct().ToList();
            }
            return null;
;
        }
        //-------------------------------------------------------------------------------------------
        public IList<Product> RecommenceByHobbyGetListProduct(Recommence recommence)
        {
            int cate1 = 0;
            int cate2 = 0;
            int cate3 = 0;
            int cate4 = 0;
            IEnumerable<Product> resultProducts = new List<Product>();
            foreach (var item in recommence.ListDataProductCodes)
            {
                
                var list = ListDistinct(recommence.ProductCodeOfUserLogging.ToList(), item);
                if(list != null)
                {
                    resultProducts = resultProducts.Concat(ListDistinct(recommence.ProductCodeOfUserLogging.ToList(), item));

                }
            }
            var listCategory = recommence.ProductCodeOfUserLogging.Select(x => x.CategoryId).ToList();

            //get 2 selected nearest category.
            if (listCategory.Count >= 4)
            {
                cate1 = listCategory[listCategory.Count - 1];
                cate2 = listCategory[listCategory.Count - 2];
                cate3 = listCategory[listCategory.Count - 3];
                cate4 = listCategory[listCategory.Count - 4];
            }

            //get products in  2 category from listResult
            var result = resultProducts.Where(x => x.CategoryId == cate1 || x.CategoryId == cate2 || x.CategoryId == cate4 || x.CategoryId == cate3);

            //create new product if not existed
            //_productService.CreateListProducts(result.ToList());

            return result.Distinct().ToList();
        }

        //-----------------------------------------------------------------------------
        public void LoadAndUpdate(RecommenceHobby newModel)
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
        //---------------------------------------------------------------------------------------------
        public void  Create(RecommenceHobby newModel)
        {
            _repo.Create(newModel);
            _unitOfWork.Save();
        }
        //---------------------------------------------------------------------------------------------
        public void Update(RecommenceHobby modifiedmodel)
        {
            var updateModel = _repo.Get(x => x.UserId == modifiedmodel.UserId, x => x.ProductRecommenceHobbies);
            updateModel.ProductRecommenceHobbies = modifiedmodel.ProductRecommenceHobbies;
            _repo.Update(updateModel);
            _unitOfWork.Save();
        }
        //--------------------------------------------------------------------------------------------
        public RecommenceHobby CheckAndLoadFromDb(int userLoggingId)
        {
            var result = _repo.Get(x => x.UserId == userLoggingId, x => x.ProductRecommenceHobbies);
            return result;
        }

       

        public class DataModel
        {
            public string Data { get; set; }
        }


    }

}
