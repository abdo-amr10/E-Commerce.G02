using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Services.Abstraction;
using Services.Specifications;
using Shared;

namespace Services
{
    public class ProductService(IUnitOfWork _unitOfWork , IMapper _mapper) : IProductService
    {
       

        public async Task<IEnumerable<ProductResultDto>> GetAllProductsAsync(string? sort, int? brandId, int? typeId)
        {
            var products = await _unitOfWork.GetRepository<Product, int>().GetAllAsync(new ProductWithBrandAndTypeSpecifications(sort , brandId , typeId));
            var productsResult = _mapper.Map<IEnumerable<ProductResultDto>>(products);
            return productsResult;
        }
        public async Task<ProductResultDto> GetProductByIdAsync(int id)
        {
            var product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(new ProductWithBrandAndTypeSpecifications(id));
            var productResult = _mapper.Map<ProductResultDto>(product);
            return productResult;
        }

        public async Task<IEnumerable<TypeResultDto>> GetAllTypesAsync()
        {
            var types = await _unitOfWork.GetRepository<ProductType, int>().GetAllAsync();
            var typesResult = _mapper.Map<IEnumerable<TypeResultDto>>(types);
            return typesResult;
        }

        public async Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync()
        {
            var brands = await _unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync();
            var brandsResult = _mapper.Map<IEnumerable<BrandResultDto>>(brands);
            return brandsResult;
        }


    }
}
