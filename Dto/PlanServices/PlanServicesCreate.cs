using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Utilities;
using static System.String;

namespace Dto.PlanServices
{
    public class PlanServicesCreate
    {
        [Required(ErrorMessage = "Number requests is Required")]
        [Range(10, int.MaxValue, ErrorMessage = "The number must be greater than 10"), DefaultValue(0)]
        public long NumberRequests { get; set; } = 0;
        [DefaultValue(1)] public int Scope { get; set; } = 1;
        public ProcessorType Processor { get; set; }

        [EnumDataType(typeof(ConnectionType))]

        public ConnectionType ConnectionType { get; set; }

        [Required]
        public string ServiceId { get; set; } = Empty;
    }
}
