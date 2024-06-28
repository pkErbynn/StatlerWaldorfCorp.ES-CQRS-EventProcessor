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

        private readonly ConnectionFactory connectionFactory;
        private IConnection connection;
        private readonly object _lock = new();

        protected AMQPOptions amqpOptions;

        public AMQPConnectionFactory(
            ILogger<AMQPConnectionFactory> logger,
            IOptions<AMQPOptions> options
        )
        {
            this.connectionFactory = new ConnectionFactory();

            this.amqpOptions = options.Value;

            connectionFactory.UserName = amqpOptions.Username;
            connectionFactory.Password = amqpOptions.Password;
            connectionFactory.VirtualHost = amqpOptions.VirtualHost;
            connectionFactory.HostName = amqpOptions.HostName;
            connectionFactory.Uri = new Uri(amqpOptions.Uri);

            logger.LogInformation($"AMQP Connection configured for URI : {amqpOptions.Uri}");        
        }

        public IConnection GetConnection()
        {
            if (this.connection == null || !this.connection.IsOpen)
            {
                lock (_lock)
                {
                    if (this.connection == null || !this.connection.IsOpen)
                    {
                        this.connection = this.connectionFactory.CreateConnection();
                    }
                }
            }
            return this.connection;
        }
    }
}