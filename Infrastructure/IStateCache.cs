using Test_task.Infrastructure.Models;

namespace Test_task.Infrastructure;

public interface IStateCache
{
    /// <summary>
    /// Добавить значение в кеш
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    public ConvertingState AddCache(ConvertingState state);

    /// <summary>
    /// Получить значение из кеша
    /// </summary>
    /// <param name="stateId"></param>
    /// <returns></returns>
    public ConvertingState GetCache(Guid stateId);
}