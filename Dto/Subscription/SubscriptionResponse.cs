using Dto.Plan;

namespace Dto.Subscription
{
    public class SubscriptionResponse
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string PlanId { get; set; }
        public string CustomerId { get; set; }
        public string BillingPeriod { get; set; }
        public DateTime StartDate { get; set; }
        public string Status { get; set; }
        public bool CancelAtPeriodEnd { get; set; }

        public PlanResponse Plan { get; set; }
    }
}
