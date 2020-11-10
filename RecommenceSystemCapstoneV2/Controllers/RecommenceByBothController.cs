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
    public class RecommenceByBothController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRecommencePriceService _priceService;
        private readonly IRecommenceHobbyService _hobbyService;
        private readonly IRecommenceByBothService _bothService;
        private readonly IProductService _productService;
        private readonly IUserService _userService;
        private readonly ICategoryService _categoryService;
        private readonly IRecommendBestSellerService _bestSellerService;
        public RecommenceByBothController(IMapper mapper, IRecommencePriceService priceService,
           IProductService productService,
           IUserService userService,
           ICategoryService categoryService,
           IRecommendBestSellerService bestSellerService,
           IRecommenceByBothService bothService,
           IRecommenceHobbyService hobbyService
           )
        {
            _priceService = priceService;
            _mapper = mapper;
            _productService = productService;
            _userService = userService;
            _categoryService = categoryService;
            _bestSellerService = bestSellerService;
            _hobbyService = hobbyService;
            _bothService = bothService;
        }

        [HttpPost]
        public ActionResult<ResultViewModel> Load([FromForm]string data)
        {
            Recommence recommence = new Recommence();
            //convert model from web 
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

            var list = _hobbyService.CheckAndLoadFromDb(recommence.UserId).ProductRecommenceHobbies.Select(x => x.ProductId).ToList();
            
            IList<Product> hobbys = new List<Product>();
            foreach (var item in list)
            {
                var product = _productService.Get(item);
                hobbys.Add(product);
            }

            var list2 = _priceService.CheckAndLoadFromDb(recommence.UserId).ProductRecommencePrices.Select(x => x.ProductId).ToList();
            IList<Product> prices= new List<Product>();
            foreach (var item in list2)
            {
                var product = _productService.Get(item);
                prices.Add(product);
            }

            var result = _bothService.GetListProductByBoth(hobbys, prices).Distinct().ToList();

            var bestSeller = _bestSellerService.RecommendByBestSeller(recommence);
            IList<string> finalResult = new List<string>();

            ResultViewModel resultViewModel = new ResultViewModel();
            if (result.Count < 4)
            {
                if (hobbys.Count < 4)
                {
                    if (prices.Count < 4)
                    {
                        finalResult = bestSeller;
                        resultViewModel.FinalResult = finalResult;
                        resultViewModel.Name = "Best Seller For You";
                    }
                    else
                    {
                        finalResult = prices.Select(x => x.Code).ToList();
                        resultViewModel.FinalResult = finalResult;
                        resultViewModel.Name = "Recommend By Prices";
                    }
                }
                else
                {
                    finalResult = hobbys.Select(x => x.Code).ToList();
                    resultViewModel.FinalResult = finalResult;
                    resultViewModel.Name = "Recommend By Hobby";
                }
            }
            else
            {
                finalResult = result;
                resultViewModel.FinalResult = finalResult;
                resultViewModel.Name = "Best Recommend For You";
            }
            
            return Ok(resultViewModel);
        }
    }
}