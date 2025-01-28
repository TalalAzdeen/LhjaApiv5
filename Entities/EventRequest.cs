using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class EventRequest
    {
        [Key]
        public string Id { get; set; } = $"event_{Guid.NewGuid():N}";

        public required string Status { get; set; }
        [DataType(DataType.MultilineText)]
        public string? Details { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public required string RequestId { get; set; }
        public Request Request { get; set; }

    }
}
