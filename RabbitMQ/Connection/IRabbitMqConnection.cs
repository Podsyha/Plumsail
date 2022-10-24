using RabbitMQ.Client;

namespace Test_task.RabbitMQ.Connection;

public interface IRabbitMqConnection
{
    /// <summary>
    /// Получить соединение
    /// </summary>
    /// <returns></returns>
    public IConnection GetConnection();
}