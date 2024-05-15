/* using Microsoft.AspNetCore.Mvc;
using Stockify.Models;
using Stockify.Business;

namespace TeteUser.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IUserService _UserService;

        public LoginController(IUserService UserService)
        {
            _UserService = UserService;
        }

        [HttpPost]
        public ActionResult<User> Get(string mail, string Password)
        {
            var res = _UserService.Login(mail, Password);
            return Ok(res);
        }
    }
} */
