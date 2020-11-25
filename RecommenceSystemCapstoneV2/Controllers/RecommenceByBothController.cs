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
        public async Task<ActionResult<ResultViewModel>> Load(int Id)
        {
           
            ResultViewModel resultViewModel = new ResultViewModel();
            if (Id != null)
            {
                IList<int> list = new List<int>();
                var list1 = _hobbyService.CheckAndLoadFromDb(Id);
                if(list1 != null)
                {
                     list = list1.ProductRecommenceHobbies.Select(x => x.ProductId).ToList();
                }
                

                IList<Product> hobbys = new List<Product>();
                foreach (var item in list)
                {
                    var product = _productService.Get(item);
                    hobbys.Add(product);
                }
                IList<int> list2 = new List<int>();
                var list3 = _priceService.CheckAndLoadFromDb(Id);
                if(list3 != null)
                {
                    list2 = list3.ProductRecommencePrices.Select(x => x.ProductId).ToList();

                }
                IList<Product> prices = new List<Product>();
                foreach (var item in list2)
                {
                    var product = _productService.Get(item);
                    prices.Add(product);
                }

                var result = _bothService.GetListProductByBoth(hobbys, prices).Distinct().ToList();


                IList<string> finalResult = new List<string>();


                if (result.Count < 4)
                {
                    if (hobbys.Count < 4)
                    {
                        if (prices.Count < 4)
                        {
                            resultViewModel.FinalResult = null;
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
            return Ok();
            
        }
    }
}