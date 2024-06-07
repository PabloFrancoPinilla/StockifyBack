namespace Stockify.API.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.Extensions.Configuration;
    using Stockify.Business;
    using Stockify.Models;
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using Microsoft.IdentityModel.Tokens;

    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;

        public AuthController(IAuthService authService, IConfiguration configuration)
        {
            _authService = authService;
            _configuration = configuration;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = _authService.Login(loginRequest);

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["JWT:SecretKey"]);

                var claims = new List<Claim>();
                string role = null;

                if (loginRequest.IsTenant)
                {
                    var tenant = (TenantDto)result;
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, tenant.Id.ToString()));
                    claims.Add(new Claim("TenantId", tenant.Id.ToString()));
                    claims.Add(new Claim(ClaimTypes.Name, tenant.Name));
                    claims.Add(new Claim("Contact", tenant.Contact));
                    claims.Add(new Claim(ClaimTypes.Role, tenant.Role));
                    claims.Add(new Claim("Service", tenant.Service));
                    role = "Tenant";
                    
                }
                else
                {
                    var user = (UserDto)result;
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                    claims.Add(new Claim(ClaimTypes.Email, user.Email));
                    claims.Add(new Claim(ClaimTypes.Name, user.Name));
                    claims.Add(new Claim("LastName", user.LastName));
                    claims.Add(new Claim("TenantName", user.TenantName));
                    claims.Add(new Claim("TenantId", user.TenantId.ToString()));
                    claims.Add(new Claim(ClaimTypes.Role, user.Role));
                    role = user.Role;
                }

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddHours(2),
                    Issuer = _configuration["JWT:ValidIssuer"],
                    Audience = _configuration["JWT:ValidAudience"],
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);


                var response = new
                {
                    token = tokenString,
                    role = role
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Ocurrió un error al intentar iniciar sesión.", error = ex.Message });
            }
        }

        [HttpPost("Register")]
        [Authorize( Roles = Roles.Tenant)]
        public IActionResult RegistrarSesion([FromBody] UserCreateDto userCreateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var user = _authService.Register(userCreateDto, HttpContext);

                if (user == null)
                {
                    return Conflict("El usuario ya existe.");
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["JWT:SecretKey"]);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim("LastName", user.LastName),
                    new Claim("TenantName", user.TenantName),
                    new Claim("TenantId", user.TenantId.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                    
                };

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddHours(2),
                    Issuer = _configuration["JWT:ValidIssuer"],
                    Audience = _configuration["JWT:ValidAudience"],
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                return Ok(new { token = tokenString });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Ocurrió un error al intentar registrar el usuario.", error = ex.Message });
            }
        }

    }
}
