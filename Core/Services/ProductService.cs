using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Domain.Exceptions;
using Services.Abstraction;
using Services.Specifications;
using Shared;
using Shared.Dtos;

namespace Services
{
    public class ProductService(IUnitOfWork _unitOfWork , IMapper _mapper) : IProductService
    {
       

        public async Task<PaginatedResult<ProductResultDto>> GetAllProductsAsync(ProductSpecParams parameters)
        {
            var products = await _unitOfWork.GetRepository<Product, int>().GetAllAsync(new ProductWithBrandAndTypeSpecifications(parameters));
            var productsResult = _mapper.Map<IEnumerable<ProductResultDto>>(products);
            var count = productsResult.Count(); 
            var totalCount = await _unitOfWork.GetRepository<Product, int>().CountAsync(new ProductCountSpecifications(parameters));
            var result = new PaginatedResult<ProductResultDto>
            (
                parameters.PageIndex,
                count,
                totalCount,
                productsResult
            );
            return result;
        }
        public async Task<ProductResultDto> GetProductByIdAsync(int id)
        {
            var product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(new ProductWithBrandAndTypeSpecifications(id));
            //var productResult = _mapper.Map<ProductResultDto>(product);
            //return productResult;
            return product is null ? throw new ProductNotFoundException(id) : _mapper.Map<ProductResultDto>(product);
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
