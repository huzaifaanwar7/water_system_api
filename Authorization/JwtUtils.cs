using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using GBS.Api;
using GBS.Api.DbModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public interface IJwtUtils
{
    public string GenerateJwtToken(User user);
    public int? ValidateJwtToken(string token);
}

public class JwtUtils(IOptions<AppSettings> _appSettings) : IJwtUtils
{
    public string GenerateJwtToken(User user)
    {
        // generate token that is valid for 7 days
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.Value.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] 
            { 
                new Claim("Id", user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            Issuer = _appSettings.Value.LDAPDomain,
            Audience = _appSettings.Value.LDAPDomain,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public int? ValidateJwtToken(string token)
    {
        if (token == null)
            return null;

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.Value.Secret);
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _appSettings.Value.LDAPDomain,
                ValidAudience = _appSettings.Value.LDAPDomain,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = Convert.ToInt32(jwtToken.Claims.First(x => x.Type == "Id").Value);
            return userId;
        }
        catch
        {
            return null;
        }
    }
}