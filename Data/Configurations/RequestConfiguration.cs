using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations
{
    internal sealed class RequestConfiguration : IEntityTypeConfiguration<Request>
    {
        public void Configure(EntityTypeBuilder<Request> builder)
        {
            builder
            .HasOne(s => s.User)
            .WithOne()
            .HasForeignKey<Request>(s => s.UserId)
            .OnDelete(DeleteBehavior.NoAction);


            builder
            .HasOne(s => s.Subscription)
            .WithOne()
            .OnDelete(DeleteBehavior.NoAction);

            builder
            .HasOne(s => s.Service)
            .WithOne()
            .OnDelete(DeleteBehavior.NoAction);

            //builder.Navigation(r => r.Events).AutoInclude();
        }
    }
}
