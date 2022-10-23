using Microsoft.AspNetCore.Mvc;

namespace Test_task.Controllers;

[ApiController]
[Route("[controller]")]
public class ConverterController : ControllerBase
{
    private readonly ILogger<ConverterController> _logger;

    public ConverterController(ILogger<ConverterController> logger)
    {
        _logger = logger;
    }
    
}