using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities;
using Shared.Dtos;

namespace Services.Mapping_Profiles
{
    internal class BasketProfile : Profile
    {
        public BasketProfile()
        {
            CreateMap<CustomerBasket,BasketDto>();
            CreateMap<BasketDto, CustomerBasket>();

            CreateMap<BasketItems, BasketItemDto>();
            CreateMap<BasketItemDto, BasketItems>();
        }
    }
}
