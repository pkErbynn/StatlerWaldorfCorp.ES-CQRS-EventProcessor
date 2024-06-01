using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StatlerWaldorfCorp.EventProcessor.Location;

namespace StatlerWaldorfCorp.EventProcessor.Events
{
    /// <summary>
    /// This method assumes that the memberLocations collection only includes
    /// members relevant for proximity detection.
    /// 
    /// Non-team members should be filtered out before using this method.
    /// The distance threshold unit is specified in kilometers.
    /// </summary>
    public class ProximityDetector
    {
        public void DetectProximityEvents(
            MemberLocationRecordedEvent memberLocationRecordedEvent,
            ICollection<MemberLocation> memberLocations,
            double distanceThreshold
        ){
            
        }
    }
}