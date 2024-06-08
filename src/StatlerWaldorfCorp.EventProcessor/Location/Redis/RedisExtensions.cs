using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace StatlerWaldorfCorp.EventProcessor.Location.Redis
{
    public static class RedisExtensions
    {
        public static IServiceCollection AddRedisConnectionMultiplexer(this IServiceCollection services, IConfiguration configuration)
        {
            if(services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if(configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var redisConfig = configuration.GetSection("redis:configstring").Value;

            services.AddSingleton(typeof(IConnectionMultiplexer), ConnectionMultiplexer.ConnectAsync(redisConfig).Result);

            return services;
        }
    }
}