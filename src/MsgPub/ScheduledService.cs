
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;

namespace MsgPub
{
    public class ScheduledService : IHostedService
    {
        private Timer _timer;
        private readonly IMsgPubSvc _msgPubSvc;

        public ScheduledService (IMsgPubSvc msgPubSvc)
        {
            _msgPubSvc = msgPubSvc;
        } 
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            _msgPubSvc.PubMsg("Hello! " + DateTime.Now.ToString());
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}