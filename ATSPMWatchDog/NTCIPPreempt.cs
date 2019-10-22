using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSPMWatchDog
{
    public class NTCIPPreempt : NTCIPBase
    {
        public enum preemptState
        {
            other = 1,
            notActive = 2,
            notActiveWithCall = 3,
            entryStarted = 4,
            trackService = 5,
            dwell = 6,
            linkActive = 7,
            exitStarted = 8,
            maxPresence = 9
        }

        // PROPERTIES------------------------------------------------------------------------------------
        private byte m_Number;
        /// <summary>
        ///     ''' The preempt number for objects in this row. The value shall not exceed the maxPreempts object value.
        ///     ''' When all preemptControl objects have a value where bit 2 = 0, 
        ///     ''' each preemptNumber routine shall be a higher priority and override all preemptNumber routines that have a larger preemptNumber.
        ///     ''' When a preemptControl object has a value where bit 2 = 1, 
        ///     ''' the next higher preemptNumber becomes of equal priority with the preemptNumber 
        ///     ''' but may still be a higher priority than larger preemptNumbers depending on bit 2 of the relavent preemptControl objects.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public byte Number
        {
            get
            {
                return m_Number;
            }
            set
            {
                m_Number = value;
            }
        }

        private byte m_PreemptControl;
        /// <summary>
        ///     ''' Preempt Miscellaneous Control Parameter Mask (individual bits of 8-bit integer)
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public byte PreemptControl
        {
            get
            {
                m_PreemptControl = CreateBit8(NonLocking, OverrideFlash, PreemptOverride, FlashDwell, false, false, false, false);
                return m_PreemptControl;
            }
            set
            {
                m_PreemptControl = value;
                // bits 7, 6, 5, 4 reserved
                FlashDwell = GetBit8(value, 3);
                PreemptOverride = GetBit8(value, 2);
                OverrideFlash = GetBit8(value, 1);
                NonLocking = GetBit8(value, 0);
            }
        }

        private bool m_FlashDwell;
        /// <summary>
        ///     ''' the CU shall cause the phases listed in the preemptDwellPhase object to flash Yellow during the Dwell interval. 
        ///     ''' All active phases not listed in preemptDwellPhase shall flash Red.
        ///     ''' The CU shall cause the overlaps listed in the preemptDwellOverlap object to flash Yellow during the Dwell state. 
        ///     ''' All active overlaps not listed in preemptDwellOverlap shall flash Red.
        ///     ''' Preempt cycling phase programming is ignored if this bit is set.
        ///     ''' This control is optional.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public bool FlashDwell
        {
            get
            {
                return m_FlashDwell;
            }
            set
            {
                m_FlashDwell = value;
            }
        }

        private bool m_PreemptOverride;
        /// <summary>
        ///     ''' provide a means to define whether this preempt shall NOT override the next higher numbered Preempt. 
        ///     ''' When set (1) this preempt shall not override the next higher numbered preempt.
        ///     ''' Lowered numbered preempts override higher numbered preempts. 
        ///     ''' For example, 1 overrides 3, and the only way to get 3 equal to 1, is to set both 1 and 2 to NOT override the next higher numbered preempt. 
        ///     ''' This parameter shall be ignored when preemptNumber equals maxPreempts.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public bool PreemptOverride
        {
            get
            {
                return m_PreemptOverride;
            }
            set
            {
                m_PreemptOverride = value;
            }
        }

        private bool m_OverrideFlash;
        /// <summary>
        ///     ''' provide a means to define whether this preempt shall NOT override Automatic Flash. 
        ///     ''' When set (1) this preempt shall not override Automatic Flash.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public bool OverrideFlash
        {
            get
            {
                return m_OverrideFlash;
            }
            set
            {
                m_OverrideFlash = value;
            }
        }

        private bool m_NonLocking;
        /// <summary>
        ///     ''' provide a means to enable an operation which does not require detector memory. 
        ///     ''' When set (1) a preempt sequence shall not occur if the preempt input terminates prior to expiration of the preemptDelay time.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public bool NonLocking
        {
            get
            {
                return m_NonLocking;
            }
            set
            {
                m_NonLocking = value;
            }
        }

        private byte m_PreemptLink;
        /// <summary>
        ///     ''' This object provides a means to define a higher priority preempt to be combined (linked) with this preempt. 
        ///     ''' At the end of preemptDwellGreen, the linked preempt shall receive an automatic call that shall be maintained 
        ///     ''' as long as the demand for this preempt is active. 
        ///     ''' Any value that is not a higher priority preempt or a valid preempt shall be ignored. 
        ///     ''' The value shall not exceed the maxPreempts object value.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public byte PreemptLink
        {
            get
            {
                return m_PreemptLink;
            }
            set
            {
                m_PreemptLink = value;
            }
        }

        private ushort m_PreemptDelay;
        /// <summary>
        ///     ''' Preempt Delay Time in seconds (0-600 sec). 
        ///     ''' This value determines the time the preempt input shall be active prior to initiating any preempt sequence. 
        ///     ''' A non-locking preempt input which is removed prior to the completion of this time shall not cause a preempt sequence to occur.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public ushort PreemptDelay
        {
            get
            {
                return m_PreemptDelay;
            }
            set
            {
                m_PreemptDelay = value;
            }
        }

        private ushort m_Duration;
        /// <summary>
        ///     ''' Preempt Minimum Duration Time in seconds (0..65535 sec). 
        ///     ''' This value determines the minimum time during which the preempt is active. 
        ///     ''' Duration begins timing at the end of Preempt Delay (if non zero) and will prevent an exit from the Dwell interval until this time has elapsed.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public ushort Duration
        {
            get
            {
                return m_Duration;
            }
            set
            {
                m_Duration = value;
            }
        }

        private byte m_MinimumGreen;
        /// <summary>
        ///     ''' Preempt Minimum Green Time in seconds (0-255 sec). 
        ///     ''' A preempt initiated transition shall not cause the termination of an existing Green 
        ///     ''' prior to its display for lesser of the phase’s Minimum Green time or this period. 
        ///     ''' CAUTION -- if this value is zero, phase Green is terminated immediately.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public byte MinimumGreen
        {
            get
            {
                return m_MinimumGreen;
            }
            set
            {
                m_MinimumGreen = value;
            }
        }

        private byte m_MinimumWalk;
        /// <summary>
        ///     ''' Preempt Minimum Walk Time in seconds (0-255 sec). 
        ///     ''' A preempt initiated transition shall not cause the termination of an existing Walk 
        ///     ''' prior to its display for the lesser of the phase’s Walk time or this period. 
        ///     ''' CAUTION -- if this value is zero, phase Walk is terminated immediately.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public byte MinimumWalk
        {
            get
            {
                return m_MinimumWalk;
            }
            set
            {
                m_MinimumWalk = value;
            }
        }

        private byte m_EnterPedClear;
        /// <summary>
        ///     ''' Enter Ped ClearTime in seconds (0-255 sec). 
        ///     ''' This parameter controls the ped clear timing for a normal Walk signal terminated by a preempt initiated transition. 
        ///     ''' A preempt initiated transition shall not cause the termination of a Pedestrian Clearance 
        ///     ''' prior to its display for the lesser of the phase’s Pedestrian Clearance time or this period. 
        ///     ''' CAUTION -- if this value is zero, phase Ped Clear is terminated immediately.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public byte EnterPedClear
        {
            get
            {
                return m_EnterPedClear;
            }
            set
            {
                m_EnterPedClear = value;
            }
        }

        private byte m_TrackClear;
        /// <summary>
        ///     ''' Track Clear Green Time in seconds (0-255 sec). 
        ///     ''' This parameter controls the green timing for the track clearance movement. 
        ///     ''' Track Clear phase(s) are enabled in the preemptTrackPhase object. 
        ///     ''' If this value is zero, the track clearance movement is omitted, regardless of preemptTrackPhase programming.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public byte TrackClear
        {
            get
            {
                return m_TrackClear;
            }
            set
            {
                m_TrackClear = value;
            }
        }

        private byte m_MinimumDwell;
        /// <summary>
        ///     ''' Minimum Dwell interval in seconds (1-255 sec). 
        ///     ''' This parameter controls the minimum timing for the dwell interval. 
        ///     ''' Phase(s) active during the Dwell interval are enabled in preemptDwellPhase and preemptCyclingPhase objects. 
        ///     ''' The Dwell interval shall not terminate prior to the completion of preemptMinimumDuration, 
        ///     ''' preemptDwellGreen (this object), and the call is no longer present.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public byte MinimumDwell
        {
            get
            {
                return m_MinimumDwell;
            }
            set
            {
                m_MinimumDwell = value;
            }
        }

        private ushort m_MaximumPresence;
        /// <summary>
        ///     ''' Preempt Maximum Presence time in seconds (0-65535 sec). 
        ///     ''' This value determines the maximum time which a preempt call may remain active and be considered valid. 
        ///     ''' When the preempt call has been active for this time period, the CU shall return to normal operation. 
        ///     ''' This preempt call shall be considered invalid until such time as a change in state occurs (no longer active). 
        ///     ''' When set to zero the preempt maximum presence time is disabled.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public ushort MaximumPresence
        {
            get
            {
                return m_MaximumPresence;
            }
            set
            {
                m_MaximumPresence = value;
            }
        }

        private string m_TrackPhase;
        /// <summary>
        ///     ''' Each octet within the octet string contains a phaseNumber(binary value) that shall be active during the Preempt Track Clear intervals.
        ///     ''' The values of phaseNumber used here shall not exceed maxPhases or violate the Consistency Checks defined in Annex B.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public string TrackPhase
        {
            get
            {
                return m_TrackPhase;
            }
            set
            {
                m_TrackPhase = value;
            }
        }

        private string m_DwellPhase;
        /// <summary>
        ///     ''' Each octet within the octet string contains a phaseNumber (binary value) that specifies the phase(s) to be served in the Preempt Dwell interval. 
        ///     ''' The phase(s) defined in preemptCyclingPhase shall occur after those defined herein.
        ///     ''' The values of phaseNumber used here shall not exceed maxPhases or violate the Consistency Checks defined in Annex B.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public string DwellPhase
        {
            get
            {
                return m_DwellPhase;
            }
            set
            {
                m_DwellPhase = value;
            }
        }

        private string m_DwellPed;
        /// <summary>
        ///     ''' Each octet within the octet string contains a phaseNumber (binary value) that specifies the 
        ///     ''' pedestrian movement(s) to be served in the Preempt Dwell interval. The peds defined in 
        ///     ''' premptCyclingPed shall occur after those defined herein.
        ///     ''' The values of phaseNumber used here shall not exceed maxPhases or violate the Consistency Checks defined in Annex B.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public string DwellPed
        {
            get
            {
                return m_DwellPed;
            }
            set
            {
                m_DwellPed = value;
            }
        }

        private string m_ExitPhase;
        /// <summary>
        ///     ''' Each octet within the octet string contains a phaseNumber (binary value) that shall be active following Preempt.
        ///     ''' The values of phaseNumber used here shall not exceed maxPhases or violate the Consistency Checks defined in Annex B.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public string ExitPhase
        {
            get
            {
                return m_ExitPhase;
            }
            set
            {
                m_ExitPhase = value;
            }
        }

        private preemptState m_State;
        /// <summary>
        ///     ''' Preempt State provides status on which state the associated preempt is in. The states are as follows:
        ///     ''' other: preempt service is not specified in this standard.
        ///     ''' notActive: preempt input is not active, this preempt is not active.
        ///     ''' notActiveWithCall: preempt input is active, preempt service has not started.
        ///     ''' entryStarted: preempt service is timing the entry intervals.
        ///     ''' trackService: preempt service is timing the track intervals.
        ///     ''' dwell: preempt service is timing the dwell intervals.
        ///     ''' linkActive: preempt service is performing linked operation.
        ///     ''' exitStarted: preempt service is timing the exit intervals.
        ///     ''' maxPresence: preempt input has exceeded maxPresence time
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public preemptState State
        {
            get
            {
                return m_State;
            }
            set
            {
                m_State = value;
            }
        }

        private string m_TrackOverlap;
        /// <summary>
        ///     ''' Each octet within the octet string contains a overlapNumber (binary value) that shall be active during the Preempt Track Clear intervals.
        ///     ''' The values of overlapNumber used here shall not exceed maxOverlaps or violate the consistency checks defined in Annex B.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public string TrackOverlap
        {
            get
            {
                return m_TrackOverlap;
            }
            set
            {
                m_TrackOverlap = value;
            }
        }

        private string m_DwellOverlap;
        /// <summary>
        ///     ''' Each octet within the octet string contains a overlapNumber (binary value) that is allowed during the Preempt Dwell interval.
        ///     ''' The values of overlapNumber used here shall not exceed maxOverlaps or violate the consistency checks defined in Annex B.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public string DwellOverlap
        {
            get
            {
                return m_DwellOverlap;
            }
            set
            {
                m_DwellOverlap = value;
            }
        }

        private string m_CyclingPhase;
        /// <summary>
        ///     ''' Each octet within the octet string contains a phaseNumber (binary value) that is allowed to cycle during the Preempt Dwell interval.
        ///     ''' The values of phaseNumber used here shall not exceed maxPhases or violate the Consistency Checks defined in Annex B.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public string CyclingPhase
        {
            get
            {
                return m_CyclingPhase;
            }
            set
            {
                m_CyclingPhase = value;
            }
        }

        private string m_CyclingPed;
        /// <summary>
        ///     ''' Each octet within the octet string contains a phaseNumber (binary value) indicating a 
        ///     ''' pedestrian movement that is allowed to cycle during the Preempt Dwell interval.
        ///     ''' The values of phaseNumber used here shall not exceed maxPhases or violate the consistency checks defined in Annex B.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public string CyclingPed
        {
            get
            {
                return m_CyclingPed;
            }
            set
            {
                m_CyclingPed = value;
            }
        }

        private string m_CyclingOverlap;
        /// <summary>
        ///     ''' Each octet within the octet string contains a overlapNumber (binary value) that is allowed to cycle during the Preempt Dwell interval.
        ///     ''' The values of overlapNumber used here shall not exceed maxOverlaps or violate the consistency checks defined in Annex B.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public string CyclingOverlap
        {
            get
            {
                return m_CyclingOverlap;
            }
            set
            {
                m_CyclingOverlap = value;
            }
        }

        private byte m_EnterYellowChange;
        /// <summary>
        ///     ''' Enter Yellow Change in tenth seconds (0-25.5 sec). 
        ///     ''' This parameter controls the yellow change timing for a normal Yellow Change signal terminated by a preempt initiated transition. 
        ///     ''' A preempt initiated transition shall not cause the termination of a Yellow Change 
        ///     ''' prior to its display for the lesser of the phase’s Yellow Change time or this period. 
        ///     ''' 'CAUTION -- if this value is zero, phase Yellow Change is terminated immediately.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public double EnterYellowChange
        {
            get
            {
                return System.Convert.ToDouble(m_EnterYellowChange / 10.0);
        }
            set
            {
                m_EnterYellowChange = System.Convert.ToByte(value * 10.0);
        }
        }

        private byte m_EnterRedClear;
        /// <summary>
        ///     ''' Enter Red Clear in tenth seconds (0-25.5 sec). 
        ///     ''' This parameter controls the red clearance timing for a normal Red Clear signal terminated by a preempt initiated transition. 
        ///     ''' A preempt initiated transition shall not cause the termination of a Red Clear 
        ///     ''' prior to its display for the lesser of the phase’s Red Clear time or this period. 
        ///     ''' CAUTION -- if this value is zero, phase Red Clear is terminated immediately.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public double EnterRedClear
        {
            get
            {
                return System.Convert.ToDouble(m_EnterRedClear / 10.0);
        }
            set
            {
                m_EnterRedClear = System.Convert.ToByte(value * 10.0);
        }
        }

        private byte m_TrackYellowChange;
        /// <summary>
        ///     ''' Track Clear Yellow Change time in tenth seconds (0-25.5 sec). The lesser of the phase’s 
        ///     ''' Yellow Change time or this parameter controls the yellow timing for the track clearance movement. 
        ///     ''' Track clear phase(s) are enabled in the preemptTrackPhase object.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public double TrackYellowChange
        {
            get
            {
                return System.Convert.ToDouble(m_TrackYellowChange / 10.0);
        }
            set
            {
                m_TrackYellowChange = System.Convert.ToByte(value * 10.0);
        }
        }

        private byte m_TrackRedClear;
        /// <summary>
        ///     ''' Track Clear Red Clear time in tenth seconds (0-25.5 sec). The lesser of the phase’s Red Clear 
        ///     ''' time or this parameter controls the Red Clear timing for the track clearance movement. Track clear
        ///     ''' phase(s) are enabled in the preemptTrackPhase object.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public double TrackRedClear
        {
            get
            {
                return System.Convert.ToDouble(m_TrackRedClear / 10.0);
        }
            set
            {
                m_TrackRedClear = System.Convert.ToByte(value * 10.0);
        }
        }
    }
}
