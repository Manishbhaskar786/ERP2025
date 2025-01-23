using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ERPack.MultiTenancy
{
    public class TenantBackgroundService : BackgroundService
    {
       // private readonly HttpClient _httpClient = new HttpClient();
        private readonly IServiceProvider _serviceProvider;

        public TenantBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.Now;
                var nextRunTime = GetNextRunTime(now);
                var delay = nextRunTime - now;
                if (delay.TotalMilliseconds < 0)
                {
                    nextRunTime = nextRunTime.AddDays(1);
                    delay = nextRunTime - now;
                }
                await Task.Delay(delay, stoppingToken);
                await FunctionCall(stoppingToken);
            }
        }
        private DateTime GetNextRunTime(DateTime now)
        {
            //var nextRunTime = new DateTime(now.Year, now.Month, now.Day, 14, 40, 00);
            var nextRunTime = now.AddDays(15);
            return nextRunTime;
        }
        private async Task FunctionCall(object state)
        {
            var tenantAppService =  _serviceProvider.GetRequiredService<ITenantBgService>();
            var tenantsToDelete = await tenantAppService.GetAllAsync();
            var tenantsAfterThreshold = tenantsToDelete.
                Where(t => t.DeletionTime.HasValue && 
                t.DeletionTime.Value < DateTime.Now.Subtract(TimeSpan.FromDays(90)));

            foreach (var tenant in tenantsAfterThreshold)
            {
                //TODO
                await tenantAppService.DeleteTenantAsync(tenant.Id);
            }
        }

    }
}
