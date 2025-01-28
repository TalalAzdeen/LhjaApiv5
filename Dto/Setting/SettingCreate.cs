using System.ComponentModel.DataAnnotations;

namespace Dto.Setting
{
    public class SettingCreate
    {
        [Required]
        public string Name { get; set; }

        public string? Value { get; set; }
    }
}
