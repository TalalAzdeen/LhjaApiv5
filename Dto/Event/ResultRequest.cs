namespace Utilities
{
    public class ResultRequest
    {
        public required string Token { get; set; }
        public string? Result { get; set; }
        public string Status { get; set; }
        //public required string PrivateKey { get; set; }
    }
}
