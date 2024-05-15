using Microsoft.AspNetCore.Mvc;
using Stockify.Models;
using Stockify.Business;

namespace TeteProduct.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _ProductService;

        public ProductController(IProductService ProductService)
        {
            _ProductService = ProductService;
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            var inventories = _ProductService.GetAll();
            return Ok(inventories);
        }


        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var Product = _ProductService.Get(id);
            if (Product == null)
            {
                return NotFound();
            }
            return Ok(Product);
        }


        [HttpPost]
        public IActionResult Add(Product Product)
        {
            _ProductService.Add(Product);
            return CreatedAtAction(nameof(Get), new { id = Product.Id }, Product);
        }


        [HttpPut("{id}")]
        public IActionResult Update(int id, Product Product)
        {
            if (id != Product.Id)
            {
                return BadRequest();
            }
            _ProductService.Update(Product);
            return NoContent();
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _ProductService.Delete(id);
            return NoContent();
        }
    }
}
