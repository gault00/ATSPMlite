using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATSPMWatchDog
{
    public class DetectorDataPoint
    {
        /// <summary>
        /// Represents a time span from the start of the red to red cycle
        /// </summary>
        public double YPoint { get; }

        /// <summary>
        /// The actual time of the detector activation
        /// </summary>
        public DateTime TimeStamp { get; }
        public double Delay { get; }
        public bool ArrivalOnGreen { get; }


        /// <summary>
        /// Constructor for the DetectorDataPoint. Sets the timestamp
        /// and the y coordinate.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="eventTime"></param>
        public DetectorDataPoint(DateTime startDate, DateTime eventTime, DateTime greenEvent, DateTime redEvent)
        {
            TimeStamp = eventTime;
            YPoint = (eventTime - startDate).TotalSeconds;
            if (eventTime < greenEvent)
            {
                Delay = (greenEvent - eventTime).TotalSeconds;
                ArrivalOnGreen = false;
            }
            else
            {
                Delay = 0;
                ArrivalOnGreen = true;
            }
        }
    }
}
