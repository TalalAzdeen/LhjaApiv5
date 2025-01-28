namespace Dto.Role
{
    public class PermissionResponse
    {
        public string? RoleId { get; set; }
        public IList<RoleClaimsResponse> RoleClaims { get; set; }
    }

    public class RoleClaimsResponse
    {
        public string Type { get; set; }
        public string Value { get; set; }
        public bool Selected { get; set; }
    }
}
