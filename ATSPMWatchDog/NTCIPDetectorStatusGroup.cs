using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSPMWatchDog
{
    public class NTCIPDetectorStatusGroup
    {
        private byte m_Number;
        /// <summary>
        ///     ''' The detector status group number for objects in this row. 
        ///     ''' This value shall not exceed the maxVehicleDetectorStatusGroups object value.
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

        private byte m_StatusActive;
        /// <summary>
        ///     ''' This object shall return the detection status of each detector associated with the group.
        ///     ''' Each detector shall be represented as ON (detect) or OFF (no-detect) by individual bits in this object.
        ///     ''' If a detector is ON then the associated bit shall be set (1). 
        ///     ''' If a detector is OFF then the associated bit shall be clear (0).
        ///     ''' Bit 7: Det # = ( vehicleDetectorStatusGroupNumber * 8)
        ///     ''' Bit 6: Det # = ( vehicleDetectorStatusGroupNumber * 8) - 1
        ///     ''' Bit 5: Det # = ( vehicleDetectorStatusGroupNumber * 8) - 2
        ///     ''' Bit 4: Det # = ( vehicleDetectorStatusGroupNumber * 8) - 3
        ///     ''' Bit 3: Det # = ( vehicleDetectorStatusGroupNumber * 8) - 4
        ///     ''' Bit 2: Det # = ( vehicleDetectorStatusGroupNumber * 8) - 5
        ///     ''' Bit 1: Det # = ( vehicleDetectorStatusGroupNumber * 8) - 6
        ///     ''' Bit 0: Det # = ( vehicleDetectorStatusGroupNumber * 8) - 7
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public byte StatusActive
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

        private byte m_AlarmStatus;
        /// <summary>
        ///     ''' This object shall return the alarm status of the detectors associated with the group. 
        ///     ''' Each detector alarm status shall be represented as ON or OFF by individual bits in this object. 
        ///     ''' If any detector alarm (defined in the vehicleDetectorAlarm object) is active the associated bit shall be set (1). 
        ///     ''' If a detector alarm is not active the associated bit shall be clear (0).
        ///     ''' Bit 7: Det # = ( vehicleDetectorStatusGroupNumber * 8)
        ///     ''' Bit 6: Det # = ( vehicleDetectorStatusGroupNumber * 8) - 1
        ///     ''' Bit 5: Det # = ( vehicleDetectorStatusGroupNumber * 8) - 2
        ///     ''' Bit 4: Det # = ( vehicleDetectorStatusGroupNumber * 8) - 3
        ///     ''' Bit 3: Det # = ( vehicleDetectorStatusGroupNumber * 8) - 4
        ///     ''' Bit 2: Det # = ( vehicleDetectorStatusGroupNumber * 8) - 5
        ///     ''' Bit 1: Det # = ( vehicleDetectorStatusGroupNumber * 8) - 6
        ///     ''' Bit 0: Det # = ( vehicleDetectorStatusGroupNumber * 8) - 7
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public byte AlarmStatus
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
    }

}
