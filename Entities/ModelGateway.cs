using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class ModelGateway
    {
        [Key]
        public string Id { get; set; } = $"modg_{Guid.NewGuid():N}";
        public string Name { get; set; }
        public string Url { get; set; }
        public string? Token { get; set; }
        public bool IsDefault { get; set; }
    }
}
