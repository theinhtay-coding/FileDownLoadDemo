using JWTAuthDemo.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FileDownLoadDemo.Services;

public class AuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(AppDbContext context, IConfiguration configuration = null)
    {
        _context = context;
        _configuration = configuration;
    }

    public UserModel Authenticate(string email, string password)
    {
        // Hash the password and compare it with the stored hashed password
        var user = _context.Users.SingleOrDefault(x => x.Email == email && x.Password == password);

        if (user == null)
            return null;

        return user;
    }

    public string GenerateJwtToken(UserModel user)
    {
        var roleName = GetRoleNameById(user.RoleId);

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.UserId.ToString()),
                new Claim(ClaimTypes.Role, roleName)
            }),
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private string GetRoleNameById(int roleId)
    {
        var role = _context.Roles.SingleOrDefault(r => r.RoleId == roleId);
        return role?.Name ?? "Unknown";
    }
}
