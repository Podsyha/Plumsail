namespace Test_task.RabbitMQ.Service;

public interface IRabbitMqService
{
    void SendMessage(object obj);
}