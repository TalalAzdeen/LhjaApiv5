using ASG.Helper;
using Dto.Auth;
using Entities;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Api.Repositories;

public interface IAuthRepository
{
    List<Claim> Claims { get; set; }

    Task<bool> ValidateCredentials(LoginRequest login);

    //Task<Result<object>> RegiterUser(RegisterRequest model);
    Task<AccessTokenResponse> GenerateToken();
}
public class AuthRepository(UserManager<ApplicationUser> userManager, IOptions<AppSettings> options, RoleManager<ApplicationRole> roleManager, SignInManager<ApplicationUser> signInManager) : IAuthRepository
{

    private ApplicationUser? _user;
    private List<Claim> claims;

    public List<Claim> Claims { get => claims; set => claims = value; }

    public async Task<bool> ValidateCredentials(LoginRequest login)
    {
        _user = await userManager.FindByNameAsync(login.Email);
        signInManager.AuthenticationScheme = IdentityConstants.BearerScheme;
        return _user != null && await userManager.CheckPasswordAsync(_user, login.Password);
    }
    public async Task<AccessTokenResponse> GenerateToken()
    {
        var principal = await signInManager.CreateUserPrincipalAsync(_user);

        claims = principal.Claims.ToList() ?? [];
        claims.AddRange([
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, _user.Id),
                new Claim(JwtRegisteredClaimNames.Iss, "Jwt Subject"),
            ]);

        var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(options.Value.Jwt.Secret));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: options.Value.Jwt.validIssuer,
            audience: options.Value.Jwt.ValidAudience,
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: credentials
            );

        return new AccessTokenResponse
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            RefreshToken = GenerateRefreshToken(),
            ExpiresIn = DateTime.Now.AddDays(5).Second,
        };

    }
    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
    //public async Task<Result<object>> RegiterUser2(RegisterRequest model)
    //{

    //    var existing = await userManager.FindByEmailAsync(model.Email);
    //    if (existing != null)
    //        return Result<object>.Failure(Error.ValidationError("This email is already in registered"));


    //    string user_Id = Guid.NewGuid().ToString();

    //    var identityResult = await userManager.CreateAsync(new ApplicationUser()
    //    {
    //        Id = user_Id,//(new BigInteger(Guid.NewGuid().ToByteArray())).ToString(),
    //        Email = model.Email,
    //        UserName = model.Email,
    //    }, model.Password);


    //    if (identityResult.Succeeded)
    //    {
    //        var applicationUser = await userManager.FindByEmailAsync(model.Email);
    //        if (applicationUser != null)
    //        {

    //            //var roleExist = await roleManager.RoleExistsAsync(model.UserRole.ToString());
    //            //if (!roleExist)
    //            //{
    //            //    await roleManager.CreateAsync(new ApplicationRole(model.UserRole.ToString()));
    //            //}
    //            //await userManager.AddToRoleAsync(applicationUser, model.UserRole.ToString());

    //            return new Result<object>("");
    //        }

    //    }

    //    return new Result<object>(Error.ValidationError("Con't register user"));
    //}


}
