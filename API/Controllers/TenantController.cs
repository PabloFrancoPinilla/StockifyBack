using Microsoft.AspNetCore.Mvc;
using Stockify.Models;
using Stockify.Business;

namespace Stockify.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TenantController : ControllerBase
    {
        private readonly ITenantService _TenantService;

        public TenantController(ITenantService TenantService)
        {
            _TenantService = TenantService;
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            var inventories = _TenantService.GetAll();
            return Ok(inventories);
        }


        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var Tenant = _TenantService.Get(id);
            if (Tenant == null)
            {
                return NotFound();
            }
            return Ok(Tenant);
        }


        [HttpPost]
        public IActionResult Add([FromBody] TenantCreateDto TenantCreateDto)
        {
            if (TenantCreateDto == null){
                return BadRequest();
            }
            var Tenant = new Tenant
            {
                Name = TenantCreateDto.Name,
                Password = TenantCreateDto.Password,
                Contact = TenantCreateDto.Contact
                
            };
            _TenantService.Add(Tenant);
            return CreatedAtAction(nameof(Get), new { id = Tenant.Id }, Tenant);
        }


        [HttpPut("{id}")]
        public IActionResult Update(int id, Tenant Tenant)
        {
            if (id != Tenant.Id)
            {
                return BadRequest();
            }
            _TenantService.Update(Tenant);
            return NoContent();
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _TenantService.Delete(id);
            return NoContent();
        }
    }
}
