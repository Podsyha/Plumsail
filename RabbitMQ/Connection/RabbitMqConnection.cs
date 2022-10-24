using RabbitMQ.Client;

namespace Test_task.RabbitMQ.Connection;

public class RabbitMqConnection : IRabbitMqConnection
{
    public RabbitMqConnection()
    {
        _currentConnection = _factory.CreateConnection();
    }
    
    private readonly ConnectionFactory _factory = new()
        { Uri = new("amqps://lgksldrw:j1I-o7Tn0rb63R5RPuxYi3Bcs4BZl3cu@kangaroo.rmq.cloudamqp.com/lgksldrw") };
    private IConnection _currentConnection;

    public IConnection GetConnection()
    {
        if (_currentConnection.IsOpen)
            return _currentConnection;

        _currentConnection = _factory.CreateConnection();
        return _currentConnection;
    }
}