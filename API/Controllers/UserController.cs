using Microsoft.AspNetCore.Mvc;
using Stockify.Models;
using Stockify.Business;
using Stockify.Data;

namespace  Stockify.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _UserService;
        private readonly StockifyContext _context;

        public UserController(IUserService UserService,StockifyContext context)
        {
            _UserService = UserService;
            _context = context;
            
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            var inventories = _UserService.GetAll();
            return Ok(inventories);
        }


        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var User = _UserService.Get(id);
            if (User == null)
            {
                return NotFound();
            }
            return Ok(User);
        }
       


        [HttpPost]
        public IActionResult Add(UserCreateDto userCreateDto)
        {
           var userDto = _UserService.Add(userCreateDto,HttpContext);
        return Ok(userDto);
        }


        [HttpPut("{id}")]
        public IActionResult Update(int id, User User)
        {
            if (id != User.Id)
            {
                return BadRequest();
            }
            _UserService.Update(User);
            return NoContent();
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _UserService.Delete(id);
            return NoContent();
        }
    }
}
