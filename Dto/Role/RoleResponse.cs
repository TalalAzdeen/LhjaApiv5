namespace Dto.Role
{
    public class RoleResponse
    {
        public string? Id { get; init; }
        public string? Name { get; init; }

        public string[] Permissions { get; init; }
    }
}
