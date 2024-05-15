using Microsoft.AspNetCore.Mvc;
using Stockify.Models;
using Stockify.Business;

namespace  Stockify.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _UserService;

        public UserController(IUserService UserService)
        {
            _UserService = UserService;
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
        public IActionResult Add(User User)
        {
            _UserService.Add(User);
            return CreatedAtAction(nameof(Get), new { id = User.Id }, User);
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
