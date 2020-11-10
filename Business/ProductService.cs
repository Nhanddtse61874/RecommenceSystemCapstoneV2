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

        Product Get(int productId);
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
            => _productRepo.Get(filter : x => x.Code == code, x => x.ProductRecommencePrices, x=> x.ProductRecommenceHobbies);

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

        public Product Get(int productId)
        {
            var product = _productRepo.Get(x => x.Id == productId);
            return product;
        }
        public IList<Product> GetByProductCodes(IList<string> productCodes)
        {
            var result = new List<Product>();

            foreach (var item in productCodes)
            {
                result.Add(_productRepo.Get(x => x.Code == item, x => x.ProductRecommenceHobbies, x => x.ProductRecommencePrices));
            }
            return result;
        }
    }
}
