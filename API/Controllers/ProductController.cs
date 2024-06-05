using Microsoft.AspNetCore.Mvc;
using Stockify.Models;
using Stockify.Business;
using Microsoft.AspNetCore.Authorization;

namespace TeteProduct.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _ProductService;
        private readonly ITransactionService _TransactionService;

        public ProductController(IProductService ProductService, ITransactionService TransactionService)
        {
            _ProductService = ProductService;
            _TransactionService = TransactionService;
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            var inventories = _ProductService.GetAll();
            return Ok(inventories);
        }


        [HttpGet("{id}")]
        [Authorize(Roles = Roles.Reader + "," + Roles.Admin + "," + Roles.Tenant)]
        public IActionResult Get(int id)
        {
            var Product = _ProductService.Get(id);
            if (Product == null)
            {
                return NotFound();
            }
            return Ok(Product);
        }

        [HttpGet("{id}/transactions")]
        [Authorize(Roles = Roles.Reader + "," + Roles.Admin + "," + Roles.Tenant)]
        public IActionResult GetTransactionsByProductId(int id)
        {
            var transactions =_TransactionService.GetTransactionsByProductId(id);
            if (transactions == null)
            {
                return NotFound();
            }
            return Ok(transactions);
        }


        [HttpPost]
        [Authorize(Roles = Roles.Admin + "," + Roles.Tenant)]
        public IActionResult Add([FromBody] ProductCreateDto productCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = _ProductService.Add(productCreateDto, HttpContext);
            return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Admin + "," + Roles.Tenant)]
        public IActionResult Update(int id, ProductUpdateDto Product)
        {
            if (id != Product.Id)
            {
                return BadRequest();
            }
            _ProductService.Update(Product, HttpContext);
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
