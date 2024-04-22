using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

public class AuthUserService
{
    private readonly MongoDBContext _dbContext;
    private readonly IConfiguration _configuration;

    public AuthUserService(MongoDBContext dbContext, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _configuration = configuration;
    }

    public UserAuth Authenticate(string email, string password)
    {
        var user = _dbContext.GetCollection<UserAuth>("user").AsQueryable().FirstOrDefault(u => u.Email == email);

        if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
        {
            return user;
        }
        else
        {
            return null;
        }
    }

    public string GenerateJwtToken(UserAuth user)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var keyBytes = Convert.FromBase64String(_configuration["JWTSettings:SecretKey"]);
            var signingKey = new SymmetricSecurityKey(keyBytes);

            var claims = new List<Claim>
            {
                new Claim("id", user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Rol)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(1440),
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error al generar el token JWT.", ex);
        }
    }
}
