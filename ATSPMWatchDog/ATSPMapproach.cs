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

// Public Enum ATSPMdirectionType
// Basic = 1
// AdvancedCount
// AdvancedSpeed
// LaneByLaneCount
// LaneByLaneWithSpeedRestriction
// StopBarPresence
// End Enum

namespace ATSPMWatchDog
{

    public class ATSPMapproach
    {
        [JsonProperty("approachId")]
        public long ApproachID { get; set; }

        [JsonProperty("signalId")]
        public int SignalID { get; set; }

        [JsonProperty("versionId")]
        public int VersionID { get; set; }

        [JsonProperty("directionTypeId", NullValueHandling = NullValueHandling.Ignore)]
        public ATSPMdirectionType DirectionTypeID { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty("mph", NullValueHandling = NullValueHandling.Ignore)]
        public int? MPH { get; set; }

        [JsonProperty("protectedPhaseNumber")]
        public byte ProtectedPhaseNumber { get; set; }

        [JsonProperty("isProtectedPhaseOverlap")]
        public bool IsProtectedPhaseOverlap { get; set; }

        [JsonProperty("permissivePhaseNumber")]
        public byte? PermissivePhaseNumber { get; set; }

        [JsonProperty("isPermissivePhaseOverlap")]
        public bool IsPermissivePhaseOverlap { get; set; }

        [JsonProperty("detectors")]
        public List<ATSPMdetector> Detectors { get; set; }

        //// from partial class - added 1/30/19
        //public List<ATSPMdetector> GetAllDetectorsOfDetectionType(int detectionTypeID)
        //{
        //    if (Detectors != null)
        //    {
        //        foreach (var d in Detectors)
        //        {
        //            if (d.DetectionTypeIDs == null)
        //            {
        //                d.DetectionTypeIDs = new List<int>();
        //                foreach (ATSPMdetectionType dt in d.DetectionTypes)
        //                {
        //                }
        //            }
        //        }
        //    }
        //}

        // added 3/29/19
        public List<ATSPMdetector> GetDetectorsForMetricType(int metricTypeId = 1)
        {
            List<ATSPMdetector> detectorsForMetricType = new List<ATSPMdetector>();

            if (Detectors != null)
            {
                foreach (var d in Detectors)
                {
                    if (d.DetetorSupportsThisMetric(metricTypeId))
                        detectorsForMetricType.Add(d);
                }
            }
            return detectorsForMetricType;
        }


        public ATSPMapproach()
        {
            Detectors = new List<ATSPMdetector>();
        }
    }
}