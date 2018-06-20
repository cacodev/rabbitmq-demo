using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;

namespace MsgPub
{
    public class MsgPubSvc
    {
        public static void PubMsg(string msg)
        {    
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
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