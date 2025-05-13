using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Order_Entities;
using Microsoft.Extensions.Configuration;
using Shared.Dtos;
using Shared.Order_Models;

namespace Services.Mapping_Profiles
{
    public class OrderItemPictureUrlResolver(IConfiguration configuration) : IValueResolver<OrderItem, OrderItemDto, string>
    {
        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (string.IsNullOrWhiteSpace(source.Product.PictureUrl))
                return string.Empty;

            return $"{configuration["BaseUrl"]}{source.Product.PictureUrl}";
        }
    }
}
