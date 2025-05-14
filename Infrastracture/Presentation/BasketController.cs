using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction;
using Shared.Dtos;

namespace Presentation
{
    [Authorize]

    public class BasketController(IServiceManger _serviceManger) : ApiController
    {
        [HttpGet]
        public async Task<ActionResult<BasketDto>> Get(string id)
        {
            var basket = await _serviceManger.BasketService.GetBasketAsync(id);
            return basket;
        }

        [HttpPost]
        public async Task<ActionResult<BasketDto>> Update(BasketDto basketDto)
        {
            var basket = await _serviceManger.BasketService.UpdateBasketAsync(basketDto);
            return Ok(basket);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<BasketDto>> Delete(string id)
        {
            var basket = await _serviceManger.BasketService.DeleteBasketAsync(id);
            return NoContent();
        }

    }
}
