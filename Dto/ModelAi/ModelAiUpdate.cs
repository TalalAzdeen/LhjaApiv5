using System.ComponentModel.DataAnnotations;

namespace Dto.ModelAi
{
    public class ModelAiUpdate
    {
        [Required]
        public string Name { get; set; }
        public string? Token { get; set; }
        public string? AbsolutePath { get; set; }
    }
}
