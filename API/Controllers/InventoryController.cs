using Microsoft.AspNetCore.Mvc;
using Stockify.Models;
using Stockify.Business;

namespace TeteInventory.Controllers
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
        public IActionResult GetAll()
        {
            var inventories = _inventoryService.GetAll();
            return Ok(inventories);
        }


        [HttpGet("{id}")]
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
        public IActionResult Add(Inventory inventory)
        {
            _inventoryService.Add(inventory);
            return CreatedAtAction(nameof(Get), new { id = inventory.Id }, inventory);
        }


        [HttpPut("{id}")]
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
        public IActionResult Delete(int id)
        {
            _inventoryService.Delete(id);
            return NoContent();
        }
    }
}
