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

builder.Services.AddTransient(typeof(IConnectionFactory), typeof(AMQPConnectionFactory));
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

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

// app.MapGet("/weatherforecast", () =>
// {
//     var forecast =  Enumerable.Range(1, 5).Select(index =>
//         new WeatherForecast
//         (
//             DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//             Random.Shared.Next(-20, 55),
//             summaries[Random.Shared.Next(summaries.Length)]
//         ))
//         .ToArray();
//     return forecast;
// })
// .WithName("GetWeatherForecast")
// .WithOpenApi();

app.Run();

// record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
// {
//     public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
// }
