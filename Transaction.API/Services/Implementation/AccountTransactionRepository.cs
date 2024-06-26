
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Plain.RabbitMQ;
using Serilog;
using Shared.Models;
using Transaction.API.Database;
using Transaction.API.Enums;
using Transaction.API.Models;
using Transaction.API.Services.Interface;


namespace Transaction.API.Services.Implementation;

public class AccountTransactionRepository : IAccountTransaction
{
    private readonly TransactionContext _transactionContext;
    private readonly IPublisher _publisher;

    public AccountTransactionRepository(TransactionContext transactionContext, IPublisher publisher)
    {
        _transactionContext = transactionContext;
        _publisher = publisher;
    }
    public async Task DeleteTransaction(AccountTransaction accountTransaction)
    {
        if (accountTransaction != null)
            _transactionContext.Transactions.Remove(accountTransaction);
        _transactionContext.SaveChanges();
    }

    public async Task<List<AccountTransaction>> GetAllTransactions()
    {
        return await _transactionContext.Transactions.ToListAsync();
    }

    public async Task<AccountTransaction> GetTransaction(int id)
    {
        var accountTransaction = await _transactionContext.Transactions.FindAsync(id);
        return accountTransaction;
    }

    public async Task<AccountTransaction> PostTransaction(AccountTransaction accountTransaction)
    {
        var result = _transactionContext.Transactions.Add(accountTransaction);
        _transactionContext.SaveChanges();
        // new inserted identity value
        int id = accountTransaction.TransactionId;
        Log.Information("test");
        long test = (long)TransactionType.Debit;


        _publisher.Publish(JsonConvert.SerializeObject(new TransactionRequest
        {

            TransactionId = accountTransaction.TransactionId,
            AccountId = accountTransaction.AccountId,
            Amount = accountTransaction.Amount,
            TransactionType = (Shared.Enums.TransactionType)(int)accountTransaction.TransactionType

        }),
          "transaction_created_routingkey",   // routing key
        null);


        return result.Entity;
    }

    public async Task<AccountTransaction> PutTransaction(int id, AccountTransaction accountTransaction)
    {
        var existingTransaction = await _transactionContext.Transactions.FindAsync(id);

        if (existingTransaction == null)
        {
            return null;
        }

        existingTransaction.AccountId = accountTransaction.AccountId;
        existingTransaction.TransactionType = accountTransaction.TransactionType;
        existingTransaction.Amount = accountTransaction.Amount;
        existingTransaction.TransactionDate = accountTransaction.TransactionDate;

        _transactionContext.Transactions.Update(existingTransaction);

        await _transactionContext.SaveChangesAsync();

        return existingTransaction;
    }
}