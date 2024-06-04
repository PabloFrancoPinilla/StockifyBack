using Microsoft.AspNetCore.Mvc;
using Stockify.Models;
using Stockify.Business;
using Microsoft.AspNetCore.Authorization;

namespace Stockify.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _CategoryService;

        public CategoryController(ICategoryService CategoryService)
        {
            _CategoryService = CategoryService;
        }


       


        [HttpGet("{id}")]
        [Authorize(Roles = Roles.Reader + "," + Roles.Admin + "," + Roles.Tenant)]
        public IActionResult Get(int id)
        {
            var Category = _CategoryService.Get(id);
            if (Category == null)
            {
                return NotFound();
            }
            return Ok(Category);
        }


        [HttpPost]
        [Authorize(Roles = Roles.Admin + "," + Roles.Tenant)]
        public IActionResult Add(CategoryCreateDto categoryCreateDto)
        {
            var Category = new Category
            {
                Name = categoryCreateDto.Name,
                InventoryId = categoryCreateDto.InventoryId
            };
            _CategoryService.Add(Category);
            return CreatedAtAction(nameof(Get), new { id = Category.Id }, Category);
        }


        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Admin + "," + Roles.Tenant)]
        public IActionResult Update(int id, Category Category)
        {
            if (id != Category.Id)
            {
                return BadRequest();
            }
            _CategoryService.Update(Category);
            return NoContent();
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Admin + "," + Roles.Tenant)]
        public IActionResult Delete(int id)
        {
            _CategoryService.Delete(id);
            return NoContent();
        }
    }
}
