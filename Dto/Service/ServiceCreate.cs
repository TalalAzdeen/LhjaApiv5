using System.ComponentModel.DataAnnotations;

namespace Dto.Service
{
    public class ServiceCreate
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Token { get; set; }

        public string? ModelAiId { get; set; }

        public  string? AbsolutePath { get; set; }
    }
}
