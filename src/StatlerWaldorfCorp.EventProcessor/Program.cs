using Microsoft.AspNetCore.Connections;
using RabbitMQ.Client.Events;
using StatlerWaldorfCorp.EventProcessor.Events;
using StatlerWaldorfCorp.EventProcessor.Location;
using StatlerWaldorfCorp.EventProcessor.Location.Redis;
using StatlerWaldorfCorp.EventProcessor.Models;
using StatlerWaldorfCorp.EventProcessor.Queues;
using StatlerWaldorfCorp.EventProcessor.Queues.AMQP;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOptions();

builder.Services.Configure<QueueOptions>(builder.Configuration.GetSection("queueoptions"));
builder.Services.Configure<AMQPOptions>(builder.Configuration.GetSection("amqp"));

builder.Services.AddRedisConnectionMultiplexer(builder.Configuration);

builder.Services.AddSingleton<AMQPConnectionFactory>();
builder.Services.AddTransient(typeof(EventingBasicConsumer), typeof(AMQPEventingConsumer));
builder.Services.AddSingleton(typeof(ILocationCache), typeof(RedisLocationCache));
builder.Services.AddSingleton(typeof(IEventSubscriber), typeof(AMQPEventSubscriber));
builder.Services.AddSingleton(typeof(IEventEmitter), typeof(AMQPEventEmitter));
builder.Services.AddSingleton(typeof(IEventProcessor), typeof(MemberLocationEventProcessor));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
