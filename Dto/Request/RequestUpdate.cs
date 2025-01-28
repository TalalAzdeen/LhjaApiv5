using System.ComponentModel.DataAnnotations;

namespace Dto.Request
{
    public class RequestUpdate
    {
        [Required]
        public string Status { get; set; }
        public string? Details { get; set; }

        //[Required]
        //public string SubscriptionId { get; set; }
        //[Required]
        //public string ServiceId { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
