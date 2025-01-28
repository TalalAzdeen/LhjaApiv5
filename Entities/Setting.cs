using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Setting
    {
        [Key]
        public required string Name { get; set; }
        public string? Value { get; set; }
    }
}
