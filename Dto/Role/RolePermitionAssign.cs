using System.ComponentModel.DataAnnotations;

namespace Dto.Role
{
    public class RolePermitionAssign
    {
        [Required]
        public string RoleId { get; set; }
        [Required, MinLength(1)]
        public string[] Permissions { get; set; }
    }
}
