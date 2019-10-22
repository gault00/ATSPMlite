using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSPMWatchDog
{
    /// <summary>
    /// ''' This class implements section 2.3.2 of NTCIP 1202 (vehicleDetectorObject).
    /// ''' </summary>
    /// ''' <remarks></remarks>
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

    public class NTCIPVehicleDetector : NTCIPBase
    {
        /// <summary>
        ///     ''' The vehicle detector number for objects in this row. The value shall not exceed the maxVehicleDetectors object value.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public byte Number { get; set; }

        private byte m_DetectorOptions;
        /// <summary>
        ///     ''' The vehicle detector options (based on individual bits in 8-bit integer)
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public byte DetectorOptions
        {
            get
            {
                m_DetectorOptions = CreateBit8(VolumeDetectorOption, OccupancyDetectorOption, YellowLockCallOption, RedLockCallOption, PassageOption, AddedInitialOption, QueueOption, CallOption);
                return m_DetectorOptions;
            }
            set
            {
                m_DetectorOptions = value;

                CallOption = GetBit8(value, 7);
                QueueOption = GetBit8(value, 6);
                AddedInitialOption = GetBit8(value, 5);
                PassageOption = GetBit8(value, 4);
                RedLockCallOption = GetBit8(value, 3);
                YellowLockCallOption = GetBit8(value, 2);
                OccupancyDetectorOption = GetBit8(value, 1);
                VolumeDetectorOption = GetBit8(value, 0);
            }
        }

        /// <summary>
        ///     ''' if Enabled, the CU shall place a demand for vehicular service on the assigned phase when the phase is not timing the green interval and an actuation is present.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public bool CallOption { get; set; }

        /// <summary>
        ///     ''' if Enabled, the CU shall extend the green interval of the assigned phase until a gap occurs (no actuation) or until the green has been active longer than the vehicleDetectorQueueLimit time.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public bool QueueOption { get; set; }

        /// <summary>
        ///     ''' if Enabled, the CU shall accumulate detector actuation counts for use in the added initial calculations. Counts shall be accumulated from the beginning of the yellow interval to the beginning of the green interval.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public bool AddedInitialOption { get; set; }

        /// <summary>
        ///     ''' if Enabled, the CU shall maintain a reset to the associated phase passage timer for the duration of the detector actuation when the phase is green.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public bool PassageOption { get; set; }

        private bool m_RedLockCallOption;
        /// <summary>
        ///     ''' if Enabled, the detector will lock a call to the assigned phase if an actuation occurs while the phase is not timing Green or Yellow. This mode is optional.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public bool RedLockCallOption
        {
            get
            {
                return m_RedLockCallOption;
            }
            set
            {
                m_RedLockCallOption = value;
                if (value == true & YellowLockCallOption == true)
                    m_RedLockCallOption = false;
            }
        }

        private bool m_YellowLockCallOption;
        /// <summary>
        ///     ''' if Enabled, the detector will lock a call to the assigned phase if an actuation occurs while the phase is not timing Green.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public bool YellowLockCallOption
        {
            get
            {
                return m_YellowLockCallOption;
            }
            set
            {
                m_YellowLockCallOption = value;
                if (value == true & RedLockCallOption == true)
                    RedLockCallOption = false;
            }
        }

        /// <summary>
        ///     if Enabled, the detector collects data for the associated detector occupancy object(s). This capability may not be supported on all detector inputs to a device.
        ///     </summary>
        ///     <value></value>
        ///     <returns></returns>
        ///     <remarks></remarks>
        public bool OccupancyDetectorOption { get; set; }

        /// <summary>
        ///     ''' if Enabled, the detector collects data for the associated detector volume object(s). This capability may not be supported on all detector inputs to a device.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public bool VolumeDetectorOption { get; set; }

        private byte m_CallPhase;
        /// <summary>
        ///     ''' This object contains assigned phase number for the detector input associated with this row. The associated detector call capability is enabled when this object is set to a non-zero value. The value shall not exceed the value of maxPhases.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public byte CallPhase
        {
            get
            {
                return m_CallPhase;
            }
            set
            {
                m_CallPhase = value;
                CallOption = true;
            }
        }

        /// <summary>
        ///     ''' Detector Switch Phase Parameter (i.e., Phase Number). The phase to which a vehicle detector actuation shall be switched when the assigned phase is Yellow or Red and the Switch Phase is Green.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public byte SwitchPhase { get; set; }

        private double m_Delay;
        /// <summary>
        ///     ''' Detector Delay Parameter in tenth seconds (0–255.0 sec). The period a detector actuation (input recognition) shall be delayed when the phase is not Green.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public double Delay
        {
            get
            {
                return m_Delay / 10.0;
        }
            set
            {
                m_Delay = value * 10.0;
        }
        }

        private double m_Extend;
        /// <summary>
        ///     ''' Detector Extend Parameter in tenth seconds (0–25.5 sec). The period a vehicle detector actuation (input duration) shall be extended from the point of termination , when the phase is Green.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public double Extend
        {
            get
            {
                return m_Extend / 10.0;
        }
            set
            {
                m_Extend = value * 10.0;
        }
        }

        /// <summary>
        ///     ''' Detector Queue Limit parameter in seconds (0-255 sec). The length of time that an actuation from a queue detector may continue into the phase green. This time begins when the phase becomes green and when it expires any associated detector inputs shall be ignored. This time may be shorter due to other overriding device parameters (i.e. Maximum time, Force Off’s, ...).
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public byte QueueLimit { get; set; }

        /// <summary>
        ///     ''' Detector No Activity diagnostic Parameter in minutes (0–255 min.) . If an active detector does not exhibit an actuation in the specified period, it is considered a fault by the diagnostics and the detector is classified as Failed. A value of 0 for this object shall disable this diagnostic for this detector.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public byte NoActivity { get; set; }

        /// <summary>
        ///     ''' Detector Maximum Presence diagnostic Parameter in minutes (0-255 min.). If an active detector exhibits continuous detection for too long a period, it is considered a fault by the diagnostics and the detector is classified as Failed. A value of 0 for this object shall disable this diagnostic for this detector.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public byte MaximumPresence { get; set; }

        /// <summary>
        ///     ''' Detector Erratic Counts diagnostic Parameter in counts/minute (0-255 cpm). If an active detector exhibits excessive actuations, it is considered a fault by the diagnostics and the detector is classified as Failed. A value of 0 for this object shall disable this diagnostic for this detector.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public byte ErraticCounts { get; set; }

        /// <summary>
        ///     ''' Detector Fail Time in seconds (0..255 sec). If a detector diagnostic indicates that the associated detector input is failed, then a call shall be placed on the associated phase during all non-green intervals. When each green interval begins the call shall be maintained for the length of time specified by this object and then removed. If the value of this object equals the maximum value (255) then a constant call shall be placed on the associated phase (max recall). If the value of this object equals zero then no call shall be placed on the associated phase for any interval (no recall). Compliant devices may support a limited capability for this object (i.e. only max recall or max recall and no recall). At a minimum the max recall setting must be supported.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public byte FailTime { get; set; }

        private byte m_Alarms;
        /// <summary>
        /// This object shall return indications of detector alarms based on individual bits in a 8-bit integer.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public byte Alarms
        {
            get
            {
                m_Alarms = CreateBit8(AlarmNoActivityFault, AlarmMaxPresenceFault, AlarmErraticOutputFault, AlarmCommunicationFault, AlarmConfigurationFault, false, false, AlarmOtherFault);
                return m_Alarms;
            }
            set
            {
                m_Alarms = value;

                AlarmOtherFault = GetBit8(value, 7);
                // bits 6 & 5 reserved
                AlarmConfigurationFault = GetBit8(value, 4);
                AlarmCommunicationFault = GetBit8(value, 3);
                AlarmErraticOutputFault = GetBit8(value, 2);
                AlarmMaxPresenceFault = GetBit8(value, 1);
                AlarmNoActivityFault = GetBit8(value, 0);
            }
        }

        /// <summary>
        ///     ''' The detector has failed due to some other cause.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public bool AlarmOtherFault { get; set; }

        private bool m_AlarmConfigurationFault;
        /// <summary>
        ///     ''' Detector is assigned but is not supported.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public bool AlarmConfigurationFault
        {
            get
            {
                return m_AlarmConfigurationFault;
            }
            set
            {
                m_AlarmConfigurationFault = value;
            }
        }

        private bool m_AlarmCommunicationFault;
        /// <summary>
        ///     ''' Communications to the device (if present) have failed.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public bool AlarmCommunicationFault
        {
            get
            {
                return m_AlarmCommunicationFault;
            }
            set
            {
                m_AlarmCommunicationFault = value;
            }
        }

        private bool m_AlarmErraticOutputFault;
        /// <summary>
        ///     ''' This detector has been flagged as non-operational due to erratic outputs (excessive counts) by the CU detector diagnostic.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public bool AlarmErraticOutputFault
        {
            get
            {
                return m_AlarmErraticOutputFault;
            }
            set
            {
                m_AlarmErraticOutputFault = value;
            }
        }

        private bool m_AlarmMaxPresenceFault;
        /// <summary>
        ///     ''' This detector has been flagged as non-operational due to a presence indicator that exceeded the maximum expected time by the CU detector diagnostic.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public bool AlarmMaxPresenceFault
        {
            get
            {
                return m_AlarmMaxPresenceFault;
            }
            set
            {
                m_AlarmMaxPresenceFault = value;
            }
        }

        private bool m_AlarmNoActivityFault;
        /// <summary>
        ///     ''' This detector has been flagged as non-operational due to lower than expected activity by the CU detector diagnostic.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public bool AlarmNoActivityFault
        {
            get
            {
                return m_AlarmNoActivityFault;
            }
            set
            {
                m_AlarmNoActivityFault = value;
            }
        }

        private byte m_ReportedAlarms;
        /// <summary>
        ///     ''' This object shall return detector device reported alarms (via some communications mechanism). Inductive Loop Detector Alarms are indicated as individual bits of an 8-bit integer.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public byte ReportedAlarms
        {
            get
            {
                m_ReportedAlarms = CreateBit8(ReportedAlarmOther, ReportedAlarmWatchdog, ReportedAlarmOpenLoop, ReportedAlarmShortedLoop, ReportedAlarmExcessiveChange, false, false, false);
                return m_ReportedAlarms;
            }
            set
            {
                m_ReportedAlarms = value;

                // bits 7, 6 & 5 reserved

                ReportedAlarmExcessiveChange = GetBit8(value, 4);
                ReportedAlarmShortedLoop = GetBit8(value, 3);
                ReportedAlarmOpenLoop = GetBit8(value, 2);
                ReportedAlarmWatchdog = GetBit8(value, 1);
                ReportedAlarmOther = GetBit8(value, 0);
            }
        }

        private bool m_ReportedAlarmExcessiveChange;
        /// <summary>
        ///     ''' This detector has been flagged as non-operational due to an inductance change that exceeded expected values.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public bool ReportedAlarmExcessiveChange
        {
            get
            {
                return m_ReportedAlarmExcessiveChange;
            }
            set
            {
                m_ReportedAlarmExcessiveChange = value;
            }
        }

        private bool m_ReportedAlarmShortedLoop;
        /// <summary>
        ///     ''' This detector has been flagged as non-operational due to a shorted loop wire.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public bool ReportedAlarmShortedLoop
        {
            get
            {
                return m_ReportedAlarmShortedLoop;
            }
            set
            {
                m_ReportedAlarmShortedLoop = value;
            }
        }

        private bool m_ReportedAlarmOpenLoop;
        /// <summary>
        ///     ''' This detector has been flagged as non-operational due to an open loop (broken wire).
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public bool ReportedAlarmOpenLoop
        {
            get
            {
                return m_ReportedAlarmOpenLoop;
            }
            set
            {
                m_ReportedAlarmOpenLoop = value;
            }
        }

        private bool m_ReportedAlarmWatchdog;
        /// <summary>
        ///     ''' This detector has been flagged as non-operational due to a watchdog error.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public bool ReportedAlarmWatchdog
        {
            get
            {
                return m_ReportedAlarmWatchdog;
            }
            set
            {
                m_ReportedAlarmWatchdog = value;
            }
        }

        private bool m_ReportedAlarmOther;
        /// <summary>
        ///     ''' This detector has been flagged as non-operational due to some other error.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public bool ReportedAlarmOther
        {
            get
            {
                return m_ReportedAlarmOther;
            }
            set
            {
                m_ReportedAlarmOther = value;
            }
        }

        private bool m_Reset;
        /// <summary>
        ///     ''' This object when set to TRUE (one) shall cause the CU to command the associated detector to reset. This object shall automatically return to FALSE (zero) after the CU has issued the reset command.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public bool Reset
        {
            get
            {
                return m_Reset;
            }
            set
            {
                m_Reset = value;
            }
        }
    }

}
