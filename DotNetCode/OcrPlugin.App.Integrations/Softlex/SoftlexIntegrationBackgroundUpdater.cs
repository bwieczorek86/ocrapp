using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OcrPlugin.App.Integrations.Softlex
{
    public sealed class SoftlexIntegrationBackgroundUpdater : IHostedService, IDisposable
    {
        private readonly ILogger<SoftlexIntegrationBackgroundUpdater> _logger;
        private readonly IServiceProvider _serviceProvider;
        private Timer _timer;

        public SoftlexIntegrationBackgroundUpdater(
            ILogger<SoftlexIntegrationBackgroundUpdater> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");

            var timeToStart = GetTimeToStart();
            const int hoursInterval = 24;
            _timer = new Timer(IncrementSoftlexData, null, timeToStart, TimeSpan.FromHours(hoursInterval));

            _logger.LogInformation($"Timer set to start at {timeToStart} and continue every {hoursInterval} hours.");

            return Task.CompletedTask;
        }

        private static TimeSpan GetTimeToStart()
        {
            var utcNow = DateTime.UtcNow;
            var timeToStart = DateTime.UtcNow.Date.AddDays(1).AddHours(4);

            return TimeSpan.FromMinutes((timeToStart - utcNow).TotalMinutes);
        }

        private async void IncrementSoftlexData(object state)
        {
            using var scope = _serviceProvider.CreateScope();
            var integrationClient = scope.ServiceProvider.GetRequiredService<ISoftlexCloudIntegration>();

            await integrationClient.IncrementData();
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}