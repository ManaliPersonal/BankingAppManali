using Account.API.Database;
using Account.API.Models;
using Account.API.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace Account.API.Services.Implementation;

public class AccountRepository : IAccountRepository
{
    private readonly AccountContext _accountContext;

    public AccountRepository(AccountContext accountContext)
    {
        _accountContext = accountContext;
    }
    public async Task DeleteBankAccount(BankAccount bankAccount)
    {
        if (bankAccount != null)
            _accountContext.Accounts.Remove(bankAccount);
        _accountContext.SaveChanges();
    }

    public async Task<BankAccount> GetBankAccount(int id)
    {
        var bankAccount = await _accountContext.Accounts.FindAsync(id);
        return bankAccount;
    }

    public async Task<List<BankAccount>> GetAllBankAccounts()
    {
        return await _accountContext.Accounts.ToListAsync();
    }

    public async Task<BankAccount> PostBankAccount(BankAccount bankAccount)
    {
        var result = _accountContext.Accounts.Add(bankAccount);
        await _accountContext.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<BankAccount> PutBankAccount(int id, BankAccount bankAccount)
    {

        var existingAccount = await _accountContext.Accounts.FindAsync(id);

        if (existingAccount == null)
        {
            return null;
        }

        existingAccount.AccountHolderName = bankAccount.AccountHolderName;
        existingAccount.AccountType = bankAccount.AccountType;
        existingAccount.AccountBalance = bankAccount.AccountBalance;

        _accountContext.Accounts.Update(existingAccount);
        await _accountContext.SaveChangesAsync();

        return existingAccount;
    }
}

