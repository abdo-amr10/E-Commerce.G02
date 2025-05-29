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

        const string endpointSecret = "whsec_...";

        [HttpPost("WebHook")] // https://localhost:7186/api/Payments/WebHook
        public async Task<IActionResult> WebHook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var stripeHeader = Request.Headers["Stripe-Signature"];

            // يمكن تفعيل السطر التالي إذا أردت التحقق من التوقيع هنا مباشرة:
            // var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], endpointSecret);

            await serviceManger.PaymentService.UpdateOrderPaymentStatus(json, stripeHeader!);

            return new EmptyResult();
        }

    }
}
