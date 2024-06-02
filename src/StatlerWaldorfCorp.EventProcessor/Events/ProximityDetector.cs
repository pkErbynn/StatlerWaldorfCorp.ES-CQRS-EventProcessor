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
        /// <summary>
        /// Proximity detector.
        /// 
        /// The location of the member specified in memberLocationRecordedEvent is compared against the locations of all other team members in the memberLocations collection. 
        /// This means each member's location is checked against the locations of other members to identify those who are within the specified distance threshold.
        /// Filtering and Event Creation: Only the members who are found to be within the distance threshold are considered "nearby." 
        /// For each nearby member, a proximity event is created to record the detection.
        /// This approach ensures that the proximity of each individual teammate is calculated against all other teammates' locations (excluding their own), 
        /// enabling the detection of close proximity events within the team.
        ///
        /// /// </summary>
        /// <param name="memberLocationRecordedEvent">current location event pulled from the stream: as source</param>
        /// <param name="memberLocations">repository of ist of teammates with their locations: as target</param>
        /// <param name="distanceThreshold">radius threshold</param>
        public ICollection<ProximityDetectedEvent> DetectProximityEvents(
            MemberLocationRecordedEvent memberLocationRecordedEvent,
            ICollection<MemberLocation> memberLocations,
            double distanceThreshold
        ){            
            // creating gps object from current location event received
            GpsCoordinate sourceCoordinate = new GpsCoordinate(){
                Latitude = memberLocationRecordedEvent.Latitude,
                Longitude = memberLocationRecordedEvent.Longitude
            };

            GpsUtility gpsUtility = new GpsUtility();

            // find other team members who are within the specified distance threshold from the source member's current location.
            var nearbyMembers = memberLocations.Where(
                // ensures the member is not compared with themselves. Non-team members should be filtered out from the memberLocations prior to using this method.
                ml => ml.MemberId != memberLocationRecordedEvent.MemberId &&
                gpsUtility.CalculateDistanceBetweenPoints(sourceCoordinate, ml.Location) < distanceThreshold
            );

            // creating proximity event from member locations
            var proximityDetectedEvents =  nearbyMembers.Select(ml => {
                return new ProximityDetectedEvent() {
                    SourceMemberId = memberLocationRecordedEvent.MemberId,
                    TargetMemberId = ml.MemberId,
                    TeamId = memberLocationRecordedEvent.TeamId,
                    DetectionTime = DateTime.UtcNow.Ticks,
                    SourceMemberLocation = sourceCoordinate,
                    TargetMemberLocation = ml.Location,
                    MemberDistance = gpsUtility.CalculateDistanceBetweenPoints(sourceCoordinate, ml.Location)
                };
            }).ToList();

            return proximityDetectedEvents;
        }
    }
}