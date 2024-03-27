using Account.API.Models;
using Account.API.DTO;
using Account.API.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Account.API.Enums;

namespace Account.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountRepository _accountRepository;
    private readonly IMapper _mapper;
    public AccountController(IAccountRepository accountRepository, IMapper mapper)
    {
        _accountRepository=accountRepository;
        _mapper = mapper;
    }

    [HttpGet("GetAllAccounts")]
    public async Task<ActionResult<IEnumerable<BankAccountResponse>>> GetAllAccounts()
        {
            var result = await _accountRepository.GetAllBankAccounts();
            var bankAccounts = _mapper.Map<IEnumerable<BankAccountResponse>>(result);
            return Ok(bankAccounts);
        }

     [HttpGet("GetBankAccount/{id}")]  

    public async Task<ActionResult<BankAccount>> GetBankAccount(int id)
        {
            var result=_accountRepository.GetBankAccount(id);
            return Ok(result);
        }

        [HttpPost("AddBankAccount")]
       public async Task<IActionResult> AddBankAccount([FromBody] BankAccountRequest bankAccountRequestDto)
        {
            var bankAccount = _mapper.Map<BankAccount>(bankAccountRequestDto);
            var result = await _accountRepository.PostBankAccount(bankAccount);
            var addedBankAccountDto = _mapper.Map<BankAccountResponse>(result);
            return Ok(addedBankAccountDto);
        }

    //   [HttpPost("AddBankAccount")]   
    //    public async Task<ActionResult<BankAccountResponse>> AddBankAccount(BankAccount bankAccount)
    //     {
    //         var result = await _accountRepository.PostBankAccount(bankAccount);
    //         //AccountTypeEnum accttype = (AccountTypeEnum)Enum.Parse(typeof(AccountTypeEnum), "AccountType");
    //         // var vendor = _mapper.Map<BankAccount>(ba);
    //         var bankAccountDto = _mapper.Map<BankAccountResponse>(result);
    //         return Ok(bankAccountDto);  
    //     }

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
