namespace Test_task.Controllers.Models;

public sealed class CreateMessageModel
{
    public CreateMessageModel(string fileName)
    {
        FileName = fileName;
        Id = Guid.NewGuid();
        DateAdded = DateTime.UtcNow;
    }
    
    public Guid Id { get; init; }
    public string FileName { get; init; }
    public DateTime DateAdded { get; init; }
}