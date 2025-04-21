using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities;
using Shared;

namespace Services.Mapping_Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductBrand,BrandResultDto>();
            CreateMap<ProductType,TypeResultDto>();
            CreateMap<Product, ProductResultDto>()
                      .ForMember(p => p.BrandName,options=> options.MapFrom(s=> s.ProductBrand.Name));
            CreateMap<Product, ProductResultDto>()
                      .ForMember(p => p.TypeName, options => options.MapFrom(s => s.ProductType.Name));
        }
    }
}
