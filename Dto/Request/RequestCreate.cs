using System.ComponentModel.DataAnnotations;

namespace Dto.Request
{
    public class RequestCreate
    {
        [Required]
        public string Value { get; set; }


        //[Required]
        //public string UserId { get; set; }

        //[Required]
        //public string SubscriptionId { get; set; }
        [Required]
        public string ServiceId { get; set; }

    }
}
