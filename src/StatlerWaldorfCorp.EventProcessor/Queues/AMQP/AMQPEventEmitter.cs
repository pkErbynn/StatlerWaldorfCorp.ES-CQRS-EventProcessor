using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using StatlerWaldorfCorp.EventProcessor.Events;
using StatlerWaldorfCorp.EventProcessor.Models;

namespace StatlerWaldorfCorp.EventProcessor.Queues.AMQP
{
    public class AMQPEventEmitter : IEventEmitter
    {
        private ILogger<AMQPEventEmitter> logger;
        private QueueOptions queueOptions;
        private AMQPOptions amqpOptions;
        private AMQPConnectionFactory connectionFactory;


        public AMQPEventEmitter(
            ILogger<AMQPEventEmitter> logger,
            IOptions<QueueOptions> qOptions,
            IOptions<AMQPOptions> aOptions,
            AMQPConnectionFactory connectionFactory)
        {
            this.logger = logger;
            this.queueOptions = qOptions.Value;
            this.amqpOptions = aOptions.Value;
            this.connectionFactory = connectionFactory;

            // connectionFactory = new ConnectionFactory();
            // connectionFactory.UserName = amqpOptions.Username;
            // connectionFactory.Password = amqpOptions.Password;
            // connectionFactory.VirtualHost = amqpOptions.VirtualHost;
            // ((ConnectionFactory)connectionFactory).HostName = amqpOptions.HostName;
            // connectionFactory.Uri = new Uri(amqpOptions.Uri);

            logger.LogInformation($"Emitting events on queue {this.queueOptions.ProximityDetectedEventQueueName}");
        }

        public void EmitProximityDetectedEvent(ProximityDetectedEvent proximityDetectedEvent)
        {
            using (IConnection conn = this.connectionFactory.GetConnection())
            {
                using (IModel channel = conn.CreateModel())
                {
                    channel.QueueDeclare(
                        queue: queueOptions.ProximityDetectedEventQueueName,
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                    );

                    string jsonPayload = proximityDetectedEvent.toJson();
                    var body = Encoding.UTF8.GetBytes(jsonPayload);

                    channel.BasicPublish(
                        exchange: "",
                        routingKey: queueOptions.ProximityDetectedEventQueueName,
                        basicProperties: null,
                        body: body
                    );

                    logger.LogInformation($"Emitted proximity event of {jsonPayload.Length} bytes to queue.");
                }
            }        
        }
    }
}