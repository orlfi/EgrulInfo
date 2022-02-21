using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Orlfi.EgrulLibrary;

namespace Tests;
public class Application
{
    private const string Inn = "7726381870";
    private readonly ILogger _logger;

    public Application(ILogger<Application> logger)
    {
        _logger = logger;
    }

    public async Task RunAsync()
    {
        try
        {
            _logger.LogInformation("Application started");

            try
            {
                using var client = new HttpClient();
                var egrulClient = new EgrulClient(client);
                var directory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                var fileName = $"Выписка ЕГЮЛ_{DateTime.Today.ToString("yyyy-MM-dd")}_{Inn}.pdf";
                var path = Path.Combine(directory, fileName);
                _logger.LogInformation("Загрузка отчета...");
                await egrulClient.DownloadFileAsync(Inn, path);
                _logger.LogInformation("Файл {0} загружен в папку {1}", fileName, path);

                _logger.LogInformation("Открытие файла {0}...", fileName);
                ProcessStartInfo info = new ProcessStartInfo()
                {
                    FileName = path,
                    UseShellExecute = true
                };

                Process.Start(info);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Необработанная ошибка {0}", ex.Message);
        }
    }
}