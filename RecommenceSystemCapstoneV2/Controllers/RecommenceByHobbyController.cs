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
       

        public RecommenceByHobbyController(IMapper mapper, IRecommenceHobbyService hobbyService,
            IProductService productService,
            IUserService userService,
            ICategoryService categoryService,
            IRecommendBestSellerService bestSellerService
            )
        {
            _hobbyService = hobbyService;
            _mapper = mapper;
            _productService = productService;
            _userService = userService;
            _categoryService = categoryService;
            _bestSellerService = bestSellerService;
        }

        [HttpPost]
        public  ActionResult<IList<string>> LoadAndUpdate([FromForm]string data)
        {
            //create queue
            //QueueService queue = new QueueService();
            //queue.CreateQueue(data);
            
            Recommence recommence = new Recommence();
            try
            {
                 recommence =  JsonSerializer.Deserialize<Recommence>(data);
                if (!TryValidateModel(recommence))
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error");
            }

            //var finalResult = _hobbyService.CheckAndLoadFromDb(recommence.UserId);
            //var finalListCodeProducts = finalResult.ProductRecommenceHobbies.Select(x => x.Product.Code);
            //if (finalResult == null)
            //{
            //    finalListCodeProducts = ...... BestSeller
            //}
            //var x = _bestSellerService.RecommendByBestSeller(recommence);
            
            //get product list and userId
            var result =_hobbyService.RecommenceByHobbyGetListProduct(recommence);
            var userId = recommence.UserId;

            //create new category if not exist
            foreach (var item in result)
            {
                var categoryId = item.CategoryId;
                if (_categoryService.CheckCategory(categoryId))
                {
                    CategoryViewModel newCate = new CategoryViewModel();
                    newCate.Code = categoryId;
                    _categoryService.Create(_mapper.Map<Category>(newCate));
                }
            }

            //create products if not exist
            var products = _mapper.Map<IEnumerable<CreateProductViewModel>>(result)
                .Select(x => _productService.Create(_mapper.Map<Product>(x)));
            
            //create new user if not exist
            UserViewModel newUser = new UserViewModel();
            newUser.Code = userId;
            _userService.Create(_mapper.Map<User>(newUser));
            
            //select list products'code
            IEnumerable<string> listCode = result.Select(x => x.Code);
            
            //create RecommenceHobbyModel to save database
            CreateRecommenceByHobbyViewModel hobbyViewModel = new CreateRecommenceByHobbyViewModel();
            var list = _mapper.Map<IEnumerable<ProductViewModel>>(products);
            hobbyViewModel.ProductRecommenceHobbies = list;
            hobbyViewModel.UserId = userId;
            var a = _mapper.Map<RecommenceHobby>(hobbyViewModel);
            _hobbyService.LoadAndUpdate(a);

            return Ok(listCode);
        }
    }
}