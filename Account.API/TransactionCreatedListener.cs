

using Account.API.Database;
using Account.API.Models;
using Newtonsoft.Json;
using Plain.RabbitMQ;
using Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Transactions;
using Shared.Enums;
using Serilog;

namespace Account.API
{
    public class TransactionCreatedListener : IHostedService
    {
        private ISubscriber _subscriber;
        private IPublisher _publisher;

        private readonly IServiceScopeFactory _scopeFactory;

        public TransactionCreatedListener(IServiceScopeFactory scopeFactory, ISubscriber subscriber, IPublisher publisher)
        {
            _publisher = publisher;
            _subscriber = subscriber;
            _scopeFactory = scopeFactory;
        }

        private bool Subscribe(string message, IDictionary<string, object> header)
        {
            var response = JsonConvert.DeserializeObject<TransactionRequest>(message);
            using (var scope = _scopeFactory.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<AccountContext>();
                try
                {
                    BankAccount BankAccount = _context.Accounts.Find(response.AccountId);
                    // if available balance is less then requested amount
                    if (BankAccount == null || BankAccount.AccountBalance < response.Amount)
                        throw new Exception();
        

                    Log.Information("outside of the if block");
                   

                    if (response.TransactionType==TransactionType.Credit)
                    {
                        Log.Information("Entered in If Block of Credit");
                        BankAccount.AccountBalance=BankAccount.AccountBalance+response.Amount;
                        _context.Entry(BankAccount).State = EntityState.Modified;
                        Log.Information("Before savechanges method of the if block");
                        _context.SaveChanges();

                    }
                    else
                    {
                         //  reduce the amount if debit
                    Log.Information("Entered in else Block of transactiontype debit ");
                    BankAccount.AccountBalance = BankAccount.AccountBalance - response.Amount;
                    _context.Entry(BankAccount).State = EntityState.Modified;
                    _context.SaveChanges();

                    }


                    // publish message to inform Transaction service for success
                    _publisher.Publish(JsonConvert.SerializeObject(
                        new AccountResponse
                        {
                            TransactionId = response.TransactionId,
                            AccountId = response.AccountId,
                            Balance = response.Balance,
                            IsSuccess = true,
                            TransactionType= (int)response.TransactionType
                        }
                    ), "account_response_routingkey", null);

                    Log.Information("End of publish message to inform Transaction service for success");
                }
                catch (Exception ex)
                {
                    Log.Information("entered in Exception block");
                    // publish message to inform Transaction service for failed
                    _publisher.Publish(JsonConvert.SerializeObject(
                        new AccountResponse
                        {
                            TransactionId = response.TransactionId,
                            AccountId = response.AccountId,
                            Balance = response.Balance,
                            IsSuccess = false,
                            TransactionType= (int)response.TransactionType
                        }
                    ), "account_response_routingkey", null);

                    Log.Information("Capture Exception message"+ex.Message);
                }
                return true;
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _subscriber.Subscribe(Subscribe);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}