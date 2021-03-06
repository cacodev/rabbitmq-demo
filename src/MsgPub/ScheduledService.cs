
using System;
using System.Dynamic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace MsgPub
{
    public class ScheduledService : IHostedService
    {
        private Timer _timer;
        private readonly IMsgPubSvc _msgPubSvc;
        private readonly ILogger<ScheduledService> _logger;

        public ScheduledService (IMsgPubSvc msgPubSvc, ILogger<ScheduledService> logger)
        {
            _msgPubSvc = msgPubSvc;
            _logger = logger;
        } 
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("Starting Scheduled Service");
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            _logger.LogDebug("Twerk Wrk");
            dynamic msg = new ExpandoObject();
            msg.msg = "Hello!";
            msg.created = DateTime.Now;
            _msgPubSvc.PubMsg(JsonConvert.SerializeObject(msg));
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("Stopping Scheduled Service");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _logger.LogDebug("Disposing Scheduled Service");
            _timer?.Dispose();
        }
    }
}