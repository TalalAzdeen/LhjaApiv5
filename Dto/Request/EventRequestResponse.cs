namespace Dto.Request
{
    public class EventRequestResponse
    {
        public required string Id { get; set; }
        public required string Status { get; set; }
        public string? Details { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
