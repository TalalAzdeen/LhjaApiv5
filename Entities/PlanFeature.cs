using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Utilities;

namespace Entities
{
    public class PlanFeature
    {


        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }
        [Required] public string? Description { get; set; }
        public string? PlanId { get; set; }

        public Plan? Plan { get; set; }
    }
}
