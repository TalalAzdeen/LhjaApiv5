namespace Dto.Event
{
    public class TokenValidationRequest
    {
        public required string Token { get; set; }
        public required string PublicKey { get; set; }
        //public required string PrivateKey { get; set; }
    }
}
