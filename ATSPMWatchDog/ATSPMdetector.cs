using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Newtonsoft.Json;

namespace ATSPMWatchDog
{
    public enum ATSPMmovementType
    {
        Thru = 1,
        Right,
        Left,
        ThruRight,
        ThruLeft
    }

    public enum ATSPMlaneType
    {
        Vehicle = 1,
        Bike,
        Pedestrian,
        ExitLane,
        LightRailTransit,
        Bus,
        HighOccupancyVehicle
    }

    public enum ATSPMdetectionType
    {
        Basic = 1,
        AdvancedCount,
        AdvancedSpeed,
        LaneByLaneCount,
        LaneByLaneWithSpeedRestriction,
        StopBarPresence
    }

    public enum ATSPMdetectionHardware
    {
        Unknown = 0,
        WavetronixMatrix,
        WavetronixAdvance,
        InductiveLoops,
        Sensys,
        Video
    }

    public class ATSPMdetector
    {
        public string DetectorID { get; set; }

        private byte _detChannel;
        public byte DetChannel
        {
            get
            {
                return _detChannel;
            }
            set
            {
                if (value != _detChannel)
                {
                    _detChannel = value;
                    if (this.Approach != null && this.Approach.SignalID > 0)
                        DetectorID = this.Approach.SignalID.ToString() + value.ToString();
                }
            }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int DistanceFromStopBar { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? MinSpeedFilter { get; set; }

        public DateTime DateAdded { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? DateDisabled { get; set; }

        // added 1/30/19
        [JsonIgnoreAttribute]
        public List<int> DetectionTypeIDs { get; set; }

        [JsonProperty("DetectionTypes")]
        public List<ATSPMdetectionType> DetectionTypes { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int LaneNumber { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ATSPMmovementType MovementType { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ATSPMlaneType LaneType { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? DecisionPoint { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? MovementDelay { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? LatencyCorrection { get; set; }

        // DetectorComments?

        public ATSPMapproach Approach { get; set; }

        [JsonProperty("detectionHardware", NullValueHandling = NullValueHandling.Ignore)]
        public ATSPMdetectionHardware DetectionHardware { get; set; }

        // added 3/29/19
        public bool DetetorSupportsThisMetric(int metricID)
        {
            bool result = false;
            if (DetectionTypes != null)
            {
                if (metricID == 1)
                {
                    if (DetectionTypes.Contains(ATSPMdetectionType.AdvancedCount))
                        result = true;
                    return result;
                }
            }
            return result;
        }

        // added 3/29/19
        public double GetOffset()
        {
            if (DecisionPoint == null)
                DecisionPoint = 0;
            double offset = Convert.ToDouble((DistanceFromStopBar / (double)(Approach.MPH * 1.467) - DecisionPoint) * 1000);

            return offset;
        }

        public ATSPMdetector()
        {
            MovementType = ATSPMmovementType.Thru;
            LaneNumber = 1;
            DetectionTypes = new List<ATSPMdetectionType>();
        }
    }

}
