using AutoMapper;
using Dto.Stripe.StripeSubscription;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using StripeGateway;

namespace Api.Controllers
{

    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/[controller]")]
    [ApiController]
    [OutputCache(PolicyName = "CustomPolicy")]
    public class StripeSubscriptionController(IStripeSubscription stripeSubscription, IMapper mapper) : Controller
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetSubscriptions(string? customerId)
        {
            var result = await stripeSubscription.GetAllAsync(customerId);
            //var data = result.Data;
            var data = mapper.Map<List<StripeSubscriptionResponse>>(result);
            return Ok(new { result.HasMore, data });
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetSubscription(string id)
        {
            try
            {
                var response = await stripeSubscription.GetByIdAsync(id);
                var data = mapper.Map<StripeSubscriptionResponse>(response);
                //return Ok(data);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new ProblemDetails { Title = ex.Message, Detail = ex.InnerException?.Message });
            }
        }

        [HttpDelete("cancel/{customerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Cancel(string? customerId)
        {
            try
            {
                var response = await stripeSubscription.CancelAsync(customerId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new ProblemDetails { Title = ex.Message, Detail = ex.InnerException?.Message });
            }
        }
    }
}