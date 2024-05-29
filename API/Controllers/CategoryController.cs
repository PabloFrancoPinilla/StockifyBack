using Microsoft.AspNetCore.Mvc;
using Stockify.Models;
using Stockify.Business;

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


        [HttpGet]
        public IActionResult GetAll()
        {
            var inventories = _CategoryService.GetAll();
            return Ok(inventories);
        }


        [HttpGet("{id}")]
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
        public IActionResult Add(CategoryCreateDto categoryCreateDto)
        {
            var Category = new Category{
                Name = categoryCreateDto.Name
            };
            _CategoryService.Add(Category);
            return CreatedAtAction(nameof(Get), new { id = Category.Id }, Category);
        }


        [HttpPut("{id}")]
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
        public IActionResult Delete(int id)
        {
            _CategoryService.Delete(id);
            return NoContent();
        }
    }
}
