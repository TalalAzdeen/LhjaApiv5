using Api.Repositories;
using Api.Utilities;
using ASG.ApiService.Services;
using ASG.ApiService.Utilities;
using AutoMapper;
using Data;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Stripe;
using StripeGateway;

namespace Api.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class WebhookController(
    IStripeWebhook stripeWebhook,
    ClaimsChange claimsChange,
    TrackSubscription trackSubscription,
    IServiceScopeFactory serviceScopeFactory,
    IEmailService emailSender
    ) : Controller
{
    private static readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);

    [HttpPost]
    public async void Index()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

        try
        {
            var stripeEvent = stripeWebhook.ConstructEvent(json);
            // ينتظر الحدث الحالي انتهاء أي حدث سابق
            await _lock.WaitAsync();

            //Log.Information("Type Event is: {0}", stripeEvent.Type);
            switch (stripeEvent.Type)
            {
                case EventTypes.CustomerDeleted:
                    {
                        Customer? customer = stripeEvent.Data.Object as Customer;
                        using var scope = serviceScopeFactory.CreateScope();
                        var context = scope.ServiceProvider.GetRequiredService<DataContext>();
                        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                        if (customer?.Email == null) break;

                        var user = await userManager.FindByEmailAsync(customer.Email);
                        if (user != null)
                        {
                            user.CustomerId = null;
                            await userManager.UpdateAsync(user);
                        }
                        break;
                    }
                // Handle the event
                case EventTypes.CustomerSubscriptionCreated:
                    {
                        var subscription = stripeEvent.Data.Object as Stripe.Subscription;
                        var price = subscription.Items.First().Price;

                        Console.ForegroundColor = ConsoleColor.Green;
                        Log.Information("Created Subscription: {0}", subscription.Id);
                        Console.WriteLine("Created Subscription: {0}", subscription.Id);

                        await CreateSubscription(subscription, price);
                        claimsChange.SendEmail = true;
                        break;
                    }
                case EventTypes.CustomerSubscriptionUpdated:
                    {
                        var subscription = stripeEvent.Data.Object as Stripe.Subscription;
                        var price = subscription.Items.First().Price;

                        Console.ForegroundColor = ConsoleColor.Green;
                        Log.Information("Updated Subscription: {0}", subscription.Id);
                        Console.WriteLine("Updated Subscription: {0}", subscription.Id);
                        await UpdateSubscription(subscription, price);
                        break;
                    }

                case EventTypes.CustomerSubscriptionDeleted:
                    {
                        var subscription = stripeEvent.Data.Object as Stripe.Subscription;
                        await DeleteSubscription(subscription);

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Deleted: {0}", stripeEvent.Type);
                        break;
                    }
                default:
                    // ... handle other event types
                    Console.ForegroundColor = ConsoleColor.Red;
                    //Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                    //Log.Information("Unhandled event type: {0}", stripeEvent.Type);
                    break;
            }

        }
        catch (Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            //Console.WriteLine(e.Message);
            Log.Information("Error is: {0}", e.Message);
            Console.WriteLine("Error is: {0}", e.Message);
            Log.Information($"Inner Exception is: {e.InnerException?.Message}");
            //throw;
        }
        finally
        {
            // تحرير القفل للسماح للحدث التالي بالعمل
            _lock.Release();
        }
    }

    private async Task CreateSubscription(Stripe.Subscription subscription, Price price)
    {
        using var scope = serviceScopeFactory.CreateScope();
        // Resolve DataContext from the new scope
        var context = scope.ServiceProvider.GetRequiredService<DataContext>();
        var subscriptionRepo = new SubscriptionRepository(context);
        var userSubscription = await subscriptionRepo.GetByAsync(s => s.CustomerId == subscription.CustomerId, false);
        if (userSubscription != null)
        {
            await subscriptionRepo.RemoveAsync(userSubscription);
        }

        var userRepo = new UserRepository(context);
        var user = await userRepo.GetByAsync(s => s.CustomerId == subscription.CustomerId, false);
        userSubscription = new()
        {
            Id = subscription.Id,
            UserId = user.Id,
            CustomerId = subscription.CustomerId,
            Status = subscription.Status,
            PlanId = price.Id,
            StartDate = subscription.StartDate,
        };
        trackSubscription.Status = subscription.Status;
        trackSubscription.CancelAtPeriodEnd = subscription.CancelAtPeriodEnd;
        claimsChange.IsChange = true;
        //claimsChange.User = user;
        await subscriptionRepo.CreateAsync(userSubscription);

    }

    private async Task UpdateSubscription(Stripe.Subscription subscription, Price price)
    {
        //bool condition = false;
        //while (!condition)
        //{
        using (var scope = serviceScopeFactory.CreateScope())
        {
            // Resolve DataContext from the new scope
            var context = scope.ServiceProvider.GetRequiredService<DataContext>();
            var maper = scope.ServiceProvider.GetRequiredService<IMapper>();

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var subscriptionRepo = new SubscriptionRepository(context);
            var userSubscription = await subscriptionRepo.GetByAsync(s => s.Id == subscription.Id);
            if (userSubscription != null)
            {
                userSubscription.Status = subscription.Status;
                userSubscription.CancelAt = subscription.CancelAt;
                userSubscription.CancelAtPeriodEnd = subscription.CancelAtPeriodEnd;
                userSubscription.CanceledAt = subscription.CanceledAt;
                userSubscription.PlanId = price.Id;
                await subscriptionRepo.UpdateAsync(userSubscription);

                trackSubscription.Status = subscription.Status;
                trackSubscription.CancelAtPeriodEnd = subscription.CancelAtPeriodEnd;

                claimsChange.IsChange = true;
                //claimsChange.User = user;
                //}
                if (claimsChange.SendEmail)
                {
                    var planRepository = new PlanRepository(context, maper);
                    var plan = await planRepository.GetByAsync(p => p.Id == price.Id);
                    var user = await userManager.Users.AsNoTracking()
                        .FirstOrDefaultAsync(u => u.CustomerId == subscription.CustomerId);
                    var namesSelect = plan.PlanServices.Select(p => p.Service.Name);

                    await emailSender.SendSubscriptionSuccessEmailAsync(user?.Email!,
                        user?.DisplayName!,
                        planName: string.Join(", ", namesSelect),
                        duration: plan.BillingPeriod,
                        activationDate: subscription.StartDate.ToLongDateString(),
                        subscriptionId: subscription.Id);
                    claimsChange.SendEmail = false;
                }
                //condition = true;
                Log.Information("User Subscription is:{0}", "update subscription successfully");
                //timer.Stop();
            }
            else
            {
                Log.Information("Subscription with id ({0}) not found ", subscription.Id);
            }
        };
        //} // end while
    }

    private async Task DeleteSubscription(Stripe.Subscription subscription, CancellationToken cancellationToken = default)
    {
        using (var scope = serviceScopeFactory.CreateScope())
        {
            // Resolve DataContext from the new scope

            var context = scope.ServiceProvider.GetRequiredService<DataContext>();
            var delete = await context.Subscriptions.Where(us => us.Id == subscription.Id).ExecuteDeleteAsync();
            if (delete > 0)
            {
                trackSubscription.Status = Status.Canceled;
                trackSubscription.CancelAtPeriodEnd = false;
            }
            //trackSubscription.
        }
    }

}
