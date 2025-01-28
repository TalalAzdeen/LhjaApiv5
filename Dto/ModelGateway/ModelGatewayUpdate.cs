using System.ComponentModel.DataAnnotations;

namespace Dto.ModelGateway
{
    public class ModelGatewayUpdate
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Url { get; set; }
        [Required]
        public string Token { get; set; }
        //public bool IsDefault { get; set; } = false;
    }
}
