using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace StatlerWaldorfCorp.EventProcessor.Location.Redis
{
    public class RedisLocationCache : ILocationCache
    {
        private ILogger logger;
        private IConnectionMultiplexer connection;

        public RedisLocationCache(ILogger<RedisLocationCache> logger, IConnectionMultiplexer connectionMultiplexer)
        {
            this.logger = logger;
            this.connection = connectionMultiplexer;

            logger.LogInformation($"Using redis locaiton cache - {connectionMultiplexer.Configuration}");
        }

        public RedisLocationCache(ILogger<RedisLocationCache> logger, ConnectionMultiplexer connectionMultiplexer)
        : this(logger, (IConnectionMultiplexer)connectionMultiplexer)
        {
        }

        public IList<MemberLocation> GetMemberLocations(Guid teamId)
        {
            IDatabase db = connection.GetDatabase();
            RedisValue[] values = db.HashValues(teamId.ToString());

            return ConvertRedisValuesToLocationList(values);
        }

        public void Put(Guid teamId, MemberLocation memberLocation)
        {
            IDatabase db = connection.GetDatabase();
            db.HashSet(teamId.ToString(), memberLocation.MemberId.ToString(), memberLocation.toJsonString());   // { "teamId": { "MemberId1": "memberLocation1"  } } 
        }

        private IList<MemberLocation> ConvertRedisValuesToLocationList(RedisValue[] redisValues)
        {
            List<MemberLocation> memberLocations = new List<MemberLocation>();
            foreach (var value in redisValues)
            {
                string val = (string)value;
                MemberLocation ml = MemberLocation.fromJsonStringToModel(val);
                memberLocations.Add(ml);
            }
            return memberLocations;
        }
    }
}