using Account.API.Models;
using Account.API.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Account.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountRepository _accountRepository;
    public AccountController(IAccountRepository accountRepository)
    {
        _accountRepository=accountRepository;
    }

    [HttpGet("GetAllAccounts")]
    public async Task<ActionResult<IEnumerable<BankAccount>>> GetAllAccounts()
        {
            var result = await _accountRepository.GetAllBankAccounts();
            return Ok(result);
        }

     [HttpGet("GetBankAccount/{id}")]  

    public async Task<ActionResult<BankAccount>> GetBankAccount(int id)
        {
            var result=_accountRepository.GetBankAccount(id);
            return Ok(result);
        }

      [HttpPost("AddBankAccount")]   
       public async Task<IActionResult> AddBankAccount(BankAccount bankAccount)
        {
            var result = await _accountRepository.PostBankAccount(bankAccount);
            return Ok(result);  
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBankAccount(int id)
        {
            var bankAccount = await _accountRepository.GetBankAccount(id);
            if(bankAccount==null)
            {
                return NotFound();
            }

          var result=  _accountRepository.DeleteBankAccount(bankAccount);
          return NoContent();
        
        }

        [HttpPut("{id}")]
     public async Task<IActionResult> PutBankAccount(int id, BankAccount bankAccount)
        {

           var result= _accountRepository.PutBankAccount(id,bankAccount);
           return NoContent();
        }





}
