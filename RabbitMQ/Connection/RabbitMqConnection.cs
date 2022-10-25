using RabbitMQ.Client;

namespace Test_task.RabbitMQ.Connection;

public class RabbitMqConnection : IRabbitMqConnection
{
    public RabbitMqConnection(IConfiguration configuration)
    {
        _factory = new() { Uri = new(configuration.GetSection("RabbitMqConnection").Value) };
        _currentConnection = _factory.CreateConnection();
    }

    private ConnectionFactory _factory;
    private IConnection _currentConnection;

    public IConnection GetConnection()
    {
        if (_currentConnection.IsOpen)
            return _currentConnection;

        _currentConnection = _factory.CreateConnection();
        return _currentConnection;
    }
}