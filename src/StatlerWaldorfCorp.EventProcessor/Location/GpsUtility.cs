using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StatlerWaldorfCorp.EventProcessor.Location
{
    public class GpsUtility
    {
        private const double EARTH_CIRCUMFERENCE = 40000.0; // in km

        private double DegreesToRadians(double degrees) => (Math.PI / 180) * degrees;


        /// <summary>
        /// Calculates the distance between two GPS coordinate points.
        /// </summary>
        /// <param name="point1">The first coordinate point.</param>
        /// <param name="point2">The second coordinate point.</param>
        /// <returns>The distance between the two points in kilometers.</returns>
        public double CalculateDistanceBetweenPoints(GpsCoordinate point1, GpsCoordinate point2)
        {
            double distance = 0.0;

            double lat1Rad = DegreesToRadians(point1.Latitude);
            double long1Rad = DegreesToRadians(point1.Longitude);

            double lat2Rad = DegreesToRadians(point2.Latitude);
            double long2Rad = DegreesToRadians(point2.Longitude);

            double longDifference = Math.Abs(long1Rad - long2Rad);
            if (longDifference > Math.PI) {
                longDifference = 2.0 * Math.PI - longDifference;
            }

            double degreesCalculation = Math.Acos(
                Math.Sin(lat2Rad) * Math.Sin(lat1Rad) +
                Math.Cos(lat2Rad) * Math.Cos(lat1Rad) *
                Math.Cos(longDifference));

            distance = EARTH_CIRCUMFERENCE * degreesCalculation / (2.0 * Math.PI);

            return distance;
        }
    }
}