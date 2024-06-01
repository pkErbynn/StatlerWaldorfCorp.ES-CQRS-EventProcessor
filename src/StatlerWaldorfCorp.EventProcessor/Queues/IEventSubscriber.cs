using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StatlerWaldorfCorp.EventProcessor.Events;

namespace StatlerWaldorfCorp.EventProcessor.Queues
{
    public delegate void MemberLocationRecordedEventReceivedDelegate(MemberLocationRecordedEvent recordedEvent);

    public interface IEventSubscriber
    {
        void Subscribe();
        void Unsubscribe();

        event MemberLocationRecordedEventReceivedDelegate MemberLocationRecordedEventReceived;
    }
}