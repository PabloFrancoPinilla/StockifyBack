using Microsoft.AspNetCore.Mvc;
using Stockify.Models;
using Stockify.Business;

namespace Stockify.Controllers;

[ApiController]
[Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Login")]
        public IActionResult Login(UserLogin userLogin)
        {
            try
            {
                if (!ModelState.IsValid)  {return BadRequest(ModelState); } 

                var token = _authService.Login(userLogin);
                return Ok(token);
            }
            catch (KeyNotFoundException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest
                ("Error generating the token: " + ex.Message);
            }
        }
        [HttpPost("Register")]
        public IActionResult Register(UserCreateDto userCreateDto)
        {
            try
            {
                if (!ModelState.IsValid)  {return BadRequest(ModelState); } 

                var token = _authService.Register(userCreateDto);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest
                ("Error generating the token: " + ex.Message);
            }
        }

        
    }