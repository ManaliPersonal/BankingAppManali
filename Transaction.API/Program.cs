using Account.API.Profiles;
using Microsoft.EntityFrameworkCore;
using Plain.RabbitMQ;
using RabbitMQ.Client;
using Serilog;
using Transaction.API;
using Transaction.API.Database;
using Transaction.API.Services.Implementation;
using Transaction.API.Services.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddAutoMapper(cfg => cfg.AddProfile<AccountTransactionProfile>(), typeof(Program));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

    Log.Logger = new LoggerConfiguration().MinimumLevel.Information().
    WriteTo.File("logging/Transactions.txt", rollingInterval: RollingInterval.Day).CreateLogger();

// configure database
var connectionString = builder.Configuration.GetConnectionString("mysql");
builder.Services.AddDbContext<TransactionContext>(options => 
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddScoped<IAccountTransaction, AccountTransactionRepository>();

// configure rabbitmq
builder.Services.AddSingleton<IConnectionProvider>(new ConnectionProvider("amqp://guest:guest@localhost:5672"));

// configure rabbitmq exchange where order service publsih message
builder.Services.AddSingleton<IPublisher>(p => new Publisher(p.GetService<IConnectionProvider>(),
"transaction_exchange",   // exchange name
ExchangeType.Topic));   // exchange type

// configure topic where order service will be subscribe
builder.Services.AddSingleton<ISubscriber>(s => new Subscriber(
    s.GetService<IConnectionProvider>(),
    "account_exchange",         // exchange name
    "account_response_queue",   // queue name
    "account_response_routingkey", // routing key
    ExchangeType.Topic
));

// register the listener
builder.Services.AddHostedService<AccountResponseListener>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
