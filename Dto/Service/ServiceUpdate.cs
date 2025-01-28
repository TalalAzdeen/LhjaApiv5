using System.ComponentModel.DataAnnotations;

namespace Dto.Service
{
    public class ServiceUpdate
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Token { get; set; }

        [Required]
        public string ModelAiId { get; set; }
    }
}
