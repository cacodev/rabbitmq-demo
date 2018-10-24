using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace MsgPub
{
    public class MsgPubSvc: IMsgPubSvc
    {
        private const string _exchangeName = "hello";
        private readonly IConnectionFactory _factory;
        private readonly ILogger<MsgPubSvc> _logger;

        public MsgPubSvc (IConnectionFactory factory, ILogger<MsgPubSvc> logger)
        {
            _factory = factory;
            _logger = logger;
        } 
        public void PubMsg(string msg)
        {    
            using (var connection = _factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    _logger.LogInformation($" [x] sending {msg} to {_exchangeName}");
                    var body = Encoding.ASCII.GetBytes(msg);
                    channel.BasicPublish(exchange: _exchangeName,
                                        routingKey: "",
                                        basicProperties: null,
                                        body: body);
                }
            }
        }
    }
}