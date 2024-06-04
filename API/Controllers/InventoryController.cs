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
        private readonly IProductService _productService;
         private readonly ICategoryService _categoryService;

        public InventoryController(IInventoryService inventoryService, IProductService productService, ICategoryService categoryService)
        {
            _inventoryService = inventoryService;
            _productService = productService;
            _categoryService = categoryService;
        }

        [HttpGet]
        [Authorize(Roles = Roles.Reader + "," + Roles.Admin + "," + Roles.Tenant)]// Política de autorización para leer inventarios
        public IActionResult GetAll()
        {
            var inventories = _inventoryService.GetAll(HttpContext);
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
        [HttpGet("{inventoryId}/products")]
        [Authorize(Roles = Roles.Reader + "," + Roles.Admin + "," + Roles.Tenant)]
        public IActionResult GetProductsByInventoryId(int inventoryId)
        {
            var products = _productService.GetProductsByInventoryId(inventoryId);
            if (products == null || products.Count == 0)
            {
                return NotFound();
            }
            return Ok(products);
        }
         [HttpGet("{inventoryId}/categories")]
        [Authorize(Roles = Roles.Reader + "," + Roles.Admin + "," + Roles.Tenant)]
        public IActionResult GetCategoriesByInventoryId(int inventoryId)
        {
            var inventories = _categoryService.GetCategoriesByInventoryId(inventoryId);
            return Ok(inventories);
        }


        [HttpPost]
        [Authorize(Roles = Roles.Admin + "," + Roles.Tenant)] // Política de autorización para agregar inventarios
        public IActionResult Add([FromBody] InventoryCreateDto inventoryCreateDto)
        {
            var tenantId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var inventory = new Inventory
            {
                Name = inventoryCreateDto.Name,
                CreationDate = inventoryCreateDto.CreationDate,
                Description = inventoryCreateDto.Description,
                Color = inventoryCreateDto.Color,
                TenantId = tenantId,
                Image = inventoryCreateDto.Image,
                Location = inventoryCreateDto.Location
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
