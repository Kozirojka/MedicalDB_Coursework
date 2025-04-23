using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace MedicApp.Application.LogReg.Command;

public class GenerateAccessTokenCommandHandler : IRequestHandler<GenerateAccessTokenCommand, string>
{
    private readonly IConfiguration _configuration;

    public GenerateAccessTokenCommandHandler(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<string> Handle(GenerateAccessTokenCommand request, CancellationToken cancellationToken)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, request.User.Email),
            new Claim(ClaimTypes.Role, request.Role),
            new Claim("FirstName", request.User.Firstname),
            new Claim("LastName", request.User.Lastname)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddHours(1),
            SigningCredentials = creds,
            Issuer = _configuration["JWT:Issuer"],
            Audience = _configuration["JWT:Audience"]
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}