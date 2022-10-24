using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp;
using Test_task.Controllers.Models;
using Test_task.RabbitMQ;
using Test_task.RabbitMQ.Service;

namespace Test_task.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class ConverterController : ControllerBase
{
    private readonly IRabbitMqService _mqService;

    public ConverterController(IRabbitMqService mqService)
    {
        _mqService = mqService;
    }

    [HttpPost("/sendMessage")]
    public async Task<IActionResult> SendMessage(IFormFile file)
    {
        await SaveFileToLocalStorage(file);
        CreateMessageModel message = new(file.FileName);
        
        _mqService.SendMessage(message);

        return Ok();
    }
    
    [HttpPost("/convert")]
    public async Task<IActionResult> Convert(IFormFile file)
    {
        string filePath = Path.Combine(@"C:\Users\User\RiderProjects\Test task\htmlFiles", file.FileName);
        await using Stream fileStream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(fileStream);
        await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
        await using var page = await browser.NewPageAsync();
        await page.GoToAsync(filePath);
        await page.PdfAsync($"{Directory.GetCurrentDirectory()}\\test.pdf");

        return Ok();
    }

    private async Task SaveFileToLocalStorage(IFormFile file)
    {
        string filePath = Path.Combine(@"C:\Users\User\RiderProjects\Test task\htmlFiles", file.FileName);
        await using Stream fileStream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(fileStream);
    }
}