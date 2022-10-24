using Test_task.Infrastructure;
using Test_task.Infrastructure.Models;

namespace Test_task.Logic;

public class ConverterLogic : IConverterLogic
{
    public ConverterLogic(IStateCache stateCache)
    {
        _stateCache = stateCache;
    }

    private readonly IStateCache _stateCache;
    private readonly string _storagePdfFolderName = "PdfFiles";
    private readonly string _storageHtmlFolderName = "htmlFiles";

    public async Task SaveFileToLocalStorage(IFormFile file, Guid id)
    {
        string pathWithoutExt = Path.ChangeExtension(file.FileName, null);
        string fileName = pathWithoutExt.Length > 100 ? pathWithoutExt[..100] : pathWithoutExt;
        string filePath = $"{Directory.GetCurrentDirectory()}\\{_storageHtmlFolderName}\\{fileName} {id}.html";
        await using Stream fileStream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(fileStream);
    }

    public ConvertingState GetConvertingState(Guid id) =>
        _stateCache.GetCache(id);

    public string CheckExistFile(Guid id, string fileName)
    {
        string fullPathToSavePdf = $"{Directory.GetCurrentDirectory()}\\{_storagePdfFolderName}\\{fileName} {id}.pdf";
        if (!File.Exists(fullPathToSavePdf)) 
            throw new FileNotFoundException();
        
        return fullPathToSavePdf;
    }
}