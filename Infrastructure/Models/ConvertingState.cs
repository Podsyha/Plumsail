namespace Test_task.Infrastructure.Models;

public sealed class ConvertingState
{
    public ConvertingState(Guid id, string state)
    {
        Id = id;
        State = state;
    }
    
    /// <summary>
    /// Id задачи конвертирования(ключи хранения в кеше)
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Статус задачи конвертирования
    /// </summary>
    public string State { get; set; }
}