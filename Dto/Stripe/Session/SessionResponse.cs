namespace Dto.Stripe.Session
{
    public class SessionResponse
    {
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public string Status { get; set; }
        public string Url { get; set; }
        public string SubscriptionId { get; set; }
    }
}
