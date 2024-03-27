using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Transaction.API.Database;
using Transaction.API.Models;
using Plain.RabbitMQ;
using Shared.Models;
using Microsoft.EntityFrameworkCore;
using Transaction.API.Services.Interface;
using Transaction.API.DTO;
using AutoMapper;

namespace Transaction.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly IAccountTransaction _transactionRepository;
          private readonly IMapper _mapper;
        public TransactionController(IAccountTransaction transactionRepository,IMapper mapper)
        {
            _transactionRepository=transactionRepository;
            _mapper=mapper;
        }

        // [HttpGet]
        // public async Task<ActionResult<IEnumerable<AccountTransaction>>> GetTransactions()
        // {
        //     var result = await _transactionRepository.GetAllTransactions();
        //     return Ok(result);
        // }

          [HttpGet("GetAllTransactions")]
        public async Task<ActionResult<IEnumerable<AccountTransactionResponse>>> GetTransactions()
        {
            var result = await _transactionRepository.GetAllTransactions();
            var accountTransactions=_mapper.Map<IEnumerable<AccountTransactionResponse>>(result);
            return Ok(accountTransactions);
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
        public async Task<IActionResult> PostTransaction([FromBody]AccountTransactionRequest accountTransactionRequestDto)
        {
            var accountTransaction = _mapper.Map<AccountTransaction>(accountTransactionRequestDto);
             var result = await _transactionRepository.PostTransaction(accountTransaction);
             var addedAccountTransactionDto = _mapper.Map<AccountTransactionResponse>(result);
            return Ok(addedAccountTransactionDto);
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