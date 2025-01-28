using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Utilities;

namespace Entities
{
    public class TextLG
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Body { get; set; }


        [Required] public string? LG { get; set; }


  
   
    }
}
