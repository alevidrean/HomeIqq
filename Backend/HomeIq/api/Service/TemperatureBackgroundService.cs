using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using api.Service;

namespace api.Service
{
    public class TemperatureBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public TemperatureBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var programService = scope.ServiceProvider.GetRequiredService<ITemperatureProgramService>();
                    var tempCurenta = await programService.GetCurrentTemperatureAsync();
                    if (tempCurenta != null)
                    {
                        string message = $"WantedTemperature:{tempCurenta.Value}";
                        await WebSocketHandler.BroadcastMessageAsync(message);
                    }
                }
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); // rulează la fiecare minut
            }
        }
    }

}