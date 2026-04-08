using Microsoft.AspNetCore.Mvc;
using Stockify.Models;
using Stockify.Business;

namespace Stockify.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _TransactionService;

        public TransactionController(ITransactionService TransactionService)
        {
            _TransactionService = TransactionService;
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            var inventories = _TransactionService.GetAll(HttpContext);
            return Ok(inventories);
        }


        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var Transaction = _TransactionService.Get(id);
            if (Transaction == null)
            {
                return NotFound();
            }
            return Ok(Transaction);
        }


        [HttpPost]
        public IActionResult Add(Transaction Transaction)
        {
            _TransactionService.Add(Transaction);
            return CreatedAtAction(nameof(Get), new { id = Transaction.Id }, Transaction);
        }


        [HttpPut("{id}")]
        public IActionResult Update(int id, Transaction Transaction)
        {
            if (id != Transaction.Id)
            {
                return BadRequest();
            }
            _TransactionService.Update(Transaction);
            return NoContent();
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _TransactionService.Delete(id);
            return NoContent();
        }
    }
}
