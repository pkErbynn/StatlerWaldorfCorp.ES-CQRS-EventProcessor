using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StatlerWaldorfCorp.EventProcessor.Location;
using StatlerWaldorfCorp.EventProcessor.Queues;

namespace StatlerWaldorfCorp.EventProcessor.Events
{
    /// <summary>
    /// Responsible for:
    /// 1. Subscribing to queue and receiving new messages from the event stream
    /// 2. writing message to event store (not implemented)
    /// 3. processing event stream to detect proximity
    /// 4. stream processing results emited as messages to a queue
    /// 5. submit state changes to cache/reality servcer as result of the steam processing
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