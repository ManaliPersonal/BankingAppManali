

using Transaction.API.Models;

namespace Transaction.API.Services.Interface
{
    public interface IAccountTransaction
    {

         Task<List<AccountTransaction>> GetAllTransactions();
          Task<AccountTransaction> GetTransaction(int id);
           Task<AccountTransaction> PostTransaction(AccountTransaction accountTransaction);
           Task <AccountTransaction> PutTransaction(int id, AccountTransaction accountTransaction);

           Task DeleteTransaction(AccountTransaction accountTransaction);

    }
}