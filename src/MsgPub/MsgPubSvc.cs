using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;

namespace MsgPub
{
    public class MsgPubSvc: IMsgPubSvc
    {
        private readonly IConnectionFactory _factory;

        public MsgPubSvc (IConnectionFactory factory)
        {
            _factory = factory;
        } 
        public void PubMsg(string msg)
        {    
            using (var connection = _factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var body = Encoding.UTF8.GetBytes(msg);
                    channel.BasicPublish(exchange: "hello",
                                        routingKey: "",
                                        basicProperties: null,
                                        body: body);
                }
            }
        }
    }
}