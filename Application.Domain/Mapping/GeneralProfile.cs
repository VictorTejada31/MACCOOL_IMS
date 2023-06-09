﻿using AutoMapper;
using Core.Application.ViewModels.Category;
using Core.Application.ViewModels.Client;
using Core.Application.ViewModels.DefaultProducts;
using Core.Application.ViewModels.Product;
using Core.Domain.Entities;

namespace Core.Application.Mapping
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile() {

            #region Category

            CreateMap<Category, SaveCategoryViewModel>()
                .ReverseMap()
                .ForMember(m => m.DefaultProducts, op => op.Ignore());

            CreateMap<Category, CategoryViewModel>()
               .ReverseMap()
               .ForMember(m => m.DefaultProducts, op => op.Ignore());

            #endregion

            #region Client

            CreateMap<Client, SaveClientViewModel>()
                .ReverseMap();

            CreateMap<Client, ClientViewModel>()
                .ReverseMap();

            #endregion

            #region Products

            CreateMap<Product, SaveProductViewModel>()
                .ReverseMap()
                .ForMember(m => m.DefaultProduct, op => op.Ignore());

            CreateMap<Product, ProductViewModel>()
                .ForMember(m => m.BarCode, op => op.Ignore())
                .ForMember(m => m.Img, op => op.Ignore())
                .ForMember(m => m.Category, op => op.Ignore())
                .ForMember(m => m.Name, op => op.Ignore())
                .ForMember(m => m.Description, op => op.Ignore())
                .ReverseMap()
                .ForMember(m => m.DefaultProduct, op => op.Ignore());


            #endregion

            #region DefaultProducts

            CreateMap<DefaultProduct, SaveDefaultProductViewModel>()
                //.ForMember(m => m.Img, op => op.Ignore())
                .ReverseMap()
                .ForMember(m => m.Products, op => op.Ignore());

            CreateMap<DefaultProduct, DefaultProductViewModel>()
                .ReverseMap()
                .ForMember(m => m.Products, op => op.Ignore());

            #endregion

        }
    }
}
