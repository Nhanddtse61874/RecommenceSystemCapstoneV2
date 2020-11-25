using Azure.Storage.Queues;
using Business.Services;
using Persistence.Models;
using Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public interface IRecommenceByBothService
    {
        IList<string> GetListProductByBoth(IList<Product> list1, IList<Product> list2);

        void LoadAndUpdate(RecommenceByBoth newModel);

        void Create(RecommenceByBoth newModel);

        void Update(RecommenceByBoth modifiedmodel);

        RecommenceByBoth CheckAndLoadFromDb(int userLoggingId);
    }

    public class RecommenceByBothService : ServiceBase, IRecommenceByBothService
    {
        private readonly IRepository<RecommenceByBoth> _repo;

        public RecommenceByBothService(IUnitOfWork unitOfWork,
            IProductService productService, IUserService userService) : base(unitOfWork)
        {
            _repo = _unitOfWork.Repository<RecommenceByBoth>();
        }

        public IList<string> GetListProductByBoth(IList<Product> list1, IList<Product> list2)
        {
            IList<string> result = new List<string>();
            foreach (var item1 in list1)
            {
                foreach (var item2 in list2)
                {
                    if (item1.Code == item2.Code)
                    {
                        result.Add(item1.Code);
                    }
                }
            }
            return result;
        }

        public void LoadAndUpdate(RecommenceByBoth newModel)
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

        public void Create(RecommenceByBoth newModel)
        {
            _repo.Create(newModel);
            _unitOfWork.Save();
        }

        public void Update(RecommenceByBoth modifiedmodel)
        {
            var updateModel = _repo.Get(x => x.UserId == modifiedmodel.UserId);
            updateModel.ProductRecommenceByBoths = modifiedmodel.ProductRecommenceByBoths;
            _repo.Update(updateModel);
            _unitOfWork.Save();
        }

        public RecommenceByBoth CheckAndLoadFromDb(int userLoggingId)
        {
            var result = _repo.Get(x => x.UserId == userLoggingId, x => x.ProductRecommenceByBoths);
            return result;
        }
    }
}
