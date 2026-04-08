using Microsoft.AspNetCore.Mvc;
using Stockify.Models;
using Stockify.Business;

namespace Stockify.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ITenantService _TenantService;

        public LoginController(ITenantService TenantService)
        {
            _TenantService = TenantService;
        }

        [HttpPost]
        public ActionResult<Tenant> Get(string username, string password)
        {
            var res = _TenantService.Login(username, password);
            return Ok(res);
        }
    }
}
