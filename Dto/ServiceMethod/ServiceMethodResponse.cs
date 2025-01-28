namespace Dto.ServiceMethod
{
    public class ServiceMethodResponse
    {
        public required string Id { get; set; }
        public required string Method { get; set; }
        public string[] InputParameters { get; set; }
        public string[] OutputParameters { get; set; }
    }
}
