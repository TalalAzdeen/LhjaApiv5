using Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data;

public class DataContext
    : IdentityDbContext<
        ApplicationUser, ApplicationRole, string,
        ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin,
        ApplicationRoleClaim, ApplicationUserToken>
{
    //public override DatabaseFacade? Database { get;}
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<ApplicationRole> ApplicationRoles { get; set; }
    public DbSet<ApplicationUserClaim> ApplicationUserClaims { get; set; }
    public DbSet<Plan> Plans { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }

    public DbSet<ModelGateway> ModelGateways { get; set; }
    public DbSet<ModelAi> ModelAis { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<ServiceMethod> ServiceMethods { get; set; }
    public DbSet<Request> Requests { get; set; }
    public DbSet<EventRequest> EventRequests { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<PlanServices> PlanServices { get; set; }


    public DbSet<Setting> Settings { get; set; }
    public DbSet<Space> Spaces { get; set; }
    public DbSet<PlanFeature> PlanFeatures { get; set; }
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);

        modelBuilder.Entity<Request>().Navigation(e => e.Events).AutoInclude();
    }


}
