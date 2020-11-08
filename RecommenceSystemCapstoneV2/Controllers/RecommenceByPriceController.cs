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
using Persistence.Models;
using RecommenceSystemCapstoneV2.ViewModels;

namespace RecommenceSystemCapstoneV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecommenceByPriceController : ControllerBase
    {

        private readonly IMapper _mapper;
        private readonly IRecommencePriceService _priceService;
        private readonly IProductService _productService;
        private readonly IUserService _userService;
        private readonly ICategoryService _categoryService;
        private readonly IRecommendBestSellerService _bestSellerService;
        public RecommenceByPriceController(IMapper mapper, IRecommencePriceService priceService,
           IProductService productService,
           IUserService userService,
           ICategoryService categoryService,
           IRecommendBestSellerService bestSellerService
           )
        {
            _priceService = priceService;
            _mapper = mapper;
            _productService = productService;
            _userService = userService;
            _categoryService = categoryService;
            _bestSellerService = bestSellerService;
        }

        [HttpPost]
        public ActionResult<IList<string>> LoadAndUpdate([FromForm]string data)
        {
            Recommence recommence = new Recommence();
            try
            {
                recommence = JsonSerializer.Deserialize<Recommence>(data);
                if (!TryValidateModel(recommence))
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error");
            }

            var listProducts = _priceService.RecommendByPriceAvarageGetListProducts(recommence);
            var userId = recommence.UserId;

            var products = _mapper.Map<IEnumerable<CreateProductViewModel>>(listProducts)
              .Select(x => _productService.Create(_mapper.Map<Product>(x)));
            IEnumerable<string> listCode = listProducts.Select(x => x.Code);

            CreateRecommenceByPriceViewModel hobbyViewModel = new CreateRecommenceByPriceViewModel();
            var list = _mapper.Map<IEnumerable<ProductViewModel>>(products);
            hobbyViewModel.ProductRecommencePrices = list;
            hobbyViewModel.UserId = userId;
            var a = _mapper.Map<RecommencePrice>(hobbyViewModel);
            _priceService.LoadAndUpdate(a);

            return Ok(listCode);
        }
    }
}