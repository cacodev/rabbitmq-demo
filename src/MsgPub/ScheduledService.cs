
using System;
using System.Dynamic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
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
            dynamic msg = new ExpandoObject();
            msg.msg = "Hello!";
            msg.created = DateTime.Now;
            _msgPubSvc.PubMsg(JsonConvert.SerializeObject(msg));
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