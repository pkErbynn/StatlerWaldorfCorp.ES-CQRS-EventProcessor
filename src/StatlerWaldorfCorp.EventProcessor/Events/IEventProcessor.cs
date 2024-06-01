using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StatlerWaldorfCorp.EventProcessor.Events
{
    public interface IEventProcessor
    {
        void start();
        void stop();
    }
}