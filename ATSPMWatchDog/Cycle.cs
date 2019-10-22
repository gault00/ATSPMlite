using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATSPMWatchDog
{

    public class Cycle
    {
        public enum NextEventResponse
        {
            GroupOK,
            GroupMissingData,
            GroupComplete
        }

        public enum EventType
        {
            ChangeToRed,
            ChangeToGreen,
            ChangeToYellow,
            Unknown
        }

        public enum TerminationType
        {
            ForceOff,
            GapOut,
            MaxOut,
            Unknown
        }

        /// <summary>
        /// Start time of the cycle
        /// </summary>
        /// <returns></returns>
        public DateTime CycleStart { get; private set; }

        // renamed from EndTime to CycleEnd 11/19/17 - v4 UDOT
        /// <summary>
        /// End time of the Cycle
        /// </summary>
        /// <returns></returns>
        public DateTime CycleEnd { get; private set; }

        /// <summary>
        /// Y coordinate for the green line on the chart
        /// </summary>
        /// <returns></returns>
        public double GreenLineY { get; private set; }

        /// <summary>
        /// Y coordinate of the yellow line on the chart
        /// </summary>
        /// <returns></returns>
        public double YellowLineY { get; private set; }

        /// <summary>
        /// Y coordinate for the red line on the chart
        /// </summary>
        /// <returns></returns>
        public double RedLineY { get; private set; }

        /// <summary>
        /// The next event status
        /// </summary>
        /// <returns></returns>
        public TerminationType Termination { get; set; }

        /// <summary>
        /// ?
        /// </summary>
        /// <returns></returns>
        public NextEventResponse Status { get; private set; }

        /// <summary>
        /// A collection of detector activations for the cycle
        /// </summary>
        public List<DetectorDataPoint> DetectorCollection { get; set; }

        /// <summary>
        /// A collection of preempt activations for the cycle
        /// </summary>
        public List<DetectorDataPoint> PreemptCollection { get; set; }

        // Changed from GreenEvent to ChangeToGreen 11/19/17 - v4 UDOT
        /// <summary>
        /// Green time of the cycle
        /// </summary>
        /// <returns></returns>
        public DateTime ChangeToGreen { get; private set; }

        // Renamed from YellowEvent to BeginYellowClear 11/19/17 - v4 UDOT
        /// <summary>
        /// Yellow time of the Cycle
        /// </summary>
        /// <returns></returns>
        public DateTime BeginYellowClear { get; private set; }

        // added v4 UDOT 11/19/17
        /// <summary>
        /// Timestamp for end of yellow/beginning of red
        /// </summary>
        /// <returns></returns>
        public DateTime ChangeToRed { get; set; }

        private double mTotalArrivalOnGreen = -1;
        public double TotalArrivalOnGreen
        {
            get
            {
                if (mTotalArrivalOnGreen == -1)
                    mTotalArrivalOnGreen = DetectorCollection.Where(d => d.ArrivalOnGreen == true).Count();
                return mTotalArrivalOnGreen;
            }
        }

        private double mTotalArrivalOnRed;
        public double TotalArrivalOnRed
        {
            get
            {
                if (mTotalArrivalOnRed == -1)
                    mTotalArrivalOnRed = DetectorCollection.Where(d => d.ArrivalOnGreen == false).Count();
                return mTotalArrivalOnRed;
            }
        }

        public double TotalDelay
        {
            get
            {
                return DetectorCollection.Sum(d => d.Delay);
            }
        }

        private double mTotalVolume = -1;
        public double TotalVolume
        {
            get
            {
                if (mTotalVolume == -1)
                    mTotalVolume = DetectorCollection.Count;
                return mTotalVolume;
            }
        }

        private double mTotalGreenTime = -1;
        public double TotalGreenTime
        {
            get
            {
                // revised 11/19/17 but not implemented
                if (mTotalGreenTime == -1)
                    mTotalGreenTime = (CycleEnd - ChangeToGreen).TotalSeconds;
                // If mTotalGreenTime = -1 Then mTotalGreenTime = (BeginYellowClear - ChangeToGreen).TotalSeconds
                return mTotalGreenTime;
            }
        }

        private double mTotalYellowTime = -1;
        public double TotalYellowTime
        {
            get
            {
                if (mTotalYellowTime == -1)
                    mTotalYellowTime = (CycleEnd - BeginYellowClear).TotalSeconds;
                return mTotalYellowTime;
            }
        }

        private double mTotalRedTime = -1;
        public double TotalRedTime
        {
            get
            {
                // revised 11/19/17 but not implemented
                // If mTotalRedTime = -1 Then mTotalRedTime = (BeginYellowClear - CycleStart).TotalSeconds
                // If mTotalRedTime = -1 Then mTotalRedTime = (ChangeToRed - CycleEnd).TotalSeconds

                // revised version by SG
                if (mTotalRedTime == -1)
                    mTotalRedTime = (ChangeToGreen - CycleStart).TotalSeconds;
                return mTotalRedTime;
            }
        }

        public double TotalTime
        {
            get
            {
                return (CycleEnd - CycleStart).TotalSeconds;
            }
        }

        /// <summary>
        /// Constructor for the cycle
        /// </summary>
        /// <param name="start_time"></param>
        public Cycle(DateTime start_time)
        {
            CycleStart = start_time;
            GreenLineY = 0;
            YellowLineY = 0;
            RedLineY = 0;
            DetectorCollection = new List<DetectorDataPoint>();
            PreemptCollection = new List<DetectorDataPoint>();
        }

        public void AddDetector(DetectorDataPoint ddp)
        {
            DetectorCollection.Add(ddp);
        }

        public void AddPreempt(DetectorDataPoint ddp)
        {
            PreemptCollection.Add(ddp);
        }

        public void ClearDetectorData()
        {
            mTotalArrivalOnRed = -1;
            mTotalGreenTime = -1;
            mTotalArrivalOnGreen = -1;
            mTotalVolume = -1;
            DetectorCollection.Clear();
        }

        /// <summary>
        /// Gets the next event in the cycle
        /// </summary>
        /// <param name="event_Type"></param>
        /// <param name="timeStamp"></param>
        public void NextEvent(EventType event_Type, DateTime timeStamp)
        {
            // if the event is green add its' data
            if (event_Type == EventType.ChangeToGreen)
            {
                // check to see the last event was not a change to green
                if (GreenLineY == 0)
                {
                    // check for bad data
                    if (CycleStart != DateTime.MinValue)
                    {
                        GreenLineY = (timeStamp - CycleStart).TotalSeconds;
                        ChangeToGreen = timeStamp;
                        Status = NextEventResponse.GroupOK;
                    }
                    else
                        Status = NextEventResponse.GroupMissingData;
                }
                else
                    Status = NextEventResponse.GroupOK;
            }
            else if (event_Type == EventType.ChangeToYellow)
            {
                // check to see that the last event was not a change to yellow
                if (YellowLineY == 0)
                {
                    // check that the greenline y coordinate was already added
                    // then add the data
                    if (CycleStart != DateTime.MinValue & GreenLineY != 0)
                    {
                        YellowLineY = (timeStamp - CycleStart).TotalSeconds;
                        BeginYellowClear = timeStamp;
                        Status = NextEventResponse.GroupOK;
                    }
                    else
                        Status = NextEventResponse.GroupMissingData;
                }
                else
                    Status = NextEventResponse.GroupOK;
            }
            else if (event_Type == EventType.ChangeToRed)
            {
                // check to see if the green, yellow and starting red was added
                // if not, create the next group
                if (CycleStart == DateTime.MinValue & YellowLineY == 0 & GreenLineY == 0 & RedLineY == 0)
                {
                    CycleStart = timeStamp;
                    ChangeToRed = timeStamp;    // ??
                    Status = NextEventResponse.GroupOK;
                }
                else
                    // if the yellow and green y coordinates have been added and the start time is valid add the red event as the ending red
                    if (CycleStart != DateTime.MinValue & YellowLineY != 0 & GreenLineY != 0)
                {
                    RedLineY = (timeStamp - CycleStart).TotalSeconds;
                    Status = NextEventResponse.GroupComplete;
                    CycleEnd = timeStamp;
                    ChangeToRed = timeStamp;    // ??
                }
                else
                    Status = NextEventResponse.GroupMissingData;
            }
            else
                Status = NextEventResponse.GroupOK;
        }
    }
}
