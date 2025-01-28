using AutoMapper;
using Stripe;

namespace StripeGateway
{
    public interface IStripeSubscription
    {
        Task<Subscription> CreateAsync(SubscriptionCreateOptions options, CancellationToken cancellationToken = default);
        Task<Subscription> CancelAsync(string id, CancellationToken cancellationToken = default);
        Task<StripeList<Subscription>> GetAllAsync(string customerId = null, CancellationToken cancellationToken = default);
        Task<Subscription> GetByIdAsync(string subscriptionId, CancellationToken cancellationToken = default);
        Task<Subscription> UpdateAsync(string id, SubscriptionUpdateOptions options, CancellationToken cancellationToken = default);
        Task<Subscription> ResumeAsync(string id, SubscriptionResumeOptions options, CancellationToken cancellationToken = default);
    }

    public class StripeSubscription(IMapper mapper) : IStripeSubscription
    {
        public async Task<Subscription> CreateAsync(SubscriptionCreateOptions options, CancellationToken cancellationToken = default)
        {
            var service = new SubscriptionService();
            var subscription = await service.CreateAsync(options);
            return subscription;
        }


        public async Task<Subscription> UpdateAsync(string id, SubscriptionUpdateOptions options, CancellationToken cancellationToken = default)
        {
            var service = new SubscriptionService();
            var subscription = await service.UpdateAsync(id, options);
            return subscription;
        }


        public async Task<Subscription> ResumeAsync(string id, SubscriptionResumeOptions options, CancellationToken cancellationToken = default)
        {
            var service = new SubscriptionService();
            var subscription = await service.ResumeAsync(id, options);
            return subscription;
        }

        public async Task<StripeList<Subscription>> GetAllAsync(string? customerId, CancellationToken cancellationToken)
        {
            var service = new SubscriptionService();
            var subscription = await service
                .ListAsync(new SubscriptionListOptions { Customer = customerId });
            //var result = mapper.Map<StripeList<Subscription>>(subscription);
            return subscription;
        }

        public async Task<Subscription> GetByIdAsync(string subscriptionId, CancellationToken cancellationToken = default)
        {
            var service = new SubscriptionService();
            var subscription = await service.GetAsync(subscriptionId);
            var result = mapper.Map<Subscription>(subscription);
            return result;
        }

        public async Task<Subscription> CancelAsync(string id, CancellationToken cancellationToken = default)
        {
            var service = new SubscriptionService();
            var subscription = await service.CancelAsync(id);
            var result = mapper.Map<Subscription>(subscription);
            return result;
        }


    }
}
