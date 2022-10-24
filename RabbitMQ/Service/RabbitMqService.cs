using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using Test_task.RabbitMQ.Connection;

namespace Test_task.RabbitMQ.Service;

public class RabbitMqService : IRabbitMqService
{
    public RabbitMqService(IRabbitMqConnection rabbitMqConnection)
    {
        _rabbitMqConnection = rabbitMqConnection;
    }

    private readonly IRabbitMqConnection _rabbitMqConnection;
    private const string _queueName = "Task";
    
    public void SendMessage(object obj)
    {
        string message = JsonSerializer.Serialize(obj);
        SendMessage(message);
    }
    
    private void SendMessage(string message)
    {
        IConnection connection = _rabbitMqConnection.GetConnection();
        using IModel channel = connection.CreateModel();
        channel.QueueDeclare(queue: _queueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        byte[] body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(exchange: "",
            routingKey: _queueName,
            basicProperties: null,
            body: body);
    }
}