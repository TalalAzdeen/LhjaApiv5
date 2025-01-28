using ASG.Helper;
using FluentResults;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.Services
{

    public class TokenService(IOptions<AppSettings> options)
    {
        private readonly ILogger _logger;
        private AppSettings AppSettings => options.Value;

        public string GenerateTemporaryToken(string requestId, string eventId, string clientId)
        {
            var claims = new[]
            {
            new Claim("RequestId", requestId),
            new Claim("EventId", eventId),
            new Claim("ClientId", clientId),
            //new Claim("tokenService", tokenService),
            new Claim(JwtRegisteredClaimNames.Exp, DateTimeOffset.UtcNow.AddDays(1).ToUnixTimeSeconds().ToString())
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSettings.Jwt.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: AppSettings.Jwt.validIssuer,
                audience: AppSettings.Jwt.ValidAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public Result<ClaimsPrincipal> ValidateToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return Result.Fail("Invalid data");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(AppSettings.Jwt.Secret);

            try
            {
                var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = AppSettings.Jwt.validIssuer,
                    ValidAudience = AppSettings.Jwt.ValidAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateLifetime = true // تحقق من انتهاء الصلاحية
                }, out SecurityToken validatedToken);



                return Result.Ok(claimsPrincipal);
            }
            catch (Exception ex)
            {
                return Result.Fail(new Error($"Token validation failed: {ex.Message}").CausedBy(ex));
            }
        }
    }

}
