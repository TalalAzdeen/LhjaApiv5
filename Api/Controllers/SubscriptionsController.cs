using Api.Repositories;
using Api.Services;
using ASG.Api2.Results;
using Dto.Subscription;
using Entities;
using Microsoft.AspNetCore.Mvc;
using StripeGateway;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionsController(
        SubscriptionService userSubscriptionService,
        IUserRepository userRepository,
        IStripeSubscription stripeSubscription
        ) : Controller
    {
        [HttpGet(Name = "GetSubscriptions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<SubscriptionResponse>>> GetAll()
        {
            var result = await userSubscriptionService.GetAllAsync();
            return Ok(result);
        }


        [HttpGet("{id}", Name = "GetSubscription")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<SubscriptionResponse>> GetOne(string id)
        {
            var result = await userSubscriptionService.GetByIdAsync(id);
            return Ok(result);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPut("{customerId}", Name = "CheckSubscription")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SubscriptionResponse>> CheckSubscription(string customerId)
        {
            try
            {
                var stripeSub = await stripeSubscription.GetAllAsync(customerId);
                if (stripeSub.Any())
                {
                    var sub = stripeSub.First();
                    var subscription = await userSubscriptionService.GetByCustomerAsync(customerId);
                    if (subscription != null)
                    {
                        await userSubscriptionService.DeleteAsync(subscription.Id);
                    }
                    var subscriptionCreate = new Subscription
                    {
                        Id = sub.Id,
                        PlanId = sub.Items.First().Price.Id,
                        CustomerId = customerId,
                        StartDate = sub.StartDate,
                        Status = sub.Status,
                        CancelAtPeriodEnd = sub.CancelAtPeriodEnd,
                        CancelAt = sub.CancelAt,
                        CanceledAt = sub.CanceledAt
                    };
                    await userSubscriptionService.CreateAsync(subscriptionCreate);
                    return Ok();
                }
                return NotFound(Result.NotFound("User doesn't has subscription in stripe"));
            }
            catch (Exception e)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = e.Message,
                    Detail = e.InnerException?.Message
                });
            }
        }


        [HttpPatch("pause/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SubscriptionResponse>> Pause(string id)
        {
            try
            {
                var result = await stripeSubscription.UpdateAsync(id, new Stripe.SubscriptionUpdateOptions
                {
                    CancelAtPeriodEnd = true
                });

                if (result.CancelAtPeriodEnd)
                    return Ok();
                return BadRequest(new ProblemDetails { Detail = "Faild cancel subscription" });
            }
            catch (Exception ex)
            {
                return BadRequest(Result.Problem(ex));
            }
        }

        [HttpPatch("resume/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SubscriptionResponse>> Resume(string id)
        {
            try
            {
                var result = await stripeSubscription.ResumeAsync(id, new Stripe.SubscriptionResumeOptions
                {
                    BillingCycleAnchor = Stripe.SubscriptionBillingCycleAnchor.Now,
                });
                var item = await userSubscriptionService.GetByIdAsync(id);
                return Ok(item);
            }
            catch (Exception ex)
            {
                return BadRequest(Result.Problem(ex));
            }
        }



        [HttpDelete("{id}", Name = "CancelSubscription")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SubscriptionResponse>> CancelSubscription(string id)
        {
            try
            {
                var result = await stripeSubscription.CancelAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(Result.Problem(ex));
            }
        }


    }
}