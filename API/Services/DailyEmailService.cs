using API.Interface;

namespace API.Services;
public class DailyEmailService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DailyEmailService> _logger;

    public DailyEmailService(IServiceProvider serviceProvider, ILogger<DailyEmailService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var nextRun = DateTime.Today.AddDays(1); // Chạy lúc nửa đêm
            var delay = nextRun - DateTime.Now;
            await Task.Delay(delay, stoppingToken);

            using (var scope = _serviceProvider.CreateScope())
            {
                var emailRepository = scope.ServiceProvider.GetRequiredService<IEmailRegisterRepository>();
                var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                var weatherService = scope.ServiceProvider.GetRequiredService<WeatherService>();

                try
                {
                    // Lấy danh sách email hợp lệ từ database
                    var emailRegisters = await emailRepository.GetEmailRegisterIsValid();

                    foreach (var emailRegister in emailRegisters)
                    {
                        // Lấy thông tin thời tiết cho location tương ứng
                        var weather = await weatherService.GetWeatherAsync(emailRegister.Location);

                        // Gửi email
                        await emailService.SendWeatherEmail(emailRegister.Email, weather);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to send daily weather email.");
                }
            }
        }
    }
}
