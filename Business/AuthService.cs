namespace Stockify.Business;
using Stockify.Data;
using Stockify.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.Serialization;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _repository;
    private readonly IUserService _service;

    public AuthService(IConfiguration configuration, IUserRepository repository, IUserService service)
    {
        _configuration = configuration;
        _repository = repository;
        _service = service;
    }

    public string Login(UserLogin userLogin)
    {
        var user = _repository.GetUserFromCredentials(userLogin);
        return GenerateToken(user);
    }
    public string Register(UserCreateDto userCreateDto)
    {
        var userout = _service.Add(userCreateDto);
        return GenerateToken(userout);
    }
    public string GenerateToken(UserDto userDto)
    {
        var key = Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _configuration["JWT:ValidIssuer"],
            Audience = _configuration["JWT:ValidAudience"],
            Subject = new ClaimsIdentity(new Claim[]
            {
            new Claim(ClaimTypes.NameIdentifier, userDto.Id.ToString()),
            new Claim(ClaimTypes.Name, userDto.Name),
            new Claim("LastName",userDto.LastName),
             new Claim("TenantName",userDto.TenantName),
            new Claim(ClaimTypes.Role, userDto.Role)
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return tokenString;
    }


}