using Api.Config;
using Api.Repositories;
using Api.Utilities;
using ASG.Api2;
using ASG.Api2.Behaviors;
using ASG.ApiService.Repositories;
using ASG.ApiService.Services;
using ASG.ApiService.Utilities;
using ASG.Helper;
using Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Utilities;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static void AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();
        services.AddHttpContextAccessor();
        services.AddHttpClient();  // Register HttpClient for dependency injection
        services.AddMemoryCache();
        services.AddAutoMapper(typeof(StripeMappingConfig));
        services.AddAutoMapper(typeof(MappingConfig));
        services.AddSingleton<IEmailService, EmailService>();
        services.AddSingleton<IEmailSender<ApplicationUser>, EmailService>();


        // options
        services.Configure<AppSettings>(configuration.GetSection("appsettings"));
        services.Configure<SmtpConfig>(configuration.GetSection(nameof(SmtpConfig)));

        services.AddSingleton<ClaimsChange>();
        services.AddSingleton<TrackSubscription>();

        // Scoped Repositories Automatically
        services.Scan(scan => scan
       .FromAssemblyOf<IBaseRepository<object>>() // Adjust to your assembly
    .AddClasses(classes => classes.Where(c => c.Name.EndsWith("Repository")))
    .AsImplementedInterfaces()
    .WithScopedLifetime());

        services.AddScoped<IUserClaims, UserClaims>();

        // Services
        services.Scan(scan => scan
        .FromAssemblyOf<EmailService>() // Adjust to your assembly
        .AddClasses(classes => classes.Where(c => c.Name.EndsWith("Service")))
        .AsSelf()
        .WithScopedLifetime());
        //services.AddScoped<PriceService>();
        //services.AddScoped<SubscriptionService>();
        //services.AddScoped<ServiceService>();
        //services.AddScoped<SettingService>();
        //services.AddScoped<RequestsService>();


        // Behaviors
        services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

        // Filters
        services.AddScoped<SubscriptionCheckFilter>();
    }

    public static void AddDefaultAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(op =>
        {
            op.DefaultScheme = IdentityConstants.BearerScheme;
            op.DefaultChallengeScheme = IdentityConstants.BearerScheme;
        })

    .AddBearerToken(IdentityConstants.BearerScheme)
    .AddIdentityCookies();
    }

    public static void AddDynamicAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(op =>
        {
            op.DefaultAuthenticateScheme = "DynamicScheme";
            op.DefaultChallengeScheme = "DynamicScheme";
        })
            .AddPolicyScheme("DynamicScheme", "Dynamic Authentication Scheme", options =>
            {
                options.ForwardDefaultSelector = context =>
                {
                    // التحقق مما إذا كان الطلب يحتوي على Bearer Token
                    var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
                    if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
                    {
                        return IdentityConstants.BearerScheme; // استخدم Bearer Token
                    }

                    return IdentityConstants.ApplicationScheme; // استخدم الكوكيز
                };
            })
            .AddBearerToken(IdentityConstants.BearerScheme)

            .AddIdentityCookies()
            ;
    }

    //public static void AddJwtAuthentication(this IServiceCollection services, AppSettings appSettings)
    //{
    //    services.AddAuthentication(IdentityConstants.BearerScheme)
    //.AddJwtBearer(IdentityConstants.BearerScheme, options =>
    //{
    //    options.TokenValidationParameters = new TokenValidationParameters
    //    {
    //        ValidateIssuer = true,
    //        ValidateAudience = true,
    //        ValidateLifetime = true,
    //        ValidateIssuerSigningKey = true,
    //        ValidIssuer = appSettings.Jwt.validIssuer,
    //        ValidAudience = appSettings.Jwt.ValidAudience,
    //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.Jwt.Secret))
    //    };
    //    // السماح باستخدام التوكن في الكوكيز
    //    options.Events = new JwtBearerEvents
    //    {
    //        OnMessageReceived = context =>
    //        {
    //            if (context.Request.Cookies.ContainsKey("access_token"))
    //            {
    //                context.Token = context.Request.Cookies["access_token"];
    //            }
    //            return Task.CompletedTask;
    //        }
    //    };
    //})
    //.AddCookie()
    //;
    //}

    public static async Task AddPermissionClaim(this RoleManager<ApplicationRole> roleManager, ApplicationRole role, string[] permissions)
    {
        var allClaims = await roleManager.GetClaimsAsync(role);
        foreach (var permission in permissions)
        {
            if (!allClaims.Any(a => a.Type == "Permission" && a.Value == permission))
            {
                await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
            }
        }
    }
}
