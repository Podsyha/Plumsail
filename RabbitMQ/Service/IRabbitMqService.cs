namespace Test_task.RabbitMQ.Service;

public interface IRabbitMqService
{
    /// <summary>
    /// Отправить объект в брокер
    /// </summary>
    /// <param name="obj"></param>
    void SendMessage(object obj);
}