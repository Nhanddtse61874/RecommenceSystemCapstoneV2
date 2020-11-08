using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Persistence.Models;
using RecommenceSystemCapstoneV2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecommenceSystemCapstoneV2.Controllers
{
    public  class AutoMapping : Profile
    {
        protected IMapper _mapper;

        public AutoMapping()
        {
            CreateMap<Category, CategoryViewModel>().ReverseMap();
            CreateMap<ProductRecommencePrice, ProductViewModel>().ReverseMap();
            
            CreateMap<Product, ProductViewModel>().ReverseMap();
            CreateMap<Product, CreateProductViewModel>().ReverseMap();
            
            CreateMap<RecommenceHobby, RecommenceByHobbyViewModel>().ReverseMap();
            CreateMap<RecommenceHobby, CreateRecommenceByHobbyViewModel>().ReverseMap();
            
            
            CreateMap<RecommencePrice, RecommenceByPriceViewModel>().ReverseMap();
            CreateMap<RecommencePrice, CreateRecommenceByPriceViewModel>().ReverseMap();

            CreateMap<Recommence, RecommenceViewModel>().ReverseMap();
            CreateMap<RecommenceByBoth, RecommenceByBothViewModel>().ReverseMap();
            
            CreateMap<User, UserViewModel>().ReverseMap();
            
            
            CreateMap<RecommenceHobby, ProductRecommenceHobby>()
                .ForMember(x => x.RecommenceHobbyId, src => src.MapFrom(dest => dest.Id)).ReverseMap();

            
            CreateMap<ProductViewModel, ProductRecommenceHobby>()
                .ForMember(x => x.Id, src => src.Ignore())
                .ForMember(x => x.ProductId, src => src.MapFrom(dest => dest.Id));

            
            CreateMap<RecommencePrice, ProductRecommencePrice>()
                .ForMember(x => x.RecommencePriceId, src => src.MapFrom(dest => dest.Id)).ReverseMap();

            CreateMap<ProductViewModel, ProductRecommencePrice>()
                .ForMember(x => x.Id, src => src.Ignore())
                .ForMember(x => x.ProductId, src => src.MapFrom(dest => dest.Id));

        }
    }
}
