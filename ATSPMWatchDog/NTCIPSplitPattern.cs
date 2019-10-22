using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSPMWatchDog
{
    public class NTCIPSplitPattern
    {
        public enum splitMode
        {
            other = 1,
            none = 2,
            minimumVehicleRecall = 3,
            maximumVehicleRecall = 4,
            pedestrianRecall = 5,
            maximumVehicleAndPedestrianRecall = 6,
            phaseOmitted = 7
        }

        // PROPERTIES------------------------------------------------------------------------------------
        private byte m_Number;
        /// <summary>
        ///     ''' The object defines which rows of the split table comprise a split group. 
        ///     ''' All rows that have the same splitNumber are in the same split group. 
        ///     ''' The value of this object shall not exceed the maxSplits object value.
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

        private byte m_SplitPhase;
        /// <summary>
        ///     ''' The phase number for objects in this row. 
        ///     ''' The value of this object shall not exceed the maxPhases object value.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public byte SplitPhase
        {
            get
            {
                return m_SplitPhase;
            }
            set
            {
                m_SplitPhase = value;
            }
        }

        private byte m_SplitTime;
        /// <summary>
        ///     ''' The time in seconds the splitPhase is allowed to receive (i.e. before a Force Off is applied) when constant demands exist on all phases.
        ///     ''' In floating coordForceMode, this is always the maximum time a non-coordinated phase is allowed to receive. 
        ///     ''' In fixed coordForceMode, the actual allowed time may be longer if a previous phase gapped out.
        ///     ''' The splitTime includes all phase clearance times for the associated phase. 
        ///     ''' The split time shall be longer than the sum of the phase minimum service requirements for the phase. 
        ///     ''' When the time is NOT adequate to service the minimum service requirements of the phase, Free Mode shall be the result. 
        ///     ''' The minimum requirements of a phase with a not-actuated ped include Minimum Green, Walk, Pedestrian Clear, Yellow Clearance, and Red Clearance; 
        ///     ''' the minimum requirements of a phase with an actuated pedestrian include Minimum Green, Yellow Clearance, and Red Clearance.
        ///     ''' 
        ///     ''' If the cycleTime entry of the associated patternTable entry is zero (i.e. the device is in Free Mode), 
        ///     ''' then the value of this object shall be applied, if non-zero, as a maximum time for the associated phase.
        ///     ''' 
        ///     ''' If the critical path through the phase diagram is less than the cycleTime entry of the associated patternTable entry, 
        ///     ''' all extra time is alloted to the coordination phase in each ring.
        ///     ''' 
        ///     ''' If the critical path through the phase diagram is greater than the cycleTime entry of the associated patternTable entry 
        ///     ''' (and the cycleTime is not zero) the device shall operate in the Free Mode.
        ///     ''' While the Free Mode condition exists, the Local Override bit of shortAlarm shall be set to one (1).
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public byte SplitTime
        {
            get
            {
                return m_SplitTime;
            }
            set
            {
                m_SplitTime = value;
            }
        }

        private splitMode m_Mode;
        /// <summary>
        ///     ''' This object defines operational characteristics of the phase. The following options are available:
        ///     ''' other: the operation is not specified in this standard
        ///     ''' none: no split mode control.
        ///     ''' minimumVehicleRecall: this phase operates with a minimum vehicle recall.
        ///     ''' maximumVehicleRecall: this phase operates with a maximum vehicle recall.
        ///     ''' pedestrianRecall: this phase operates with a pedestrian recall.
        ///     ''' maximumVehicleAndPedestrianRecall: this phase operates with a maximum vehicle and pedestrian recall.
        ///     ''' phaseOmitted: this phase is omitted.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public splitMode Mode
        {
            get
            {
                return m_Mode;
            }
            set
            {
                m_Mode = value;
            }
        }

        private bool m_CoordPhase;
        /// <summary>
        ///     ''' To select the associated phase as a coordinated phase this object shall be set to TRUE (non zero).
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public bool CoordPhase
        {
            get
            {
                return m_CoordPhase;
            }
            set
            {
                m_CoordPhase = value;
            }
        }



        // constructor
        public NTCIPSplitPattern()
        {
            Number = 0;
            SplitPhase = 0;
            SplitTime = 0;
            CoordPhase = false;
            Mode = splitMode.none;
        }
    }

}
