using Dto.PlanServices;

namespace Dto.Plan
{
    public class PlanResponse
    {
        public string Id { get; set; }
        public string ProductName { get; set; }
        public string ProductId { get; set; }
        public string BillingPeriod { get; set; }
        public int NumberRequests { get; set; }
        public decimal Amount { get; set; }
        public bool Active { get; set; }
        public string Description { get; set; }
        public string[]? Images { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<PlanServicesResponse> planServices { get; set; }
        //public ICollection<PlanServiceResponse> Services { get; set; }

    }
}
