using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Service
    {
        [Key]
        public string Id { get; set; } = $"serv_{Guid.NewGuid():N}";
        public required string Name { get; set; }
        public required string AbsolutePath { get; set; }
        public required string Token { get; set; }

        public string? ModelAiId { get; set; }
        public ModelAi? ModelAi { get; set; }
        public ICollection<ServiceMethod> ServiceMethods { get; set; } = [];
        public ICollection<PlanServices> PlanServices { get; set; } = [];
        public ICollection<Request> Requests { get; set; } = [];
    }


}
