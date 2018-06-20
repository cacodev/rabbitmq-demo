using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MsgSub1
{
    public class SubSvc:IHostedService, IDisposable
    {
        private ILogger<SubSvc> _logger;
        private IConnection _connection;
        private IModel _channel;
        private EventingBasicConsumer _consumer;
        public SubSvc(ILogger<SubSvc> logger) => _logger = logger;

        public void Dispose()
        {
            if (_channel != null)
            {
                _channel.Close();
                _channel = null;
            }

            if(_connection != null)
            {
                _connection.Close();
                _connection = null;
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Subscribe();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("Stopping");
            _channel.Close();
            _connection.Close();
            return Task.CompletedTask;
        }

        public void Subscribe()
        {
            _logger.LogDebug("Subscribing");

            var _factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();

            //.ExchangeDeclare(exchange: "hello", type: "fanout");

            var queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue: queueName,
                              exchange: "hello",
                              routingKey: "");


                _consumer = new EventingBasicConsumer(_channel);
                _consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    _logger.LogDebug(" [x] Received {0} on {1}", message, queueName);
                };
                _channel.BasicConsume(queue: queueName,
                                    autoAck: true,
                                    consumer: _consumer);
        }
    }

}