using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stockify.Models;
using Stockify.Business;
using System.Security.Claims;

namespace Stockify.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpGet]
        [Authorize(Roles = Roles.Reader + "," + Roles.Admin + "," + Roles.Tenant)]// Política de autorización para leer inventarios
        public IActionResult GetAll()
        {
            var inventories = _inventoryService.GetAll();
            return Ok(inventories);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = Roles.Reader + "," + Roles.Admin + "," + Roles.Tenant)] // Política de autorización para leer inventarios
        public IActionResult Get(int id)
        {
            var inventory = _inventoryService.Get(id);
            if (inventory == null)
            {
                return NotFound();
            }
            return Ok(inventory);
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin + "," + Roles.Tenant)]// Política de autorización para agregar inventarios
        public IActionResult Add([FromBody] InventoryCreateDto inventoryCreateDto)
        {
            var tenantId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var inventory = new Inventory
            {
                Name = inventoryCreateDto.Name,
                CreationDate = inventoryCreateDto.CreationDate,
                TenantId = tenantId

            };
            _inventoryService.Add(inventory);
            return CreatedAtAction(nameof(Get), new { id = inventory.Id }, inventory);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Admin + "," + Roles.Tenant)] // Política de autorización para actualizar inventarios
        public IActionResult Update(int id, Inventory inventory)
        {
            if (id != inventory.Id)
            {
                return BadRequest();
            }
            _inventoryService.Update(inventory);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Tenant)] // Política de autorización para eliminar inventarios
        public IActionResult Delete(int id)
        {
            _inventoryService.Delete(id);
            return NoContent();
        }
    }
}
