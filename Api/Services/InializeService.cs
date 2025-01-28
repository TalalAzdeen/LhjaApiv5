using ASG.ApiService.Repositories;
using ASG.ApiService.Utilities;
using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Services
{
    public class InializeService([FromServices] IServiceProvider sp, IUserClaims userClaims, TrackSubscription trackSubscription)
    {
        public async Task Inialize()
        {
            if (userClaims.User?.Identity != null && userClaims.User.Identity.IsAuthenticated)
            {
                var userManager = sp.GetRequiredService<UserManager<ApplicationUser>>();
                var user = await userManager.Users.Include(u => u.Subscription)
                    .FirstOrDefaultAsync(u => u.Id == userClaims.UserId);

                if (user?.Subscription != null)
                {
                    var subscription = user.Subscription;
                    trackSubscription.Status = subscription.Status;
                    trackSubscription.CancelAtPeriodEnd = subscription.CancelAtPeriodEnd;
                }
            }

        }
    }
}
