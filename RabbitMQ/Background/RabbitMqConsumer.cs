using System.Text;
using Newtonsoft.Json;
using PuppeteerSharp;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Test_task.Controllers.Models;
using Test_task.Infrastructure;
using Test_task.Infrastructure.Models;

namespace Test_task.RabbitMQ.Background;

/// <summary>
/// Бекграунд консьюмер
/// </summary>
public class RabbitMqConsumer : BackgroundService
{
    public RabbitMqConsumer(IStateCache stateCache, IConfiguration configuration)
    {
        _stateCache = stateCache; 
        ConnectionFactory factory = new() { Uri = new(configuration.GetSection("RabbitMqConnection").Value) };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
    }
    
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly IStateCache _stateCache;
    private const string _queueName = "Task";
    private readonly string _storageHtmlFolderName = "htmlFiles";
    private readonly string _storagePdfFolderName = "PdfFiles";

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        EventingBasicConsumer consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (ch, ea) =>
        {
            string content = Encoding.UTF8.GetString(ea.Body.ToArray());
            await Convert(content, ea);

            _channel.BasicAck(ea.DeliveryTag, false);
        };

        _channel.BasicConsume(_queueName, false, consumer);

        return Task.CompletedTask;
    }

    private async Task Convert(string content, BasicDeliverEventArgs ea)
    {
        try
        {
            CreateMessageModel convertModel = JsonConvert.DeserializeObject<CreateMessageModel>(content);
            ConvertingState state = new ConvertingState(convertModel.Id, "In progress");

            _stateCache.AddCache(state);
            var browserFetcher = new BrowserFetcher();
            await browserFetcher.DownloadAsync(); //при первом обращении будет скачан локальный хромиум
            await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
            await using var page = await browser.NewPageAsync();

            string pathWithoutExt = Path.ChangeExtension(convertModel.FileName, null);
            string filePath =
                $"{Directory.GetCurrentDirectory()}\\{_storageHtmlFolderName}\\{pathWithoutExt} {convertModel.Id}.html";
            string fullPathToSavePdf =
                $"{Directory.GetCurrentDirectory()}\\{_storagePdfFolderName}\\{convertModel.FileName} {convertModel.Id}.pdf";

            await page.GoToAsync(filePath);
            await page.PdfAsync(fullPathToSavePdf);

            state.State = "Converted";
            _stateCache.AddCache(state);
        }
        catch (NavigationException e)
        {
            CreateMessageModel convertModel = JsonConvert.DeserializeObject<CreateMessageModel>(content);
            ConvertingState state = new ConvertingState(convertModel.Id, "Error: file not found");
            _stateCache.AddCache(state);
            _channel.BasicAck(ea.DeliveryTag, false);
            throw new NavigationException(e.Message);
        }
        catch (Exception e)
        {
            CreateMessageModel convertModel = JsonConvert.DeserializeObject<CreateMessageModel>(content);
            ConvertingState state = new ConvertingState(convertModel.Id, "Error");
            _stateCache.AddCache(state);
            throw new InvalidOperationException(e.Message);
        }
    }

    public override void Dispose()
    {
        _channel.Close();
        _connection.Close();
        base.Dispose();
    }
}