using System.ComponentModel.DataAnnotations;

namespace Dto.Role
{
    public class RoleCreate
    {
        [Required]
        public string Name { get; set; }
    }
}
