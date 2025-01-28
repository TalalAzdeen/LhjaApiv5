using Dto.Subscription;

namespace Dto.User;

public class UserResponse
{
    //public required string Id { get; init; }
    public required string Email { get; init; }
    public string? CustomerId { get; init; }
    public string? SubscriptionId { get; init; }
    public string? PhoneNumber { get; init; }
    public bool IsEmailConfirmed { get; init; }
    public bool IsPhoneNumberConfirmed { get; init; }
    public string? Role { get; init; }
    public SubscriptionResponse Subscription { get; init; }

}
