using Dto.Service;
using Dto.Subscription;
using Dto.User;

namespace Dto.Request
{
    public class RequestResponse
    {
        public required string Id { get; set; }
        public required string Status { get; set; }
        public required string Question { get; set; }
        public string? Answer { get; set; }
        public string? ModelGateway { get; set; }

        public string? ModelAi { get; set; }
        public required UserResponse User { get; set; }
        public required SubscriptionResponse Subscription { get; set; }
        public required ServiceResponse Service { get; set; }
        public ICollection<EventRequestResponse> Events { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
