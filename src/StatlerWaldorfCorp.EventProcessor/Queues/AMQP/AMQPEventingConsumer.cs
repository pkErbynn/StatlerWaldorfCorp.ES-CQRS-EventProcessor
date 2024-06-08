using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace StatlerWaldorfCorp.EventProcessor.Queues.AMQP
{
    // Wrapper to get to know Rabbit predefined events
    public class AMQPEventingConsumer: EventingBasicConsumer
    {
        public AMQPEventingConsumer(ILogger<AMQPEventingConsumer> logger, IConnectionFactory factory): base(factory.CreateConnection().CreateModel())
        {
        }
    }
}