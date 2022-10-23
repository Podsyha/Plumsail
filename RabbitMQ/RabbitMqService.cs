using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace Test_task.RabbitMQ;

public class RabbitMqService : IRabbitMqService
{
    public void SendMessage(object obj)
    {
        var message = JsonSerializer.Serialize(obj);
        SendMessage(message);
    }
    
    private readonly ConnectionFactory _factory = new() { Uri = new("amqps://lgksldrw:j1I-o7Tn0rb63R5RPuxYi3Bcs4BZl3cu@kangaroo.rmq.cloudamqp.com/lgksldrw") };

    public void SendMessage(string message)
    {
        //TODO: вынести инициализацибю коннекта в отдельную структуру, что бы не пересоздавалось каждый раз
        using var connection = _factory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.QueueDeclare(queue: "MyQueue",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(exchange: "",
            routingKey: "MyQueue",
            basicProperties: null,
            body: body);
    }
}