using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSPMWatchDog
{
    public class NTCIPPhaseStatusGroup
    {
        private byte m_Number;
        /// <summary>
        ///     ''' The Phase StatusGroup number for objects in this row. This value shall not exceed the maxPhaseGroups object value.
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

        private byte m_Reds;
        /// <summary>
        ///     ''' Phase Red Output Status Mask, when a bit = 1, the Phase Red is currently active. When a bit = 0, the Phase Red is NOT currently active.
        ///     ''' Bit 7: Phase # = (phaseStatusGroupNumber * 8)
        ///     ''' Bit 6: Phase # = (phaseStatusGroupNumber * 8) - 1
        ///     ''' Bit 5: Phase # = (phaseStatusGroupNumber * 8) - 2
        ///     ''' Bit 4: Phase # = (phaseStatusGroupNumber * 8) - 3
        ///     ''' Bit 3: Phase # = (phaseStatusGroupNumber * 8) - 4
        ///     ''' Bit 2: Phase # = (phaseStatusGroupNumber * 8) - 5
        ///     ''' Bit 1: Phase # = (phaseStatusGroupNumber * 8) - 6
        ///     ''' Bit 0: Phase # = (phaseStatusGroupNumber * 8) - 7
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public byte Reds
        {
            get
            {
                return m_Reds;
            }
            set
            {
                m_Reds = value;
            }
        }

        private byte m_Yellows;
        /// <summary>
        ///     ''' Phase Yellow Output Status Mask, when a bit = 1, the Phase Yellow is currently active. When a bit = 0, the Phase Yellow is NOT currently active.
        ///     ''' Bit 7: Phase # = (phaseStatusGroupNumber * 8)
        ///     ''' Bit 6: Phase # = (phaseStatusGroupNumber * 8) - 1
        ///     ''' Bit 5: Phase # = (phaseStatusGroupNumber * 8) - 2
        ///     ''' Bit 4: Phase # = (phaseStatusGroupNumber * 8) - 3
        ///     ''' Bit 3: Phase # = (phaseStatusGroupNumber * 8) - 4
        ///     ''' Bit 2: Phase # = (phaseStatusGroupNumber * 8) - 5
        ///     ''' Bit 1: Phase # = (phaseStatusGroupNumber * 8) - 6
        ///     ''' Bit 0: Phase # = (phaseStatusGroupNumber * 8) - 7
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public byte Yellows
        {
            get
            {
                return m_Yellows;
            }
            set
            {
                m_Yellows = value;
            }
        }

        private byte m_Greens;
        /// <summary>
        ///     ''' Phase Green Output Status Mask, when a bit = 1, the Phase Green is currently active. When a bit = 0, the Phase Green is NOT currently active.
        ///     ''' Bit 7: Phase # = (phaseStatusGroupNumber * 8)
        ///     ''' Bit 6: Phase # = (phaseStatusGroupNumber * 8) - 1
        ///     ''' Bit 5: Phase # = (phaseStatusGroupNumber * 8) - 2
        ///     ''' Bit 4: Phase # = (phaseStatusGroupNumber * 8) - 3
        ///     ''' Bit 3: Phase # = (phaseStatusGroupNumber * 8) - 4
        ///     ''' Bit 2: Phase # = (phaseStatusGroupNumber * 8) - 5
        ///     ''' Bit 1: Phase # = (phaseStatusGroupNumber * 8) - 6
        ///     ''' Bit 0: Phase # = (phaseStatusGroupNumber * 8) - 7
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public byte Greens
        {
            get
            {
                return m_Greens;
            }
            set
            {
                m_Greens = value;
            }
        }

        private byte m_DontWalks;
        /// <summary>
        ///     ''' Phase Dont Walk Output Status Mask, when a bit = 1, the Phase Dont Walk is currently active. When a bit = 0, the Phase Dont Walk is NOT currently active.
        ///     ''' Bit 7: Phase # = (phaseStatusGroupNumber * 8)
        ///     ''' Bit 6: Phase # = (phaseStatusGroupNumber * 8) - 1
        ///     ''' Bit 5: Phase # = (phaseStatusGroupNumber * 8) - 2
        ///     ''' Bit 4: Phase # = (phaseStatusGroupNumber * 8) - 3
        ///     ''' Bit 3: Phase # = (phaseStatusGroupNumber * 8) - 4
        ///     ''' Bit 2: Phase # = (phaseStatusGroupNumber * 8) - 5
        ///     ''' Bit 1: Phase # = (phaseStatusGroupNumber * 8) - 6
        ///     ''' Bit 0: Phase # = (phaseStatusGroupNumber * 8) - 7
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public byte DontWalks
        {
            get
            {
                return m_DontWalks;
            }
            set
            {
                m_DontWalks = value;
            }
        }

        private byte m_VehicleCalls;
        /// <summary>
        ///     ''' Phase Vehicle Call Status Mask, when a bit = 1, the Phase vehicle currently has a call for service. 
        ///     ''' When a bit = 0, the Phase vehicle currently does NOT have a call for service.
        ///     ''' Bit 7: Phase # = (phaseStatusGroupNumber * 8)
        ///     ''' Bit 6: Phase # = (phaseStatusGroupNumber * 8) - 1
        ///     ''' Bit 5: Phase # = (phaseStatusGroupNumber * 8) - 2
        ///     ''' Bit 4: Phase # = (phaseStatusGroupNumber * 8) - 3
        ///     ''' Bit 3: Phase # = (phaseStatusGroupNumber * 8) - 4
        ///     ''' Bit 2: Phase # = (phaseStatusGroupNumber * 8) - 5
        ///     ''' Bit 1: Phase # = (phaseStatusGroupNumber * 8) - 6
        ///     ''' Bit 0: Phase # = (phaseStatusGroupNumber * 8) - 7
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public byte VehicleCalls
        {
            get
            {
                return m_VehicleCalls;
            }
            set
            {
                m_VehicleCalls = value;
            }
        }

        private byte m_PedestrianClears;
        /// <summary>
        ///     ''' Phase Ped Clear Output Status Mask, when a bit = 1, the Phase Ped Clear is currently active. When a bit = 0, the Phase Ped Clear is NOT currently active.
        ///     ''' Bit 7: Phase # = (phaseStatusGroupNumber * 8)
        ///     ''' Bit 6: Phase # = (phaseStatusGroupNumber * 8) - 1
        ///     ''' Bit 5: Phase # = (phaseStatusGroupNumber * 8) - 2
        ///     ''' Bit 4: Phase # = (phaseStatusGroupNumber * 8) - 3
        ///     ''' Bit 3: Phase # = (phaseStatusGroupNumber * 8) - 4
        ///     ''' Bit 2: Phase # = (phaseStatusGroupNumber * 8) - 5
        ///     ''' Bit 1: Phase # = (phaseStatusGroupNumber * 8) - 6
        ///     ''' Bit 0: Phase # = (phaseStatusGroupNumber * 8) - 7
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public byte PedestrianClears
        {
            get
            {
                return m_PedestrianClears;
            }
            set
            {
                m_PedestrianClears = value;
            }
        }

        private byte m_Walks;
        /// <summary>
        ///     ''' Phase Walk Output Status Mask, when a bit = 1, the Phase Walk is currently active. When a bit = 0, the Phase Walk is NOT currently active.
        ///     ''' Bit 7: Phase # = (phaseStatusGroupNumber * 8)
        ///     ''' Bit 6: Phase # = (phaseStatusGroupNumber * 8) - 1
        ///     ''' Bit 5: Phase # = (phaseStatusGroupNumber * 8) - 2
        ///     ''' Bit 4: Phase # = (phaseStatusGroupNumber * 8) - 3
        ///     ''' Bit 3: Phase # = (phaseStatusGroupNumber * 8) - 4
        ///     ''' Bit 2: Phase # = (phaseStatusGroupNumber * 8) - 5
        ///     ''' Bit 1: Phase # = (phaseStatusGroupNumber * 8) - 6
        ///     ''' Bit 0: Phase # = (phaseStatusGroupNumber * 8) - 7
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public byte Walks
        {
            get
            {
                return m_Walks;
            }
            set
            {
                m_Walks = value;
            }
        }

        private byte m_PedestrianCalls;
        /// <summary>
        ///     ''' Phase Pedestrian Call Status Mask, when a bit = 1, the Phase pedestrian currently has a call for service. When a bit = 0, the Phase pedestrian currently does NOT have a call for service.
        ///     ''' Bit 7: Phase # = (phaseStatusGroupNumber * 8)
        ///     ''' Bit 6: Phase # = (phaseStatusGroupNumber * 8) - 1
        ///     ''' Bit 5: Phase # = (phaseStatusGroupNumber * 8) - 2
        ///     ''' Bit 4: Phase # = (phaseStatusGroupNumber * 8) - 3
        ///     ''' Bit 3: Phase # = (phaseStatusGroupNumber * 8) - 4
        ///     ''' Bit 2: Phase # = (phaseStatusGroupNumber * 8) - 5
        ///     ''' Bit 1: Phase # = (phaseStatusGroupNumber * 8) - 6
        ///     ''' Bit 0: Phase # = (phaseStatusGroupNumber * 8) - 7
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public byte PedestrianCalls
        {
            get
            {
                return m_PedestrianCalls;
            }
            set
            {
                m_PedestrianCalls = value;
            }
        }

        private byte m_PhaseOns;
        /// <summary>
        ///     ''' Phase On Status Mask, when a bit = 1, the Phase is currently active. 
        ///     ''' When a bit = 0, the Phase currently is NOT active. 
        ///     ''' The phase is ON during the Green, Yellow, and Red Clearance intervals of that phase. 
        ///     ''' It shall be permissible for this status to be True (bit=1) during the Red Dwell state.
        ///     ''' Bit 7: Phase # = (phaseStatusGroupNumber * 8)
        ///     ''' Bit 6: Phase # = (phaseStatusGroupNumber * 8) - 1
        ///     ''' Bit 5: Phase # = (phaseStatusGroupNumber * 8) - 2
        ///     ''' Bit 4: Phase # = (phaseStatusGroupNumber * 8) - 3
        ///     ''' Bit 3: Phase # = (phaseStatusGroupNumber * 8) - 4
        ///     ''' Bit 2: Phase # = (phaseStatusGroupNumber * 8) - 5
        ///     ''' Bit 1: Phase # = (phaseStatusGroupNumber * 8) - 6
        ///     ''' Bit 0: Phase # = (phaseStatusGroupNumber * 8) - 7
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public byte PhaseOns
        {
            get
            {
                return m_PhaseOns;
            }
            set
            {
                m_PhaseOns = value;
            }
        }

        private byte m_PhaseNexts;
        /// <summary>
        ///     ''' Phase Next Status Mask, when a bit = 1, the Phase currently is committed to be NEXT 
        ///     ''' in sequence and remains present until the phase becomes active (On/Timing). 
        ///     ''' When a bit = 0, the Phase currently is NOT committed to be NEXT in sequence.
        ///     ''' The phase next to be serviced shall be determined at the end of the green interval of the terminating phase;
        ///     ''' except that if the decision cannot be made at the end of the Green interval, 
        ///     ''' it shall not be made until after the end of all Vehicle Change and Clearance intervals.
        ///     ''' Bit 7: Phase # = (phaseStatusGroupNumber * 8)
        ///     ''' Bit 6: Phase # = (phaseStatusGroupNumber * 8) - 1
        ///     ''' Bit 5: Phase # = (phaseStatusGroupNumber * 8) - 2
        ///     ''' Bit 4: Phase # = (phaseStatusGroupNumber * 8) - 3
        ///     ''' Bit 3: Phase # = (phaseStatusGroupNumber * 8) - 4
        ///     ''' Bit 2: Phase # = (phaseStatusGroupNumber * 8) - 5
        ///     ''' Bit 1: Phase # = (phaseStatusGroupNumber * 8) - 6
        ///     ''' Bit 0: Phase # = (phaseStatusGroupNumber * 8) - 7
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public byte PhaseNexts
        {
            get
            {
                return m_PhaseNexts;
            }
            set
            {
                m_PhaseNexts = value;
            }
        }
    }

}
