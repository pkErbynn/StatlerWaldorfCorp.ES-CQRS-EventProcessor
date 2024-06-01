using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StatlerWaldorfCorp.EventProcessor.Queues;

namespace StatlerWaldorfCorp.EventProcessor.Events
{
    public class MemberLocationEventProcessor : IEventProcessor
    {
        private ILogger logger;
        private IEventSubscriber subscriber;

        private IEventEmitter eventEmitter;

        private ProximityDetector proximityDetector;

        private ILocationCache locationCache;
        
        public void start()
        {
            throw new NotImplementedException();
        }

        public void stop()
        {
            throw new NotImplementedException();
        }
    }
}