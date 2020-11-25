using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Business;
using Business.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Persistence.DataAccess;
using Persistence.Models;
using RecommenceSystemCapstoneV2.ViewModels;
using static Business.Services.RecommenceHobbyService;

namespace RecommenceSystemCapstoneV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecommenceByHobbyController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRecommenceHobbyService _hobbyService;
        private readonly IProductService _productService;
        private readonly IUserService _userService;
        private readonly ICategoryService _categoryService;
        private readonly IRecommendBestSellerService _bestSellerService;
        private readonly IRecommencePriceService _priceService;


        public RecommenceByHobbyController(IMapper mapper, IRecommenceHobbyService hobbyService,
            IProductService productService,
            IUserService userService,
            ICategoryService categoryService,
            IRecommendBestSellerService bestSellerService,
            IRecommencePriceService priceService
            )
        {
            _priceService = priceService;
            _hobbyService = hobbyService;
            _mapper = mapper;
            _productService = productService;
            _userService = userService;
            _categoryService = categoryService;
            _bestSellerService = bestSellerService;
        }

        [HttpPost]
        public  ActionResult<IList<string>> LoadAndUpdate(Recommence recommence)
        {
           
            var result =_hobbyService.RecommenceByHobbyGetListProduct(recommence);
            var userId = recommence.UserId;

            //create new category if not exist
            foreach (var item in result)
            {
                var categoryId = item.CategoryId;
                if (_categoryService.CheckCategory(categoryId))
                {
                    CreateCategoryViewModel newCate = new CreateCategoryViewModel();
                    newCate.Code = categoryId;
                    _categoryService.Create(_mapper.Map<Category>(newCate));
                }
            }

            //create products if not exist
            var products = _mapper.Map<IEnumerable<CreateProductViewModel>>(result)
                .Select(x => _productService.Create(_mapper.Map<Product>(x)));
            
            //create new user if not exist
            CreateUserViewModel newUser = new CreateUserViewModel();
            newUser.Code = userId;
            _userService.Create(_mapper.Map<User>(newUser));
            
            //select list products'code
            IEnumerable<string> listCode = result.Select(x => x.Code);
            
           //create new model to store database
            CreateRecommenceByHobbyViewModel hobbyViewModel = new CreateRecommenceByHobbyViewModel();
            var list = _mapper.Map<IEnumerable<ProductViewModel>>(products);
            hobbyViewModel.ProductRecommenceHobbies = list;
            hobbyViewModel.UserId = userId;

            //create RecommenceHobbyModel to save database
            var a = _mapper.Map<RecommenceHobby>(hobbyViewModel);
            _hobbyService.LoadAndUpdate(a);
            //----------------------
            //RecommenceByPriceService
            
            
            //get listproducts from model 
            var listProducts = _priceService.RecommendByPriceAvarageGetListProducts(recommence);
            
            
            //check and new category if not exist
            foreach (var item in listProducts)
            {
                if (_categoryService.CheckCategory(item.CategoryId) == false)
                {
                    CreateCategoryViewModel newModel = new CreateCategoryViewModel();
                    newModel.Code = item.CategoryId;
                    _categoryService.Create(_mapper.Map<Category>(newModel));
                }

            }
            //check and create new product if not existed
            var productPrices = _mapper.Map<IEnumerable<CreateProductViewModel>>(listProducts)
              .Select(x => _productService.Create(_mapper.Map<Product>(x)));
            
            
            //new model to store database
            CreateRecommenceByPriceViewModel priceViewModel = new CreateRecommenceByPriceViewModel();
            var listp = _mapper.Map<IEnumerable<ProductViewModel>>(productPrices);
            priceViewModel.ProductRecommencePrices = listp;
            priceViewModel.UserId = userId;

            //create new model or update to savechanges
            var b = _mapper.Map<RecommencePrice>(priceViewModel);
            _priceService.LoadAndUpdate(b);

            return Ok();
        }
    }
}