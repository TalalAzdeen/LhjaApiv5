using System.ComponentModel.DataAnnotations;
using Utilities;

namespace Dto.PlanServices
{
    public class PlanServicesUpdate
    {
        public long NumberRequests { get; set; }

        public int Scope { get; set; }
        public ProcessorType Processor { get; set; }

        public ConnectionType ConnectionType { get; set; }

        [Required]
        public string ServiceId { get; set; }
    }
}
