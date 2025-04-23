using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction;
using Shared;
using Shared.Dtos;
using Shared.Error_Models;

namespace Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(IServiceManger ServiceManger) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResultDto>>> GetAllProducts([FromQuery] ProductSpecParams parameters)
        {
            var products = await ServiceManger.ProductService.GetAllProductsAsync(parameters);
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
        [ProducesResponseType(typeof(ErrorDetails), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorDetails), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ValidationErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProductResultDto), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ProductResultDto>> GetProduct(int id)
        {
            var product = await ServiceManger.ProductService.GetProductByIdAsync(id);
            return Ok(product);
        }
    }
}
