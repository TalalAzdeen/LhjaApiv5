using System.Text.Json;

namespace Dto.ServiceMethod
{
    public class ServiceMethodUpdate
    {

        public required string Method { get; set; }
        public string[] InputParameters { get; set; }
        public string[] OutputParameters { get; set; }

        public string? Inputs => JsonSerializer.Serialize(InputParameters);
        public string? Outputs => JsonSerializer.Serialize(OutputParameters);
    }
}
