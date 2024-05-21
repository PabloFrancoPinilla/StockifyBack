using Microsoft.AspNetCore.Mvc;
using Stockify.Business;
using Stockify.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

namespace Stockify.API.Controllers
{
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
        public IActionResult IniciarSesion([FromBody] UserLogin userLogin)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var user = _authService.Login(userLogin);

                if (user == null)
                    return NotFound("Usuario no encontrado.");

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["JWT:SecretKey"]);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim("LastName", user.LastName),
                    new Claim("TenantName", user.TenantName),
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
                return BadRequest(new { message = "Ocurrió un error al intentar iniciar sesión.", error = ex.Message });
            }
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public IActionResult RegistrarSesion([FromBody] UserCreateDto userCreateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var user = _authService.Register(userCreateDto);

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
