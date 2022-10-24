using System.Diagnostics;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Test_task.RabbitMQ.Background;

public class RabbitMqConsumer : BackgroundService
{
    public RabbitMqConsumer()
    {
        ConnectionFactory factory = new ConnectionFactory { Uri = new("amqps://lgksldrw:j1I-o7Tn0rb63R5RPuxYi3Bcs4BZl3cu@kangaroo.rmq.cloudamqp.com/lgksldrw") };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
    }
    
    private IConnection _connection;
    private IModel _channel;
    private const string _queueName = "Task";

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        EventingBasicConsumer consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (ch, ea) =>
        {
            var content = Encoding.UTF8.GetString(ea.Body.ToArray());
			
            // Каким-то образом обрабатываем полученное сообщение
            Debug.WriteLine($"Получено сообщение: {content}");

            _channel.BasicAck(ea.DeliveryTag, false);
        };

        _channel.BasicConsume(_queueName, false, consumer);

        return Task.CompletedTask;
    }
	
    public override void Dispose()
    {
        _channel.Close();
        _connection.Close();
        base.Dispose();
    }
}