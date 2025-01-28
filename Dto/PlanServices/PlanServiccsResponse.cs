using Utilities;

namespace Dto.PlanServices
{
    public class PlanServicesResponse
    {
        public string? ServiceId { get; set; }
        public string? Name { get; set; }
        public string? Token { get; set; }
        public string? AbsolutePath { get; set; }
        public long NumberRequests { get; set; }
        public ProcessorType Processor { get; set; }
        public ConnectionType ConnectionType { get; set; }

    }
}
