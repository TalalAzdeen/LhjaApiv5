using Api;
using Api.CustomPolicy;
using Api.Repositories.DachRepository;
using Api.Seeds;
using Api.ServiceLayer.LayerModel;
using ASG.ApiService.CustomPolicy;
using ASG.ApiService.OutputCaching;
using ASG.Helper;
using CardShapping.Api.RepositoryAPI.SerchRepository;
using Data;
using Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text.Json.Serialization;
using Utilities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddProblemDetails();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.RespectRequiredConstructorParameters = true;
    });



builder.Services.Configure<JsonOptions>(options =>
{
    options.JsonSerializerOptions.RespectRequiredConstructorParameters = true;
});

builder.Logging.AddConsole().SetMinimumLevel(LogLevel.Debug);

builder.Services.AddOutputCache(options =>
{
    options.AddBasePolicy(b => b.AddPolicy<CustomePolicy>(), true);
});

// Scoped Repositories
builder.Services.AddApiServices(builder.Configuration);

var appSettings = builder.Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();

#region External Services
Log.Logger = new LoggerConfiguration().MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("logs/myLog-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
//builder.Logging.ClearProviders();
//builder.Logging.AddConsole();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services
    .AddStripeGateway(builder.Configuration)
    .AddDataContext(builder.Configuration)
    ;
#endregion

builder.Services.AddTransient<IClaimsTransformation, MyClaimsTransformation>();

builder.Services.AddDynamicAuthentication();
//builder.Services.AddJwtAuthentication(appSettings);

// Configure authorization
builder.Services.AddAuthorizationBuilder();


// Add identity and opt-in to endpoints
builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedEmail = true;
})
    .AddRoles<ApplicationRole>()
    .AddEntityFrameworkStores<DataContext>()
    .AddDefaultTokenProviders()
    //.AddUserConfirmation<ApplicationUser>()
    .AddApiEndpoints();


builder.Services.Configure<IdentityOptions>(options =>
{
    /*
     *  يسمح للمستخدمين الجدد بتجربة تسجيل الدخول دون القلق من حظر حساباتهم.
     */
    options.Lockout.AllowedForNewUsers = true;
});

builder.Services.AddScoped<IModelService, ModelService>();
builder.Services.AddScoped(typeof(ISearchRepository<>), typeof(SearchRepository<>));


builder.Services.AddScoped<IPlanVisualizationRepository, PlanVisualizationRepository>();
builder.Services.AddScoped<IRequestVisualizationRepository, RequestVisualizationRepository>();
builder.Services.AddScoped<IServiceVisualizationRepository, ServiceVisualizationRepository>();



builder.Services.AddScoped<IAuthorizationHandler, PermissionHandler>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanViewPlan", policy =>
        policy.Requirements.Add(new PermissionRequirement("Permission", Permissions.ViewPlan))
        ); // Example: Requires a claim
});

// Add a CORS policy for the client
builder.Services.AddCors(
    options => options.AddPolicy(
        "wasm",
        policy => policy.WithOrigins([builder.Configuration["appsettings:baseurls:api"] ?? "https://localhost:7001",
            builder.Configuration["FrontendUrl"] ?? "https://localhost:7003"])
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()));

builder.Services.AddEndpointsApiExplorer();




// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerGen(c =>
{
    //c.EnableAnnotations();
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert token with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
            },
            Array.Empty<string>()
        }
    });
    // تخصيص Swagger لتحديد القيم
    //c.OperationFilter<CustomProductParameterFilter>();
});

var app = builder.Build();


SeedData.EnsureSeedData(app);

//app.MapDefaultEndpoints();
// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(o =>
    {
        //o.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
        //o.RoutePrefix = string.Empty;
        // collapse endpoints 
        o.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);

    });
}
app.MapSwagger();
app.CustomMapIdentityApi<ApplicationUser>();
app.UseHttpsRedirection();

// Activate the CORS policy
app.UseCors("wasm");


//app.UseOutputCache();
//app.UseMiddleware<DynamicAuthenticationMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();
app.MapControllers().RequireAuthorization();

// protection from cross-site request forgery (CSRF/XSRF) attacks with empty body
// form can't post anything useful so the body is null, the JSON call can pass
// an empty object {} but doesn't allow cross-site due to CORS.
app.MapPost("/api/logout", async (
    SignInManager<ApplicationUser> signInManager,
    [FromBody] object empty) =>
{
    if (empty is not null)
    {
        await signInManager.SignOutAsync();
        return Results.Ok();
    }
    return Results.NotFound();
}).RequireAuthorization().WithTags("Auth");

//app.UseOutputCache();

app.Run();
