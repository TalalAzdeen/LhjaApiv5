using Api.Repositories;
using Dto.Stripe.CheckoutDto;
using Dto.Stripe.Customer;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using StripeGateway;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckoutController(
        IUserRepository userRepository,
        IStripeSubscription stripeSubscription,
        IStripeCheckout stripeCheckout,
        IStripeCustomer stripeCustomer,
        IPlanRepository planRepository
        ) : Controller
    {


        [HttpPost(Name = "CreateCheckout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CheckoutResponse>> CreateCheckout(CheckoutOptions checkoutOptions)
        {
            try
            {
                var user = await userRepository.GetByAsync(null);
                //if (user.SubscriptionId != null)
                //{
                //    return Conflict(new ProblemDetails { Detail = "You already have subscription" });
                //}

                var plan = await planRepository.GetByAsync(p => p.Id == checkoutOptions.PlanId);
                if (plan is null) return NotFound(new ProblemDetails { Title = "NOT FOUND", Detail = "Plan not found" });

                await CreateCustomer(user);

                //var setting = await settingService.GetBy(s => s.Value == checkoutOptions.PlanId);
                var response = await CreateFreeSubscription(checkoutOptions, plan, user);
                if (response != null) return response;

                var options = new SessionCreateOptions
                {
                    Customer = user.CustomerId,
                    SuccessUrl = $"{checkoutOptions.SuccessUrl}?session_id={{CHECKOUT_SESSION_ID}}",
                    CancelUrl = $"{checkoutOptions.CancelUrl}",
                    Mode = "subscription",
                    Expand = new List<string> { "customer" },
                    LineItems = new List<SessionLineItemOptions>
                {
                        new SessionLineItemOptions
                        {
                        Price = checkoutOptions.PlanId,
                        Quantity = 1,
                    },
                },
                    // AutomaticTax = new SessionAutomaticTaxOptions { Enabled = true },
                };

                //options.Customer = user.CustomerId;
                //else options.CustomerEmail = user.Email;

                var session = await stripeCheckout.CreateCheckoutSession(options);
                return Ok(new { session.Url });
            }
            catch (Exception ex)
            {
                return BadRequest(new ProblemDetails { Detail = ex.Message });
            }
        }

        private async Task<ActionResult?> CreateFreeSubscription(CheckoutOptions checkoutOptions, Plan plan, ApplicationUser user)
        {
            if (plan.Amount == 0)
            {
                var sub = await stripeSubscription.CreateAsync(new Stripe.SubscriptionCreateOptions()
                {
                    Customer = user.CustomerId,
                    Items = new List<Stripe.SubscriptionItemOptions>{
                        new Stripe.SubscriptionItemOptions
                        {
                            Price = checkoutOptions.PlanId,
                        },
                    },
                    TrialPeriodDays = 0, // بدون فترة تجريبية
                    PaymentBehavior = "default_incomplete", // يتم تجاهل الدفع لأنه مجاني

                });
                if (sub != null) return Ok(new { Message = "You have successfully subscribed to the free plan." });
                return BadRequest(new ProblemDetails { Detail = "con not subscribe for free plan" });
            }

            return null;
        }

        private async Task CreateCustomer(ApplicationUser user)
        {
            var customers = await stripeCustomer.GetCustomersByEmail(user.Email);
            var customer = customers.FirstOrDefault();
            if (customer == null)
            {
                customer = await stripeCustomer.CreateAsync(new Stripe.CustomerCreateOptions()
                {
                    Name = user.DisplayName,
                    Email = user.Email
                });

                user.CustomerId = customer.Id;
                //await userRepo.SaveClaimAsync(new Claim(ClaimTypes2.CustomerId, customer.Id));
            }
            else
            {
                user.CustomerId = customer.Id;
            }

            await userRepository.UpdateAsync(user);
        }


        [HttpPost("manage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CheckoutResponse>> ManageSubscription(SessionCreate sessionCreate)
        {
            try
            {
                var user = await userRepository.GetByAsync(null);
                //if(user.CustomerId==null) return 
                var session = await stripeCustomer.CustomerPortal(new Stripe.BillingPortal.SessionCreateOptions
                {
                    Customer = user.CustomerId,
                    ReturnUrl = sessionCreate.ReturnUrl,
                });
                return Ok(new { session.Url });
            }
            catch (Exception ex)
            {
                return BadRequest(new ProblemDetails { Detail = ex.Message });
            }
        }

        //[HttpPost("subscripe-user")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public async Task<IActionResult> SubscripeUser(string sessionId)
        //{
        //    try
        //    {
        //        var session = await _stripeCheckout.GetSession(sessionId);
        //        var subscription = await _stripeCheckout.GetSubscription(session.SubscriptionId);
        //        var price = subscription.Items.First().Price;
        //        var user = await _userManager.FindByEmailAsync(User.GetClaim(ClaimTypes.Email));
        //        var plan = await _planRepo.GetAsync(p => p.Id == price.Id);

        //        UserSubscription userSubscription = new UserSubscription()
        //        {
        //            Id = subscription.Id,
        //            CR = plan.CountRequests,
        //            PlanId = plan.Id,
        //            OwnerId = user.Id,
        //            Status = subscription.Status
        //        };
        //        if (user.SubscriptionId == null)
        //        {
        //            user.Subscription = userSubscription;
        //            await _userManager.AddClaimsAsync(user, new Claim[]
        //            {
        //                    new Claim(StripeTypes.Type,price.Recurring.Interval),
        //                    new Claim(StripeTypes.NR,plan.CountRequests.ToString()),
        //                    new Claim(StripeTypes.CNR,"0"),
        //                    new Claim(StripeTypes.CustomerId,subscription.CustomerId),
        //                    new Claim(StripeTypes.Status,subscription.Status),

        //            });
        //        }
        //        else await _userSubscriptionRepo.UpdateAsync(userSubscription);
        //        //user.Subscriptions = new UserSubscription { };
        //        user.CustomerId = subscription.CustomerId;
        //        await _userManager.UpdateAsync(user);

        //        //await _userManager.AddClaimsAsync(user, new Claim[]
        //        //{
        //        //    //new Claim("interval",price.Recurring.Interval),
        //        //    //new Claim("count_requests",user.Plan.CountRequests.ToString()),
        //        //    new Claim("stripe_customer_id",subscription.CustomerId.ToString()),

        //        //});
        //        await _signInManager.RefreshSignInAsync(user);
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new ProblemDetails { Detail = ex.Message });
        //    }
        //}


    }
}