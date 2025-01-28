using System.ComponentModel.DataAnnotations;

namespace Utilities
{
    public enum RequestTypes
    {
        [Display(Name = "Pending")]
        Pending,
        [Display(Name = "Processing")]
        Processing,
        [Display(Name = "Succeed")]
        Succeed,
        [Display(Name = "Failed")]
        Failed,
        [Display(Name = "Failed From Server")]
        FailedFromServer,
        [Display(Name = "Retry")]
        Retry
    }
}
