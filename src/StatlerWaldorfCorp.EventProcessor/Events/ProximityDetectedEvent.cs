using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StatlerWaldorfCorp.EventProcessor.Location;

namespace StatlerWaldorfCorp.EventProcessor.Events
{
    public class ProximityDetectedEvent
    {
        public Guid SourceMemberId { get; set; }
        public Guid TargetMemberId { get; set;}
        public long DetectionTime { get; set; }
        public Guid TeamId { get; set; }
        public GpsCoordinate SourceMemberLocation { get; set; }
        public GpsCoordinate TargetMemberLocation { get; set; }
        public double MemberDistance { get; set; }

        public string toJson() {
            return JsonConvert.SerializeObject(this);
        }
    }
}