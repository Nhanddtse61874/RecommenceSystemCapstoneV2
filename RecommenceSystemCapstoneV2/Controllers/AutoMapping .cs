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
            CreateMap<Category, CreateCategoryViewModel>().ReverseMap();

            CreateMap<ProductRecommencePrice, ProductViewModel>().ReverseMap();
            
            CreateMap<Product, ProductViewModel>().ReverseMap();
            CreateMap<Product, CreateProductViewModel>().ReverseMap();
            
            CreateMap<RecommenceHobby, RecommenceByHobbyViewModel>()
                .ForMember(x => x.ProductRecommenceHobbies, src => src.MapFrom(dest => dest.ProductRecommenceHobbies))
                .ReverseMap();
            CreateMap<RecommenceHobby, CreateRecommenceByHobbyViewModel>().ReverseMap();
            
            
            CreateMap<RecommencePrice, RecommenceByPriceViewModel>()
                .ForMember(x => x.ProductRecommencePrices, src => src.MapFrom(dest => dest.ProductRecommencePrices))
                .ReverseMap();
            CreateMap<RecommencePrice, CreateRecommenceByPriceViewModel>().ReverseMap();

            CreateMap<Recommence, RecommenceViewModel>().ReverseMap();
            CreateMap<RecommenceByBoth, RecommenceByBothViewModel>().ReverseMap();
            
            CreateMap<User, UserViewModel>().ReverseMap();
            CreateMap<User, CreateUserViewModel>().ReverseMap();
            
            
            CreateMap<RecommenceHobby, ProductRecommenceHobby>()
                .ForMember(x => x.RecommenceHobbyId, src => src.MapFrom(dest => dest.Id)).ReverseMap();

            CreateMap<Product, ProductRecommenceHobby>()
                .ForMember(x => x.Id, src => src.Ignore())
                .ForMember(x => x.Product, src => src.MapFrom(dest => dest));


            //CreateMap<Product, ProductRecommencePrice>()
            //    .ForMember(x => x.Id, src => src.Ignore())
            //    .ForMember(x => x.Product, src => src.MapFrom(dest => dest));

            CreateMap<ProductViewModel, ProductRecommenceHobby>()
                .ForMember(x => x.Id, src => src.Ignore())
                .ForMember(x => x.ProductId, src => src.MapFrom(dest => dest.Id))
                //.ForMember(x => x.Product, src => src.MapFrom(dest => dest))
                .ReverseMap();
            
            CreateMap<RecommencePrice, ProductRecommencePrice>()
                .ForMember(x => x.RecommencePriceId, src => src.MapFrom(dest => dest.Id)).ReverseMap();

            CreateMap<ProductViewModel, ProductRecommencePrice>()
                .ForMember(x => x.Id, src => src.Ignore())
                .ForMember(x => x.ProductId, src => src.MapFrom(dest => dest.Id))
                .ReverseMap();
                


        }
    }
}
