using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using StatlerWaldorfCorp.EventProcessor.Events;
using StatlerWaldorfCorp.EventProcessor.Models;

namespace StatlerWaldorfCorp.EventProcessor.Queues.AMQP
{
    public class AMQPEventSubscriber : IEventSubscriber
    {
        public event MemberLocationRecordedEventReceivedDelegate MemberLocationRecordedEventReceived;

        private ILogger logger;
        private EventingBasicConsumer consumer;
        private QueueOptions queueOptions;
        private string consumerTag;
        private IModel channel;

        public AMQPEventSubscriber(
            ILogger<AMQPEventSubscriber> logger,
            IOptions<QueueOptions> qOptions,
            EventingBasicConsumer consumer)
        {
            this.logger = logger;
            this.queueOptions = qOptions.Value;
            this.consumer = consumer;
            this.channel = consumer.Model;

            this.Initialize();
        }

        private void Initialize()
        {
            channel.QueueDeclare(
                queue: queueOptions.MemberLocationRecordedEventQueueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            logger.LogInformation($"Initialized event subscriber for queue {queueOptions.MemberLocationRecordedEventQueueName}");

            consumer.Received += (channel, eventArg) => 
            {
                var body = eventArg.Body;
                var message = Encoding.UTF8.GetString(body.ToArray());
                var memberLocationEvent = JsonConvert.DeserializeObject<MemberLocationRecordedEvent>(message);
                logger.LogInformation($"Received incoming event, {body.Length} bytes.");
                
                if(MemberLocationRecordedEventReceived != null)
                {
                    MemberLocationRecordedEventReceived(memberLocationEvent);
                }

                this.channel.BasicAck(eventArg.DeliveryTag, false);
            };
        }

        public void Subscribe()
        {
            this.consumerTag = channel.BasicConsume(queueOptions.MemberLocationRecordedEventQueueName, false, consumer);
            this.logger.LogInformation("Subscribed to queue.");
        }

        public void Unsubscribe()
        {
            this.channel.BasicCancel(consumerTag);
            this.logger.LogInformation("UnSubscribed from queue.");
        }
    }
}