namespace ASG.Api2;

using ASG.ApiService.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class SubscriptionCheckFilter(TrackSubscription trackSubscription) : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (trackSubscription.CancelAtPeriodEnd)
        {
            context.Result = new ObjectResult(new { Message = "You have canceled your subscription, renew it if you want use this service." }) { StatusCode = StatusCodes.Status402PaymentRequired };
            //context.Result = new UnauthorizedObjectResult("Your subscription has expired. Please renew.");
        }
        else if (trackSubscription.Canceled)
        {
            context.Result = new ObjectResult(new { Message = "Your subscription canceled." }) { StatusCode = StatusCodes.Status402PaymentRequired };
            //context.Result = new UnauthorizedObjectResult("Your subscription canceled.");
        }
        else if (!trackSubscription.IsSubscribe)
        {
            context.Result = new ObjectResult(new { Message = "Please subscribe if you want use this service." }) { StatusCode = StatusCodes.Status402PaymentRequired };
            //context.Result = new UnauthorizedObjectResult("Please subscribe if you want use this service.");
        }


    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}

