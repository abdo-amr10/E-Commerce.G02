using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction;
using Shared;

namespace Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(IServiceManger ServiceManger) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResultDto>>> GetAllProducts(string? sort, int? brandId, int? typeId)
        {
            var products = await ServiceManger.ProductService.GetAllProductsAsync(sort , brandId , typeId);
            return Ok(products);
        }

        [HttpGet("Brands")]
        public async Task<ActionResult<IEnumerable<BrandResultDto>>> GetAllBrands()
        {
            var brands = await ServiceManger.ProductService.GetAllBrandsAsync();
            return Ok(brands);
        }

        [HttpGet("Types")]
        public async Task<ActionResult<IEnumerable<TypeResultDto>>> GetAllTypes()
        {
            var types = await ServiceManger.ProductService.GetAllTypesAsync();
            return Ok(types);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResultDto>> GetProduct(int id)
        {
            var product = await ServiceManger.ProductService.GetProductByIdAsync(id);
            return Ok(product);
        }
    }
}
