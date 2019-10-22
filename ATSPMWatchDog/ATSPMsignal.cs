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
using System.ComponentModel;

namespace ATSPMWatchDog
{
    public enum ATSPMsignalType
    {
        EconoliteASC3 = 1,
        EconoliteCobalt = 2,
        EconoliteASC3_2070 = 3,
        IntellightMaxTime = 4,
        TrafficwareATC = 5,
        SiemensSEAPAC = 6,
        McCainATCeX = 7,
        PeekATC = 8
    }

    public class ATSPMsignal
    {
        private int _signalID;

        [JsonProperty("signalID")]
        public int SignalID
        {
            get
            {
                return _signalID;
            }
            set
            {
                if (value != _signalID)
                {
                    _signalID = value;
                    if (this.Approaches != null)
                    {
                        foreach (ATSPMapproach a in this.Approaches)
                        {
                            a.SignalID = value;
                            if (a.Detectors != null)
                            {
                                foreach (ATSPMdetector d in a.Detectors)
                                    d.DetectorID = value.ToString() + d.DetChannel;
                            }
                        }
                    }
                }
            }
        }

        [JsonProperty("versionActionId")]
        public int VersionActionId { get; set; }

        // versionAction

        [DisplayName("Version label")]
        [JsonProperty("versionLabel")]
        public string Note { get; set; }

        [DisplayName("Version Start")]
        [JsonProperty("start")]
        public DateTime Start { get; set; }

        [JsonProperty("latitude", NullValueHandling = NullValueHandling.Ignore)]
        public double Latitude { get; set; }

        [JsonProperty("longitude", NullValueHandling = NullValueHandling.Ignore)]
        public double Longitude { get; set; }

        [JsonProperty("primaryName", NullValueHandling = NullValueHandling.Ignore)]
        public string PrimaryName { get; set; }

        [JsonProperty("secondaryName", NullValueHandling = NullValueHandling.Ignore)]
        public string SecondaryName { get; set; }

        [JsonProperty("ipAddress", NullValueHandling = NullValueHandling.Ignore)]
        public string IPaddress { get; set; }

        [DisplayName("Region")]
        [JsonProperty("regionID")]
        public int RegionID { get; set; }

        [JsonProperty("controllerTypeId")]
        public ATSPMsignalType ControllerTypeID { get; set; }

        /// <summary>
        ///     ''' Determines whether the signal should show up on the map
        ///     ''' </summary>
        ///     ''' <returns></returns>
        [DisplayName("Display on map")]
        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        // <JsonProperty("detectors", NullValueHandling:=NullValueHandling.Ignore)>
        public List<Graph_DetectorsUDOT> Detectors { get; set; }

        [JsonProperty("approaches", NullValueHandling = NullValueHandling.Ignore)]
        public List<ATSPMapproach> Approaches { get; set; }

        // added 3/29/19
        public List<ATSPMapproach> GetApproachesForSignalThatSupportMetric(int metricTypeID = 1)
        {
            List<ATSPMapproach> approachesForMetricType = new List<ATSPMapproach>();

            foreach (var a in Approaches)
            {
                foreach (var d in a.Detectors)
                {
                    if (d.DetectionTypes.Contains(ATSPMdetectionType.AdvancedCount))
                        approachesForMetricType.Add(a);
                }
            }

            return approachesForMetricType.OrderBy(a => a.PermissivePhaseNumber).ThenBy(b => b.ProtectedPhaseNumber).ThenBy(c => c.DirectionTypeID.Description).ToList();
        }

        public ATSPMsignal()
        {
            // Detectors = New List(Of Graph_DetectorsUDOT)
            Approaches = new List<ATSPMapproach>();
        }
    }

}

