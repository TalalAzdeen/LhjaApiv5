using Api.Utilities;
using ASG.ApiService.Utilities;
using Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Api;

public class MyClaimsTransformation(
    ClaimsChange claimsChange,
    UserManager<ApplicationUser> userManager,
    TrackSubscription trackSubscription,
    SignInManager<ApplicationUser> signInManager) : IClaimsTransformation
{
    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        ClaimsIdentity claimsIdentity = new ClaimsIdentity();
        var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        if (principal.Identity.IsAuthenticated && string.IsNullOrEmpty(trackSubscription.Status))
        {
            var user = await userManager.Users.Include(u => u.Subscription).ThenInclude(s => s.Plan)
           .FirstOrDefaultAsync(u => u.Id == userId);

            if (user?.Subscription != null)
            {
                var subscription = user.Subscription;
                //trackSubscription.NumberRequests = subscription.Plan.NumberRequests;
                trackSubscription.Status = subscription.Status;
                trackSubscription.CancelAtPeriodEnd = subscription.CancelAtPeriodEnd;
            }

            //claimsChange.IsChange = false;
        }

        //if (!principal.HasClaim(c => c.Type == ClaimTypes2.CNR))
        //{
        //    var claim = await userClaimRepository.GetByAsync(c => c.UserId == userId && c.ClaimType == ClaimTypes2.CNR);

        //    if (claim != null)
        //    {
        //        trackSubscription.CurrentNumberRequests = claim.ClaimValue.ToInt64();
        //        claimsIdentity.AddClaim(claim.ToClaim());
        //    }
        //}
        //if (!principal.HasClaim(c => c.Type == ClaimTypes2.NR) || claimsChange.IsChange)
        //{
        //    var user = await userManager.Users.Include(u => u.Subscription).ThenInclude(s => s.Plan)
        //        .FirstOrDefaultAsync(u => u.Id == principal.FindFirstValue(ClaimTypes.NameIdentifier));
        //    if (user.Subscription != null)
        //    {
        //        var subscription = user.Subscription;
        //        claimsIdentity.AddClaim(new Claim(ClaimTypes2.NR, subscription.Plan.NumberRequests.ToString()));
        //        claimsIdentity.AddClaim(new Claim(ClaimTypes2.Status, subscription.Status));
        //        claimsIdentity.AddClaim(new Claim(ClaimTypes2.CancelAtPeriodEnd, subscription.CancelAtPeriodEnd.ToString()));
        //    }
        //    claimsChange.IsChange = false;
        //}

        principal.AddIdentity(claimsIdentity);
        return principal;
    }
}
