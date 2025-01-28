namespace Dto.Request
{
    public class RequestAllowed
    {
        public long NumberRequests { get; set; }
        public long CurrentNumberRequests { get; set; }

        public bool Allowed { get; set; }
    }
}
