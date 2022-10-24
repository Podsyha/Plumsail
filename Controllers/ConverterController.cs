using Microsoft.AspNetCore.Mvc;
using Test_task.Controllers.Models;
using Test_task.Logic;
using Test_task.RabbitMQ.Service;

namespace Test_task.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class ConverterController : ControllerBase
{
    public ConverterController(IRabbitMqService mqService, IConverterLogic converterLogic)
    {
        _mqService = mqService;
        _converterLogic = converterLogic;
    }
    
    private readonly IRabbitMqService _mqService;
    private readonly IConverterLogic _converterLogic;


    /// <summary>
    /// Конвертировать файл
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    [HttpPost("/convert")]
    public async Task<IActionResult> SendFile(IFormFile file)
    {
        CreateMessageModel message = new(file.FileName);
        await _converterLogic.SaveFileToLocalStorage(file, message.Id);

        _mqService.SendMessage(message);

        return Ok(message);
    }

    /// <summary>
    /// Получить статус конвертирования
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("/converting-status")]
    public IActionResult GetConvertingStatus(Guid id) => 
        Ok(_converterLogic.GetConvertingState(id));

    /// <summary>
    /// Получить сконвертированный файл
    /// </summary>
    /// <param name="id"></param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    [HttpGet("/file")]
    public IActionResult GetPdfFile(Guid id, string fileName)
    {
        byte[] mas = System.IO.File.ReadAllBytes(_converterLogic.CheckExistFile(id, fileName));
        string file_type = "application/pdf";
        string file_name = $"{fileName}.pdf";
        return File(mas, file_type, file_name);
    }
}