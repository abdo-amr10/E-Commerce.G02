using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction;
using Shared.Dtos;

namespace Presentation
{
    public class PaymentsController(IServiceManger serviceManger) : ApiController
    {
        [HttpPost("{basketId}")]
        public async Task<ActionResult<BasketDto>> CreateOrUpdatePayment(string basketId)
        {
            var result = await serviceManger.PaymentService.CreateOrUpdatePaymentIntentAsync(basketId);
            return Ok(result);
        }
    }
}
