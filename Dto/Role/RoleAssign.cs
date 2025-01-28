using System.ComponentModel.DataAnnotations;

namespace Dto.Role
{
    public class RoleAssign
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string RoleId { get; set; }
    }
}
