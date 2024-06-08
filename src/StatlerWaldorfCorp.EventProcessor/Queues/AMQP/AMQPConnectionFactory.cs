using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using StatlerWaldorfCorp.EventProcessor.Models;

namespace StatlerWaldorfCorp.EventProcessor.Queues.AMQP
{
    public class AMQPConnectionFactory
    {
        protected AMQPOptions amqpOptions;

        public AMQPConnectionFactory(
            ILogger<AMQPConnectionFactory> logger,
            IOptions<AMQPOptions> options
        ): base()
        {
            var connectionFactory = new ConnectionFactory();

            this.amqpOptions = options.Value;

            connectionFactory.UserName = amqpOptions.Username;
            connectionFactory.Password = amqpOptions.Password;
            connectionFactory.VirtualHost = amqpOptions.VirtualHost;
            connectionFactory.HostName = amqpOptions.HostName;
            connectionFactory.Uri = new Uri(amqpOptions.Uri);

            logger.LogInformation($"AMQP Connection configured for URI : {amqpOptions.Uri}");        }
    }
}