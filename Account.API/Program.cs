using Account.API;
using Account.API.Database;
using Account.API.Profiles;
using Account.API.Services.Implementation;
using Account.API.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Plain.RabbitMQ;
using RabbitMQ.Client;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<BankAccountProfile>(), typeof(Program));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Host.UseSerilog();
// Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().
//     WriteTo.File("log/Accounts.txt", rollingInterval: RollingInterval.Day).CreateLogger();

    Log.Logger = new LoggerConfiguration().MinimumLevel.Information().
    WriteTo.File("logging/Accounts.txt", rollingInterval: RollingInterval.Day).CreateLogger();

//
//Configure database service
var connectionstring= builder.Configuration.GetConnectionString("mysql");

builder.Services.AddDbContext<AccountContext>(options=>
    options.UseMySql(connectionstring,ServerVersion.AutoDetect(connectionstring))
);


//configure rabbitmq
builder.Services.AddSingleton<IConnectionProvider>(new ConnectionProvider("amqp://guest:guest@localhost:5672"));

// configure rabbitmq exchange where Account service publish the message 
builder.Services.AddSingleton<IPublisher>(p => new Publisher(p.GetService<IConnectionProvider>(),
    "account_exchange", //exchange name  //Exchanges are elements in the messaging system that route messages to queues
    ExchangeType.Topic //exchange type
));

//configure the topic where Account service will be subscribed

builder.Services.AddSingleton<ISubscriber>(s=>new Subscriber(
s.GetService<IConnectionProvider>(),  //connect to RabbitMq
"transaction_exchange",  //exchange name  //This is the name of the exchange being used. Messages will be published to this exchange.
"transaction_response_queue", // queue name //This is the name of the queue to which messages will be delivered.
"transaction_created_routingkey", //Routing key //This is the routing key used to route messages from the exchange to the queue
ExchangeType.Topic               //exchange type
));

builder.Services.AddScoped<IAccountRepository, AccountRepository>();
//Register the Listner
builder.Services.AddHostedService<TransactionCreatedListener>();

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
