using RabbitMQ.Client;

namespace Test_task.RabbitMQ.Connection;

public interface IRabbitMqConnection
{
    public IConnection GetConnection();
}