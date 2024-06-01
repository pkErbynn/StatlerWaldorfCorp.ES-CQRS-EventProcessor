using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace StatlerWaldorfCorp.EventProcessor.Location
{
    public class MemberLocation
    {
        public Guid MemberId { get; set; }
        public GpsCoordinate Location { get; set; }

        public string toJsonString() {
            return JsonConvert.SerializeObject(this);
        }

        public MemberLocation fromJsonStringToModel(string jsonString){
            return JsonConvert.DeserializeObject<MemberLocation>(jsonString);
        }

    }
}