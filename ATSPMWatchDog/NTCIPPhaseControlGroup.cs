using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSPMWatchDog
{
    public class NTCIPPhaseControlGroup
    {
        private byte m_Number;
        /// <summary>
        ///     ''' The Phase Control Group number for objects in this row. This value shall not exceed the maxPhaseGroups object value.
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

        private byte m_PhaseOmit;
        /// <summary>
        ///     ''' This object is used to allow a remote entity to omit phases from being serviced in the device. 
        ///     ''' When a bit = 1, the device shall activate the System Phase Omit control for that phase. 
        ///     ''' When a bit = 0, the device shall not activate the System Phase Omit control for that phase.
        ///     ''' The device shall reset this object to ZERO when in BACKUP Mode. 
        ///     ''' A write to this object shall reset the Backup timer to ZERO (see unitBackupTime).
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
        public byte PhaseOmit
        {
            get
            {
                return m_PhaseOmit;
            }
            set
            {
                m_PhaseOmit = value;
            }
        }

        private byte m_PedestrianOmit;
        /// <summary>
        ///     ''' This object is used to allow a remote entity to omit peds from being serviced in the device. 
        ///     ''' When a bit = 1, the device shall activate the System Ped Omit control for that phase. 
        ///     ''' When a bit = 0, the device shall not activate the System Ped Omit control for that phase.
        ///     ''' The device shall reset this object to ZERO when in BACKUP Mode. 
        ///     ''' A write to this object shall reset the Backup timer to ZERO (see unitBackupTime).
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
        public byte PedestrianOmit
        {
            get
            {
                return m_PedestrianOmit;
            }
            set
            {
                m_PedestrianOmit = value;
            }
        }

        private byte m_Hold;
        /// <summary>
        ///     ''' This object is used to allow a remote entity to hold phases in the device. 
        ///     ''' When a bit = 1, the device shall activate the System Phase Hold control for that phase. 
        ///     ''' When a bit = 0, the device shall not activate the System Phase Hold control for that phase.
        ///     ''' The device shall reset this object to ZERO when in BACKUP Mode. 
        ///     ''' A write to this object shall reset the Backup timer to ZERO (see unitBackupTime).
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
        public byte Hold
        {
            get
            {
                return m_Hold;
            }
            set
            {
                m_Hold = value;
            }
        }

        private byte m_ForceOff;
        /// <summary>
        ///     ''' This object is used to apply force offs on a per phase basis. 
        ///     ''' When a bit = 1, the device shall activate the System Phase Force Off control for that phase. 
        ///     ''' When a bit = 0, the device shall not activate the System Phase Force Off control for that phase. 
        ///     ''' When the phase green terminates, the associated bit shall be reset to 0.
        ///     ''' The device shall reset this object to ZERO when in BACKUP Mode. 
        ///     ''' A write to this object shall reset the Backup timer to ZERO (see unitBackupTime).
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
        public byte ForceOff
        {
            get
            {
                return m_ForceOff;
            }
            set
            {
                m_ForceOff = value;
            }
        }

        private byte m_VehicleCall;
        /// <summary>
        ///     ''' This object is used to allow a remote entity to place calls for vehicle service in the device. 
        ///     ''' When a bit = 1, the device shall place a call for vehicle service on that phase. 
        ///     ''' When a bit = 0, the device shall not place a call for vehicle service on that phase.
        ///     ''' The device shall reset this object to ZERO when in BACKUP Mode.
        ///     ''' A write to this object shall reset the Backup timer to ZERO (see unitBackupTime).
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
        public byte VehicleCall
        {
            get
            {
                return m_VehicleCall;
            }
            set
            {
                m_VehicleCall = value;
            }
        }

        private byte m_PedestrianCall;
        /// <summary>
        ///     ''' This object is used to allow a remote entity to place calls for ped service in the device.
        ///     ''' When a bit = 1, the device shall place a call for ped service on that phase. 
        ///     ''' When a bit = 0, the device shall not place a call for ped service on that phase.
        ///     ''' The device shall reset this object to ZERO when in BACKUP Mode. 
        ///     ''' A write to this object shall reset the Backup timer to ZERO (see unitBackupTime).
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
        public byte PedestrianCall
        {
            get
            {
                return m_PedestrianCall;
            }
            set
            {
                m_PedestrianCall = value;
            }
        }
    }

}
