using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StatlerWaldorfCorp.EventProcessor.Location;
using StatlerWaldorfCorp.EventProcessor.Queues;

namespace StatlerWaldorfCorp.EventProcessor.Events
{
    /// <summary>
    /// 
    /// </summary>
    public class MemberLocationEventProcessor : IEventProcessor
    {
        private ILogger logger;
        private IEventSubscriber subscriber;

        private IEventEmitter eventEmitter;

        private ProximityDetector proximityDetector;

        private ILocationCache locationCache;

        public MemberLocationEventProcessor(
            ILogger<MemberLocationEventProcessor> logger,
            IEventSubscriber eventSubscriber,
            IEventEmitter eventEmitter,
            ILocationCache locationCache)
        {
            this.logger = logger;
            this.subscriber = eventSubscriber;
            this.eventEmitter = eventEmitter;
            this.proximityDetector = new ProximityDetector();
            this.locationCache = locationCache;

            this.subscriber.MemberLocationRecordedEventReceived += (memberLocationRecordedEventObject) => {

                var memberLocations = locationCache.GetMemberLocations(memberLocationRecordedEventObject.TeamId);
                var proximityEvents = proximityDetector.DetectProximityEvents(memberLocationRecordedEventObject, memberLocations, 30.0f);

                foreach (var proximityEvent in proximityEvents)
                {
                    eventEmitter.EmitProximityDetectedEvent(proximityEvent);
                }

                locationCache.Put(memberLocationRecordedEventObject.TeamId, new MemberLocation{
                    MemberId = memberLocationRecordedEventObject.MemberId,
                    Location = new GpsCoordinate {
                        Latitude = memberLocationRecordedEventObject.Latitude,
                        Longitude = memberLocationRecordedEventObject.Longitude
                    }
                });

            };
        }
        
        public void start()
        {
            this.subscriber.Subscribe();
        }

        public void stop()
        {
            this.subscriber.Unsubscribe();
        }
    }
}