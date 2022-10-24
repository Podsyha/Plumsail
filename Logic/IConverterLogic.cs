using Test_task.Infrastructure.Models;

namespace Test_task.Logic;

public interface IConverterLogic
{
    /// <summary>
    /// Сохранить файл в локальное хранилище
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    Task SaveFileToLocalStorage(IFormFile file, Guid id);
    /// <summary>
    /// Получить статус конвертации
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    ConvertingState GetConvertingState(Guid id);
    /// <summary>
    /// Проверить наличие файла по пути
    /// </summary>
    /// <param name="id"></param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    string CheckExistFile(Guid id, string fileName);

}