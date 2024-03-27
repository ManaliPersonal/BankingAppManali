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
        _accountRepository = accountRepository;
        _mapper = mapper;
    }

    [HttpGet("GetAllAccounts")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<IEnumerable<BankAccountResponse>>> GetAllAccounts()
    {
        var result = await _accountRepository.GetAllBankAccounts();
        var bankAccounts = _mapper.Map<IEnumerable<BankAccountResponse>>(result);
        return Ok(bankAccounts);
    }

    [HttpGet("GetBankAccount/{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]

    public async Task<ActionResult<BankAccountResponse>> GetBankAccount(int id)
    {
        var result = await _accountRepository.GetBankAccount(id);
        var bankAccountDto = _mapper.Map<BankAccountResponse>(result);
        return Ok(bankAccountDto);
    }

    [HttpPost("AddBankAccount")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> AddBankAccount([FromBody] BankAccountRequest bankAccountRequestDto)
    {
        var bankAccount = _mapper.Map<BankAccount>(bankAccountRequestDto);
        var result = await _accountRepository.PostBankAccount(bankAccount);
        var addedBankAccountDto = _mapper.Map<BankAccountResponse>(result);
        return Ok(addedBankAccountDto);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> PutBankAccount(int id, BankAccountRequest bankAccountRequestDto)
    {

        var bankAccount = _mapper.Map<BankAccount>(bankAccountRequestDto);
        var result = await _accountRepository.PutBankAccount(id, bankAccount);
        return NoContent();
    }


    [HttpDelete("{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> DeleteBankAccount(int id)
    {
        var bankAccount = await _accountRepository.GetBankAccount(id);
        if (bankAccount == null)
        {
            return NotFound();
        }

        var result = _accountRepository.DeleteBankAccount(bankAccount);
        return NoContent();
    }

}
