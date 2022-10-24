using Test_task.Infrastructure;
using Test_task.Logic;
using Test_task.RabbitMQ.Background;
using Test_task.RabbitMQ.Connection;
using Test_task.RabbitMQ.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
builder.Services.AddHostedService<RabbitMqConsumer>();

builder.Services.AddSingleton<IRabbitMqService, RabbitMqService>();
builder.Services.AddSingleton<IRabbitMqConnection, RabbitMqConnection>();
builder.Services.AddSingleton<IStateCache, StateCache>();
builder.Services.AddTransient<IConverterLogic, ConverterLogic>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(policyBuilder => policyBuilder.AllowAnyOrigin());
app.UseAuthorization();

app.MapControllers();

app.Run();