using Account.API.Models;

namespace Account.API.Services.Interface
{
    public interface IAccountRepository
    {
        Task<List<BankAccount>> GetAllBankAccounts();
        Task<BankAccount> GetBankAccount(int id);
        Task<BankAccount> PostBankAccount(BankAccount bankAccount);
        Task <BankAccount> PutBankAccount(int id, BankAccount bankAccount);
        Task DeleteBankAccount(BankAccount bankAccount);


    }
}