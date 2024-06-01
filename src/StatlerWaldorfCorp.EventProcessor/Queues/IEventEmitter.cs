using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StatlerWaldorfCorp.EventProcessor.Events;

namespace StatlerWaldorfCorp.EventProcessor.Queues
{
    public interface IEventEmitter
    {
        void EmitProximityDetectedEvent(ProximityDetectedEvent proximityDetectedEvent);
    }
}