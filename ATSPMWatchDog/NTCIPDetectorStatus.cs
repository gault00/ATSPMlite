using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSPMWatchDog
{
    public class NTCIPDetectorStatus
    {
        // PROPERTIES------------------------------------------------------------------------------------

        private int m_Number;
        /// <summary>
        ///     ''' The detector status group number for objects in this row. 
        ///     ''' This value shall not exceed the maxVehicleDetectorStatusGroups object value.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public int Number
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

        private bool m_StatusActive;
        /// <summary>
        ///     ''' This object shall return the detection status of each detector associated with the group.
        ///     ''' Each detector shall be represented as ON (detect) or OFF (no-detect) by individual bits in this object.
        ///     ''' If a detector is ON then the associated bit shall be set (1). 
        ///     ''' If a detector is OFF then the associated bit shall be clear (0).
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public bool StatusActive
        {
            get
            {
                return m_StatusActive;
            }
            set
            {
                m_StatusActive = value;
            }
        }

        private bool m_AlarmStatus;
        /// <summary>
        ///     ''' This object shall return the alarm status of the detectors associated with the group. 
        ///     ''' Each detector alarm status shall be represented as ON or OFF by individual bits in this object. 
        ///     ''' If any detector alarm (defined in the vehicleDetectorAlarm object) is active the associated bit shall be set (1). 
        ///     ''' If a detector alarm is not active the associated bit shall be clear (0).
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public bool AlarmStatus
        {
            get
            {
                return m_AlarmStatus;
            }
            set
            {
                m_AlarmStatus = value;
            }
        }

        public NTCIPDetectorStatus()
        {
            // DetectorOnTime = #12:00:00 AM#
            // For d = 1 To 64
            StatusActive = false;
            AlarmStatus = false;
        }
    }

}
