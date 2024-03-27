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
using Microsoft.AspNetCore.Http.HttpResults;

namespace Transaction.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly IAccountTransaction _transactionRepository;
        private readonly IMapper _mapper;
        public TransactionController(IAccountTransaction transactionRepository, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }

        [HttpGet("GetAllTransactions")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<AccountTransactionResponse>>> GetTransactions()
        {
            var result = await _transactionRepository.GetAllTransactions();
            var accountTransactions = _mapper.Map<IEnumerable<AccountTransactionResponse>>(result);
            return Ok(accountTransactions);
        }

        [HttpGet("GetTransaction/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<AccountTransactionResponse>> GetTransaction(int id)
        {
            var transaction = await _transactionRepository.GetTransaction(id);
            if (transaction == null)
                return NotFound();

            var transactionDto = _mapper.Map<AccountTransactionResponse>(transaction);
            return Ok(transactionDto);
        }

        [HttpPost("AddTransaction")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PostTransaction([FromBody] AccountTransactionRequest accountTransactionRequestDto)
        {
            var accountTransaction = _mapper.Map<AccountTransaction>(accountTransactionRequestDto);
            var result = await _transactionRepository.PostTransaction(accountTransaction);
            var addedAccountTransactionDto = _mapper.Map<AccountTransactionResponse>(result);
            return Ok(addedAccountTransactionDto);
        }


        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PutTransaction(int id, AccountTransactionRequest accountTransactionRequestDto)
        {
            var accountTransaction = _mapper.Map<AccountTransaction>(accountTransactionRequestDto);
            var result = _transactionRepository.PutTransaction(id, accountTransaction);
            return Ok();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var transaction = await _transactionRepository.GetTransaction(id);
            if (transaction == null)
            {
                return NotFound();
            }
            var result = _transactionRepository.DeleteTransaction(transaction);
            return Ok();
        }
    }

}