using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Transaction.API.Database;
using Transaction.API.Models;
using Plain.RabbitMQ;
using Shared.Models;
using Microsoft.EntityFrameworkCore;
using Transaction.API.Services.Interface;

namespace Transaction.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly IAccountTransaction _transactionRepository;
        public TransactionController(IAccountTransaction transactionRepository)
        {
            _transactionRepository=transactionRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountTransaction>>> GetTransactions()
        {
            var result = await _transactionRepository.GetAllTransactions();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AccountTransaction>> GetTransaction(int id)
        {
            var transaction = await _transactionRepository.GetTransaction(id);
            if (transaction == null)
                return NotFound();
            return Ok(transaction);
        }

        
        [HttpPost]
        public async Task<IActionResult> PostTransaction(AccountTransaction accountTransaction)
        {
            var result=_transactionRepository.PostTransaction(accountTransaction);
            return Ok(result);
        }

        [HttpPut]

        public async Task<IActionResult> PutTransaction(int id, AccountTransaction accountTransaction)
        {
            var result=_transactionRepository.PutTransaction(id,accountTransaction);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var transaction = await _transactionRepository.GetTransaction(id);
            if(transaction==null)
            {
                return NotFound();
            }
            var result=_transactionRepository.DeleteTransaction(transaction);
            return NoContent();
        }
    }

}