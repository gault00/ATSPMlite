using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSPMWatchDog
{
    public class NTCIPCoordination
    {
        // PROPERTIES------------------------------------------------------------------------------------
        public enum coordCorrectionMode
        {
            other = 1,
            dwell = 2,
            shortway = 3,
            addOnly = 4
        }

        public enum coordMaximumMode
        {
            other = 1,
            maximum1 = 2,
            maximum2 = 3,
            maxInhibit = 4
        }

        public enum coordForceMode
        {
            other = 1,
            floating = 2,
            @fixed = 3
        }

        public enum LocalFree
        {
            other = 1,
            notFree = 2,
            commandFree = 3,
            transitionFree = 4,
            inputFree = 5,
            coordFree = 6,
            badPlan = 7,
            badCycleTime = 8,
            splitOverrun = 9,
            invalidOffset = 10,
            failed = 11
        }

        private byte m_OperationMode;
        /// <summary>
        ///     ''' This object defines the operational mode for coordination. The possible modes are:
        ///     ''' 0    Automatic - this mode provides for coord operation, free, and flash to be determined
        ///     '''                  automatically by the possible sources (i.e. Interconnect, Time Base, or System Commands).
        ///     ''' 1-253 Manual Pattern - these modes provides for Coord operation running this pattern. 
        ///     '''                        This selection of pattern overrides all other pattern commands.
        ///     ''' 254 Manual Free - this mode provides for Free operation without coordination or Automatic Flash from any source.
        ///     ''' 255 Manual Flash - this mode provides for Automatic Flash without coordination or Free from any source.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public byte OperationMode
        {
            get
            {
                return m_OperationMode;
            }
            set
            {
                m_OperationMode = value;
            }
        }

        private coordCorrectionMode m_CorrectionMode;
        /// <summary>
        ///     ''' This object defines the Coord Correction Mode. The possible modes are:
        ///     ''' other: the coordinator establishes a new offset by a mechanism not defined in this standard.
        ///     ''' dwell: when changing offset, the coordinator shall establish a new offset by dwelling in the coord
        ///     '''        phase(s) until the desired offset is reached.
        ///     ''' shortway (Smooth): when changing offset, the coordinator shall establish a new offset by
        ///     '''                    adding or subtracting to/from the timings in a manner that limits the cycle change. 
        ///     '''                    This operation is performed in a device specific manner.
        ///     ''' addOnly: when changing offset, the coordinator shall establish a new offset by adding to the
        ///     '''          timings in a manner that limits the cycle change. 
        ///     '''          This operation is performed in a device specific manner.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public coordCorrectionMode CorrectionMode
        {
            get
            {
                return m_CorrectionMode;
            }
            set
            {
                m_CorrectionMode = value;
            }
        }

        private coordMaximumMode m_MaximumMode;
        /// <summary>
        ///     ''' This object defines the Coord Maximum Mode. The possible modes are:
        ///     ''' other: the maximum mode is determined by some other mechanism not defined in this standard.
        ///     ''' maximum1: the internal Maximum 1 Timing shall be effective while coordination is running a pattern.
        ///     ''' maximum2: the internal Maximum 2 Timing shall be effective while coordination is running a pattern.
        ///     ''' maxInhibit: the internal Maximum Timing shall be inhibited while coordination is running a pattern.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public coordMaximumMode MaximumMode
        {
            get
            {
                return m_MaximumMode;
            }
            set
            {
                m_MaximumMode = value;
            }
        }

        private coordForceMode m_ForceMode;
        /// <summary>
        ///     ''' This object defines the Pattern Force Mode. The possible modes are:
        ///     ''' other: the CU implements a mechanism not defined in this standard.
        ///     ''' floating: each non-coord phase will be forced to limit its time to the split time value. 
        ///     '''           This allows unused split time to revert to the coord phase.
        ///     ''' fixed: each non-coord phase will be forced at a fixed position in the cycle. 
        ///     '''        This allows unused split time to revert to the following phase.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public coordForceMode ForceMode
        {
            get
            {
                return m_ForceMode;
            }
            set
            {
                m_ForceMode = value;
            }
        }

        private byte m_CoordPatternStatus;
        /// <summary>
        ///     ''' This object defines the running coordination pattern/mode in the device. The possible values are:
        ///     ''' 0 Not used
        ///     ''' 1-253 Pattern - indicates the currently running pattern
        ///     ''' 254 Free - indicates Free operation without coordination.
        ///     ''' 255 Flash - indicates Automatic Flash without coordination.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public byte CoordPatternStatus
        {
            get
            {
                return m_CoordPatternStatus;
            }
            set
            {
                m_CoordPatternStatus = value;
            }
        }

        private LocalFree m_LocalFreeStatus;
        /// <summary>
        ///     ''' The Free modes:
        ///     ''' other: Some other condition has caused the device to run in free mode.
        ///     ''' notFree: The unit is not running in free mode.
        ///     ''' commandFree: the current pattern command is the Free mode pattern.
        ///     ''' transitionFree: the CU has a pattern command but is cycling to a point to begin coordination.
        ///     ''' inputFree: one of the CU inputs cause it to not respond to coordination.
        ///     ''' coordFree: the CU programming for the called pattern is to run Free.
        ///     ''' badPlan: Free - the called pattern is invalid.
        ///     ''' badCycleTime: the pattern cycle time is less than adequate to service the minimum requirements of all phases.
        ///     ''' splitOverrun: Free - the sum of the critical path splitTime’s exceed the programmed patternCycleTime value.
        ///     ''' invalidOffset: Free - reserved / not used
        ///     ''' failed: cycling diagnostics have called for Free.
        ///     ''' 
        ///     ''' An ASC may provide diagnostics beyond those stated herein. 
        ///     ''' Therefore, for a set of given bad data, the free status between devices may be inconsistent.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public LocalFree LocalFreeStatus
        {
            get
            {
                return m_LocalFreeStatus;
            }
            set
            {
                m_LocalFreeStatus = value;
            }
        }

        private int m_CycleStatus;
        /// <summary>
        ///     ''' The Coord Cycle Status represents the current position in the local coord cycle of the running pattern (0 to 510 sec). 
        ///     ''' This value normally counts down from patternCycleTime to Zero. This value may exceed the patternCycleTime during a coord
        ///     ''' cycle with offset correction (patternCycleTime + correction).
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public int CycleStatus
        {
            get
            {
                return m_CycleStatus;
            }
            set
            {
                m_CycleStatus = value;
            }
        }

        private int m_SyncStatus;
        /// <summary>
        ///     ''' The Coord Sync Status represents the time since the system reference point for the running pattern (0 to 510 sec). 
        ///     ''' This value normally counts up from Zero to the next system reference point (patternCycleTime). 
        ///     ''' This value may exceed the patternCycleTime during a coord cycle in which the system reference point has changed.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public int SyncStatus
        {
            get
            {
                return m_SyncStatus;
            }
            set
            {
                m_SyncStatus = value;
            }
        }

        private byte m_SystemPatternControl;
        /// <summary>
        ///     ''' This object is used to establish the Called System Pattern/Mode for the device. 
        ///     ''' The possible values are:
        ///     ''' 0 Standby - the system relinquishes control of the device.
        ///     ''' 1-253 Pattern - these values indicate the system commanded pattern
        ///     ''' 254 Free - this value indicates a call for Free 
        ///     ''' 255 Flash - this value indicates a call for Automatic Flash
        ///     ''' 
        ///     ''' If an unsupported / invalid pattern is called, Free shall be the operational mode.
        ///     ''' The device shall reset this object to ZERO when in BACKUP Mode. 
        ///     ''' A write to this object shall reset the Backup timer to ZERO (see unitBackupTime).
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public byte SystemPatternControl
        {
            get
            {
                return m_SystemPatternControl;
            }
            set
            {
                m_SystemPatternControl = value;
            }
        }

        private byte m_SystemSyncControl;
        /// <summary>
        ///     ''' This object is used to establish the system reference point for the Called System Pattern 
        ///     ''' by providing the current position in the system pattern cycle (0-254 sec). 
        ///     ''' The device shall recognize a write to this object as a command to establish the time until 
        ///     ''' the next system reference point. 
        ///     ''' Thereafter, the system reference point shall be assumed to occur at a frequency equal to the patternCycleTime.
        ///     ''' When the value in the object is 255, the system reference point shall be 
        ///     ''' referenced to the local Time Base in accordance with its programming.
        ///     ''' This CU must maintain an accuracy of 0.1 seconds based on the receipt of the SET packet.
        ///     ''' The device shall reset this object to ZERO when in BACKUP Mode. 
        ///     ''' A write to this object shall reset the Backup timer to ZERO (see unitBackupTime).
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public byte SystemSyncControl
        {
            get
            {
                return m_SystemSyncControl;
            }
            set
            {
                m_SystemSyncControl = value;
            }
        }
    }

}
