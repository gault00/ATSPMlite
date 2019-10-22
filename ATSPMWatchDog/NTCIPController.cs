using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSPMWatchDog
{
    public class NTCIPController : NTCIPBase
    {
        // ENUMERATIONS
        public enum OffsetReferenceType
        {
            BeginningOfGreen = 0,
            BeginningOfYellow = 1,
            BeginningOfRed = 2,
            TS2FirstGreen = 3,
            Type170 = 4
        }

        public enum CoordinationForceModeType
        {
            OtherForceMode = 1,
            Floating = 2,
            Fixed = 3
        }

        public enum AutoPedClearType
        {
            Disable = 1,
            Enable = 2
        }

        public enum UnitControlStatusType
        {
            other = 1,
            systemControl = 2,
            systemStandby = 3,
            backupMode = 4,
            manual = 5,
            timebase = 6,
            interconnect = 7,
            interconnectBackup = 8
        }

        public enum UnitFlashStatusType
        {
            other = 1,
            notFlash = 2,
            automatic = 3,
            localManual = 4,
            faultMonitor = 5,
            mmu = 6,
            startup = 7,
            preempt = 8
        }

        public struct SeqKey
        {
            public byte SeqNum;
            public byte RingNum;
        }

        public struct SplitKey
        {
            public byte SplitNum;
            public byte PhaseNum;
        }

        /// <summary>
        /// The maximum number of phases this controller supports. Indicates the maximum number of rows in the phaseTable object
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public byte MaxPhases { get; set; }

        /// <summary>
        /// The Maximum Number of Phase Groups (8 Phases per group) this Actuated Controller Unit 
        /// supports. This value is equal to TRUNCATE [(maxPhases + 7) / 8]. This object indicates 
        /// the maximum rows which shall appear in the phaseStatusGroupTable and phaseControlGroupTable.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public byte MaxPhaseGroups
        {
            get
            {
                return System.Convert.ToByte(Math.Truncate((MaxPhases + 7) / 8.0));
            }
        }

        /// <summary>
        /// The maximum number of vehicle detectors this actuated controller supports. Indicates the maximum number of rows in the the vehicleDetectorTable object.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public byte MaxVehicleDetectors { get; set; }

        /// <summary>
        /// The maximum number of detector status groups (8 detectors per group) this device supports. This value is equal to TRUNCATE [(maxVehicleDetectors + 7 ) / 8]. This object indicates the maximum number of rows which shall appear in the vehicleDetectorStatusGroupTable object.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public byte MaxDetectorStatusGroups
        {
            get
            {
                return System.Convert.ToByte(Math.Truncate((MaxVehicleDetectors + 7) / 8.0));
            }
        }

        /// <summary>
        /// The Maximum Number of Pedestrian Detectors this Actuated Controller Unit supports. This object indicates the maximum rows which shall appear in the pedestrianDetectorTable object.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public byte MaxPedestrianDetectors { get; set; }

        /// <summary>
        /// This object contains the maximum number of alarm groups 
        /// (8 alarm inputs per group) this device supports. This
        ///  object indicates the maximum rows which shall appear 
        /// in the alarmGroupTable object.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public byte MaxAlarmGroups { get; set; }

        /// <summary>
        /// The maximum number of Patterns this Actuated Controller Unit supports. 
        /// This object indicates how many rows are in the patternTable object 
        /// (254 and 255 are defined as non-pattern status for Free and Flash).
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public byte MaxPatterns { get; set; }

        /// <summary>
        /// The maximum number of Split Plans this Actuated Controller Unit supports. 
        /// This object indicates how many Split plans are in the splitTable object.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public byte MaxSplits { get; set; }

        /// <summary>
        /// The Maximum Number of Actions this device supports. This object indicates the maximum rows which shall appear in the timebaseAscActionTable object.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public byte MaxTimeBaseActions { get; set; }

        /// <summary>
        /// The value of this object shall specify the maximum number of rings this device supports.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public byte MaxRings { get; set; }

        /// <summary>
        /// The value of this object shall specify the maximum number of sequence plans this device supports.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public byte MaxSequences { get; set; }

        /// <summary>
        /// The maximum number of Ring Control Groups (8 rings per group) this Actuated Controller Unit supports. 
        /// This value is equal to TRUNCATE[(maxRings + 7) / 8]. This object indicates the maximum rows which shall appear in the ringControlGroupTable object.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public byte MaxRingControlGroups
        {
            get
            {
                return System.Convert.ToByte(Math.Truncate((MaxRings + 7) / 8.0));
            }
        }

        /// <summary>
        ///         ''' The Maximum Number of Preempts this Actuated Controller Unit supports. This object indicates the maximum rows which shall appear in the preemptTable object.
        ///         ''' </summary>
        ///         ''' <value></value>
        ///         ''' <returns></returns>
        ///         ''' <remarks></remarks>
        public byte MaxPreempts { get; set; }

        /// <summary>
        /// The Maximum Number of Channels this Actuated Controller Unit supports. This object indicates the maximum rows which shall appear in the channelTable object.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public byte MaxChannels { get; set; }

        /// <summary>
        /// The maximum number of Channel Status Groups (8 channels per group) this Actuated Controller Unit supports. 
        /// This value is equal to TRUNCATE [(maxChannels + 7) / 8]. This object indicates the maximum rows which shall appear in the channelStatusGroupTable object.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public byte MaxChannelStatusGroups
        {
            get
            {
                return System.Convert.ToByte(Math.Truncate((MaxChannels + 7) / 8.0));
            }
        }

        /// <summary>
        /// The Maximum Number of Overlaps this Actuated Controller Unit supports. 
        /// This object indicates the maximum number of rows which shall appear in the overlapTable object.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public byte MaxOverlaps { get; set; }

        /// <summary>
        /// The Maximum Number of Overlap Status Groups (8 overlaps per group) this Actuated Controller Unit supports. 
        /// This value is equal to TRUNCATE [(maxOverlaps + 7) / 8]. This object indicates the maximum rows which shall appear in the overlapStatusGroupTable object.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public byte MaxOverlapStatusGroups
        {
            get
            {
                return System.Convert.ToByte(Math.Truncate((MaxOverlaps + 7) / 8.0));
            }
        }

        /// <summary>
        ///          The Maximum Number of Port 1 addresses this Actuated Controller Unit supports. 
        ///          This object indicates the maximum rows which shall appear in the port1Table object.
        ///          </summary>
        ///          <value></value>
        ///          <returns></returns>
        ///          <remarks></remarks>
        public byte MaxPort1Addresses { get; set; }

        /// <summary>
        ///  
        ///  </summary>
        ///  <value></value>
        ///  <returns></returns>
        ///  <remarks></remarks>
        public byte MaxTimeBaseScheduleEntries { get; set; }

        /// <summary>
        ///  Unit Start up Flash time parameter in seconds (0 to 255 sec). The period/state 
        ///  (Start-Up Flash occurs when power is restored following a device defined power 
        ///  interruption. During the Start-Up Flash state, the Fault Monitor and Voltage 
        ///  Monitor outputs shall be inactive (if present).
        ///  </summary>
        ///  <value></value>
        ///  <returns></returns>
        ///  <remarks></remarks>
        public byte StartUpFlash { get; set; }

        /// <summary>
        ///  Unit Automatic Ped Clear parameter (1 = False/Disable 2=True/Enable). 
        ///  When enabled, the CU shall time the Pedestrian Clearance interval
        ///  when Manual Control Enable is active and prevent the Pedestrian 
        ///  Clearance interval from being terminated by the Interval Advance input.
        ///  </summary>
        ///  <value></value>
        ///  <returns></returns>
        ///  <remarks></remarks>
        public AutoPedClearType AutoPedClear { get; set; }

        /// <summary>
        /// The Backup Time in seconds (0-65535 sec). When any of the defined system 
        /// control parameters is SET, the backup timer is reset. After reset it times 
        /// the unitBackupTime interval. If the unitBackupTime interval expires without 
        /// a SET operation to any of the system control parameters, then the CU shall
        /// revert to Backup Mode. A value of zero (0) for this object shall disable 
        /// this feature. The system control parameters are:
        /// phaseControlGroupPhaseOmit,
        /// phaseControlGroupPedOmit,
        /// phaseControlGroupHold,
        /// phaseControlGroupForceOff,
        /// phaseControlGroupVehCall,
        /// phaseControlGroupPedCall,
        /// systemPatternControl,
        /// systemSyncControl,
        /// preemptControlState,
        /// ringControlGroupStopTime,
        /// ringControlGroupForceOff,
        /// ringControlGroupMax2,
        /// ringControlGroupMaxInhibit,
        /// ringControlGroupPedRecycle,
        /// ringControlGroupRedRest,
        /// ringControlGroupOmitRedClear,
        /// unitControl,
        /// specialFunctionOutputState (deprecated), and
        /// specialFunctionOutputControl.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int BackupTime { get; set; }

        private byte m_RedRevert;
        /// <summary>
        ///The red revert in tenth seconds ( 0.0 - 25.5 sec). 
        ///This value shall provide the minimum red revert time 
        ///for all phases (i.e. if it is greater than a phaseRedRevert 
        ///object value, then this value shall be used as the red 
        ///revert time for the affected phase). This object provides 
        ///a minimum Red indication following the Yellow Change interval
        ///and prior to the next display of Green on the same 
        ///signal output driver group.
        ///</summary>
        ///<value></value>
        ///<returns></returns>
        ///<remarks></remarks>
        public double RedRevert
        {
            get
            {
                return m_RedRevert / 10.0;
            }
            set
            {
                m_RedRevert = System.Convert.ToByte(value * 10.0);
            }
        }

        /// <summary>
        /// The Control Mode for Pattern, Flash, or Free at the device:
        /// other: control by a source other than those listed here.
        /// systemControl: control by master or central commands.
        /// systemStandby: control by local based on master or central command to use local control.
        /// backupMode: Backup Mode (see Terms).
        /// manual: control by entry other than zero in coordOperationalMode.
        /// timebase: control by the local Time Base.
        /// interconnect: control by the local Interconnect inputs.
        /// interconnectBackup: control by local TBC due to invalid Interconnect inputs or loss of sync.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public UnitControlStatusType UnitControlStatus { get; set; }


        /// <summary>
        ///         ''' The Flash modes:
        ///         ''' other: the CU is in flash for some other reason.
        ///         ''' notFlash: the CU is not in Flash
        ///         ''' automatic: the CU is currently in an Automatic Flash state.
        ///         ''' localManual: the Controller Unit Local Flash input is active, 
        ///         ''' MMU Flash input is not active, and Flash is not commanded by the Master.
        ///         ''' faultMonitor: the CU is currently in a Fault Monitor State.
        ///         ''' mmu: the Controller Unit MMU Flash input is active and the CU is not in Start-Up Flash.
        ///         ''' startup: the CU is currently timing the Start-Up Flash period.
        ///         ''' preempt: the CU is currently timing the preempt Flash.
        ///         ''' </summary>
        ///         ''' <value></value>
        ///         ''' <returns></returns>
        ///         ''' <remarks></remarks>
        public UnitFlashStatusType UnitFlashStatus { get; set; }

        private byte m_UnitAlarmStatus2;
        /// <summary>
        ///         ''' Device Alarm Mask 2 as set by individual bits in 8-bit integer
        ///         ''' </summary>
        ///         ''' <value></value>
        ///         ''' <returns></returns>
        ///         ''' <remarks></remarks>
        public byte UnitAlarmStatus2
        {
            get
            {
                m_UnitAlarmStatus2 = CreateBit8(AlarmPowerRestart, AlarmLowBattery, AlarmResponseFault, AlarmExternalStart, AlarmStopTime, AlarmOffsetTransitioning, false, false);
                return m_UnitAlarmStatus2;
            }
            set
            {
                m_UnitAlarmStatus2 = value;

                // bits 7 & 6 reserved
                AlarmOffsetTransitioning = GetBit8(value, 5);
                AlarmStopTime = GetBit8(value, 4);
                AlarmExternalStart = GetBit8(value, 3);
                AlarmResponseFault = GetBit8(value, 2);
                AlarmLowBattery = GetBit8(value, 1);
                AlarmPowerRestart = GetBit8(value, 0);
            }
        }

        /// <summary>
        /// Whenever the CU is performing an offset transition (correction in process)
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool AlarmOffsetTransitioning { get; set; }

        /// <summary>
        /// When either CU Stop Time Input becomes active.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool AlarmStopTime { get; set; }

        /// <summary>
        /// When the CU External Start becomes active.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool AlarmExternalStart { get; set; }

        /// <summary>
        /// When any NEMA TS2 Port 1 response frame fault occurs.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool AlarmResponseFault { get; set; }

        /// <summary>
        /// When any battery voltage falls below the required level.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool AlarmLowBattery { get; set; }

        /// <summary>
        /// When power returns after a power interruption.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool AlarmPowerRestart { get; set; }

        private byte m_UnitAlarmStatus1;
        /// <summary>
        /// Device Alarm Mask 1 as set by individual bits in an 8-bit integer
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public byte UnitAlarmStatus1
        {
            get
            {
                m_UnitAlarmStatus1 = CreateBit8(AlarmCycleFault, AlarmCoordFault, AlarmCoordFail, AlarmCycleFail, AlarmMMUFlash, AlarmLocalFlash, AlarmLocalFree, AlarmCoordActive);
                return m_UnitAlarmStatus1;
            }
            set
            {
                m_UnitAlarmStatus1 = value;

                AlarmCoordActive = GetBit8(value, 7);
                AlarmLocalFree = GetBit8(value, 6);
                AlarmLocalFlash = GetBit8(value, 5);
                AlarmMMUFlash = GetBit8(value, 4);
                AlarmCycleFail = GetBit8(value, 3);
                AlarmCoordFail = GetBit8(value, 2);
                AlarmCoordFault = GetBit8(value, 1);
                AlarmCycleFault = GetBit8(value, 0);
            }
        }

        /// <summary>
        ///  When coordination is active and not preempted or overridden.
        ///  </summary>
        ///  <value></value>
        ///  <returns></returns>
        ///  <remarks></remarks>
        public bool AlarmCoordActive { get; set; }

        /// <summary>
        /// When any of the CU inputs and/or programming cause it not to run coordination.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool AlarmLocalFree { get; set; }

        /// <summary>
        /// When the Controller Unit Local Flash input becomes active, 
        /// MMU Flash input is not active, and Flash is not commanded by the system.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool AlarmLocalFlash { get; set; }

        /// <summary>
        /// When the Controller Unit MMU Flash input remains active 
        /// for a period of time exceeding the Start-Up Flash time.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool AlarmMMUFlash { get; set; }

        /// <summary>
        /// When a local Controller Unit is operating in the non-coordinated mode,
        /// whether the result of a Cycle Fault or Free being the current normal mode, 
        /// and cycling diagnostics indicate that a serviceable call exists that has 
        /// not been serviced for two cycles.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool AlarmCycleFail { get; set; }

        /// <summary>
        /// When a Coord Fault is in effect and a Cycle Fault occurs 
        /// again within two cycles of the coordination retry.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool AlarmCoordFail { get; set; }

        /// <summary>
        /// When a Cycle Fault is in effect and the serviceable call has 
        /// been serviced within two cycles after the Cycle Fault.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool AlarmCoordFault { get; set; }

        /// <summary>
        /// When the Controller Unit is operating in the coordinated mode 
        /// and cycling diagnostics indicate that a serviceable call exists 
        /// that has not been serviced for two cycles.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool AlarmCycleFault { get; set; }

        private byte m_ShortAlarmStatus;
        /// <summary>
        /// Short Alarm Mask (individual bits of 8-bit integer)
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public byte ShortAlarmStatus
        {
            get
            {
                m_ShortAlarmStatus = CreateBit8(PreemptAlarm, TFFlashAlarm, LocalCycleZeroAlarm, LocalOverrideAlarm, CoordinationAlarm, DetectorFault, NonCriticalAlarm, CriticalAlarm);
                return m_ShortAlarmStatus;
            }
            set
            {
                m_ShortAlarmStatus = value;

                CriticalAlarm = GetBit8(value, 7);
                NonCriticalAlarm = GetBit8(value, 6);
                DetectorFault = GetBit8(value, 5);
                CoordinationAlarm = GetBit8(value, 4);
                LocalOverrideAlarm = GetBit8(value, 3);
                LocalCycleZeroAlarm = GetBit8(value, 2);
                TFFlashAlarm = GetBit8(value, 1);
                CriticalAlarm = GetBit8(value, 0);
            }
        }

        /// <summary>
        /// When the Stop Time input is active.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool CriticalAlarm { get; set; }

        /// <summary>
        /// When an physical alarm input is active.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool NonCriticalAlarm { get; set; }

        /// <summary>
        /// When any detectorAlarm fault occurs.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool DetectorFault { get; set; }

        /// <summary>
        /// When the CU is not running the called pattern without offset
        /// correction within three cycles of the command. An offset 
        /// correction requiring less than three cycles due to cycle 
        /// overrun caused by servicing a pedestrian call shall not cause a
        /// Coordination Alarm.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool CoordinationAlarm { get; set; }

        /// <summary>
        /// When any of the CU inputs and/or programming cause it not to run coordination.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool LocalOverrideAlarm { get; set; }

        /// <summary>
        /// When running coordinated and the Coord Cycle Status (coordCycleStatus) has passed through zero.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool LocalCycleZeroAlarm { get; set; }

        /// <summary>
        /// When either the Local Flash or MMU Flash input becomes active.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool TFFlashAlarm { get; set; }

        /// <summary>
        /// When any of the CU Preempt inputs become active.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool PreemptAlarm { get; set; }

        private byte m_UnitControl;
        /// <summary>
        /// This object is used to allow a remote entity to activate unit functions in the device
        /// Set by individual bits in an 8-bit integer
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public byte UnitControl
        {
            get
            {
                m_UnitControl = CreateBit8(false, false, ExternalMinRecall, CallNonActuated1, CallNonActuated2, WalkRestModifier, Interconnect, DimmingEnable);
                return m_UnitControl;
            }
            set
            {
                m_UnitControl = value;
                DimmingEnable = GetBit8(value, 7);
                Interconnect = GetBit8(value, 6);
                WalkRestModifier = GetBit8(value, 5);
                CallNonActuated2 = GetBit8(value, 4);
                CallNonActuated1 = GetBit8(value, 3);
                ExternalMinRecall = GetBit8(value, 2);
            }
        }

        /// <summary>
        /// when set to 1, causes channel dimming to operate as configured. 
        /// For dimming to occur, (this control OR a dimming input) AND 
        /// a 'timebaseAscAuxillaryFunction' must be True.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool DimmingEnable { get; set; }

        /// <summary>
        /// when set to 1, shall cause the interconnect inputs to operate 
        /// at a higher priority than the timebase control (TBC On Line).
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool Interconnect { get; set; }

        /// <summary>
        /// when set to 1, causes non-actuated phases to remain in the timed-out 
        /// Walk state (rest in Walk) in the absence of a serviceable conflicting call.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool WalkRestModifier { get; set; }

        /// <summary>
        /// when set to 1, causes any phase(s) appropriately programmed in 
        /// the phaseOptions object to operate in the Non-Actuated Mode.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool CallNonActuated2 { get; set; }

        /// <summary>
        /// when set to 1, causes any phase(s) appropriately programmed in 
        /// the phaseOptions object to operate in the Non-Actuated Mode.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool CallNonActuated1 { get; set; }

        /// <summary>
        /// when set to 1, causes a recurring demand on all 
        /// vehicle phases for a minimum vehicle service.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool ExternalMinRecall { get; set; }

        public byte SplitUnits { get; set; } // 1 = seconds, 2 = percent

        public byte OffsetUnits { get; set; } // 1 = seconds, 2 = percent
        public bool InhibitMax { get; set; }

        public OffsetReferenceType OffsetReference { get; set; }  // 0 = beginning of green, 1 = beginning of yellow, 2 = beginning of red, 3 = TS2 first green, 4 = 170

        /// <summary>
        ///         ''' Pattern Sync Reference in minutes past midnight. 
        ///         ''' When the value is 65535, the controller unit shall use the 
        ///         ''' Action time as the Sync Reference for that pattern. Action 
        ///         ''' time is the hour and minute associated with the active 
        ///         ''' dayPlanEventNumber (as defined in NTCIP 1201).
        ///         ''' </summary>
        ///         ''' <value></value>
        ///         ''' <returns></returns>
        ///         ''' <remarks></remarks>
        public int TimeBasePatternSync { get; set; }

        /// <summary>
        /// This object indicates the current time base Action Table 
        /// row that will be used when the CU is in Time Base operation. 
        /// A value of zero indicates that no time base Action is selected.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public byte TimeBaseActionStatus { get; set; }

        public Dictionary<byte, NTCIPPhase> Phases = new Dictionary<byte, NTCIPPhase>();
        public Dictionary<byte, NTCIPPhaseStatusGroup> PhaseStatusGroups = new Dictionary<byte, NTCIPPhaseStatusGroup>();
        public Dictionary<byte, NTCIPPhaseStatus> PhaseStatuses = new Dictionary<byte, NTCIPPhaseStatus>();
        public Dictionary<byte, NTCIPPhaseControlGroup> PhaseControlGroups = new Dictionary<byte, NTCIPPhaseControlGroup>();
        public Dictionary<byte, NTCIPPhaseControl> PhaseControl = new Dictionary<byte, NTCIPPhaseControl>();
        public Dictionary<byte, NTCIPVehicleDetector> Detectors = new Dictionary<byte, NTCIPVehicleDetector>();
        public Dictionary<byte, NTCIPDetectorStatus> DetectorStatuses = new Dictionary<byte, NTCIPDetectorStatus>();
        public Dictionary<byte, NTCIPDetectorStatusGroup> DetectorStatusGroups = new Dictionary<byte, NTCIPDetectorStatusGroup>();
        public Dictionary<byte, NTCIPPedestrianDetector> PedestrianDetectors = new Dictionary<byte, NTCIPPedestrianDetector>();
        public Dictionary<byte, NTCIPAlarmGroup> AlarmGroups = new Dictionary<byte, NTCIPAlarmGroup>();
        // create AlarmGroups
        // create SpecialFunctionOutputs
        public NTCIPCoordination Coordination = new NTCIPCoordination();
        public Dictionary<byte, NTCIPCoordPattern> CoordPatterns = new Dictionary<byte, NTCIPCoordPattern>();
        public Dictionary<SplitKey, NTCIPSplitPattern> Splits = new Dictionary<SplitKey, NTCIPSplitPattern>();
        public Dictionary<byte, NTCIPTimeBase> TimeBaseActions = new Dictionary<byte, NTCIPTimeBase>();
        public Dictionary<byte, NTCIPPreempt> Preempts = new Dictionary<byte, NTCIPPreempt>();
        public Dictionary<byte, NTCIPPreemptControl> PreempControls = new Dictionary<byte, NTCIPPreemptControl>();
        public Dictionary<SeqKey, NTCIPSequence> Sequences = new Dictionary<SeqKey, NTCIPSequence>();
        public Dictionary<byte, NTCIPRingControl> RingControls = new Dictionary<byte, NTCIPRingControl>();        // need to fix ring controls
        public Dictionary<byte, NTCIPRingStatus> RingStatuses = new Dictionary<byte, NTCIPRingStatus>();
        public Dictionary<byte, NTCIPChannel> Channels = new Dictionary<byte, NTCIPChannel>();
        public Dictionary<byte, NTCIPChannelStatus> ChannelStatuses = new Dictionary<byte, NTCIPChannelStatus>();
        public Dictionary<byte, NTCIPOverlap> Overlaps = new Dictionary<byte, NTCIPOverlap>();
        public Dictionary<byte, NTCIPTimeBaseSchedule> TimeBaseSchedules = new Dictionary<byte, NTCIPTimeBaseSchedule>();


        //public static event EventHandler PhaseStatusChange;

        // constructor
        public NTCIPController()
        {
            OffsetReference = NTCIPController.OffsetReferenceType.BeginningOfYellow;
            // CoordForceMode = 2
            WalkRestModifier = false;
            Interconnect = true;
            SplitUnits = 1;
            OffsetUnits = 1;
            InhibitMax = false;
        }

        public NTCIPController(byte maxPhases, byte maxVehicleDetectors, byte maxPedestrianDetectors, byte maxAlarmGroups, byte maxSpecialFunctionOutputs, byte maxPatterns, byte maxSplits, byte maxTimebaseActions, byte maxPreempts, byte maxRings, byte maxSequences, byte maxChannels, byte maxOverlaps, byte maxPort1Addresses, byte maxTimeBaseScheduleEntries, byte maxDayPlans, byte maxDayPlanEvents)
        {
            // constructor with maximum things to intialize arrays
            {
                MaxPhases = maxPhases;
                MaxVehicleDetectors = maxVehicleDetectors;
                MaxPedestrianDetectors = maxPedestrianDetectors;
                MaxAlarmGroups = maxAlarmGroups;
                MaxPatterns = maxPatterns;
                MaxSplits = maxSplits;
                MaxTimeBaseActions = maxTimebaseActions;
                MaxPreempts = maxPreempts;
                MaxRings = maxRings;
                MaxSequences = maxSequences;
                MaxChannels = maxChannels;
                MaxOverlaps = maxOverlaps;
                MaxPort1Addresses = maxPort1Addresses;
                MaxTimeBaseScheduleEntries = maxTimeBaseScheduleEntries;
            }

            // initialize dictionaries
            for (byte i = 1; i <= this.MaxPhases; i++)
                Phases.Add(i, new NTCIPPhase());

            for (byte i = 1; i <= this.MaxPhaseGroups; i++)
            {
                PhaseStatusGroups.Add(i, new NTCIPPhaseStatusGroup());
                PhaseControlGroups.Add(i, new NTCIPPhaseControlGroup());
            }

            for (byte i = 1; i <= this.MaxVehicleDetectors; i++)
                Detectors.Add(i, new NTCIPVehicleDetector());

            for (byte i = 1; i <= this.MaxDetectorStatusGroups; i++)
                DetectorStatusGroups.Add(i, new NTCIPDetectorStatusGroup());

            for (byte i = 1; i <= this.MaxPedestrianDetectors; i++)
                PedestrianDetectors.Add(i, new NTCIPPedestrianDetector());

            for (byte i = 1; i <= this.MaxAlarmGroups; i++)
                AlarmGroups.Add(i, new NTCIPAlarmGroup());

            for (byte i = 1; i <= this.MaxPatterns; i++)
                CoordPatterns.Add(i, new NTCIPCoordPattern());

            for (byte i = 1; i <= this.MaxSplits; i++)
            {
                for (byte j = 1; j <= this.MaxPhases; j++)
                {
                    SplitKey indexKey;
                    indexKey.SplitNum = i;
                    indexKey.PhaseNum = j;
                    Splits.Add(indexKey, new NTCIPSplitPattern());
                }
            }

            for (byte i = 1; i <= this.MaxTimeBaseActions; i++)
                TimeBaseActions.Add(i, new NTCIPTimeBase());

            for (byte i = 1; i <= this.MaxPreempts; i++)
            {
                Preempts.Add(i, new NTCIPPreempt());
                PreempControls.Add(i, new NTCIPPreemptControl());
            }

            for (byte i = 1; i <= this.MaxSequences; i++)
            {
                for (byte j = 1; j <= this.MaxRings; j++)
                {
                    SeqKey indexKey;
                    indexKey.SeqNum = i;
                    indexKey.RingNum = j;
                    Sequences.Add(indexKey, new NTCIPSequence());
                }
            }

            for (byte i = 1; i <= this.MaxRingControlGroups; i++)
                RingControls.Add(i, new NTCIPRingControl());

            for (byte i = 1; i <= this.MaxRings; i++)
                RingStatuses.Add(i, new NTCIPRingStatus());

            for (byte i = 1; i <= this.MaxChannels; i++)
                Channels.Add(i, new NTCIPChannel());

            // need to create channel status by channel and in groups

            for (byte i = 1; i <= this.MaxOverlaps; i++)
                Overlaps.Add(i, new NTCIPOverlap());

            // need to create overlap status groups by overlap and in groups

            // need to implement port 1 addresses

            // need to implement time base plans
            for (byte i = 1; i <= this.MaxTimeBaseScheduleEntries; i++)
                TimeBaseSchedules.Add(i, new NTCIPTimeBaseSchedule());
        }
    }
}
