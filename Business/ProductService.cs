using Persistence.Models;
using Persistence.Repositories;
using System.Collections.Generic;

namespace Business.Services
{
    public interface IProductService
    {
        //void CreateListProducts(IList<Product> products);

        Product CheckExistProduct(string code);

        Product Create(Product product);

        IList<Product> GetByProductCodes(IList<string> productCodes);
    }
    public class ProductService : ServiceBase, IProductService
    {
        private readonly IRepository<Product> _productRepo;

        

        public ProductService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _productRepo = _unitOfWork.Repository<Product>();
        }
        //public void CreateListProducts(IList<Product> products)
        //{
        //    foreach (var item in products)
        //    {
        //        if (CheckExistProduct(item.Code) == null)
        //        {
        //            _productRepo.Create(item);
        //        }
        //    }  
        //    _unitOfWork.Save();
        //}

        public Product CheckExistProduct(string code)
            => _productRepo.Get(filter : x => x.Code == code);

        public Product Create(Product product)
        {
            var temp = CheckExistProduct(product.Code);
            if (temp == null)
            {
                _productRepo.Create(product);
                _unitOfWork.Save();
            }
            return temp;
           
        }

        public IList<Product> GetByProductCodes(IList<string> productCodes)
        {
            var result = new List<Product>();

            foreach (var item in productCodes)
            {
                result.Add(_productRepo.Get(x => x.Code == item));
            }
            return result;
        }
    }
}
