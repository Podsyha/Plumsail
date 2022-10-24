namespace Test_task.Controllers.Models;

public sealed class CreateMessageModel
{
    public CreateMessageModel(string fileName)
    {
        FileName = fileName;
        Id = Guid.NewGuid();
        DateAdded = DateTime.UtcNow;
    }
    /// <summary>
    /// ID задачи конвертации
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Имя файла
    /// </summary>
    public string FileName { get; set; }
    /// <summary>
    /// Дата добавления задачи
    /// </summary>
    public DateTime DateAdded { get; set; }
}