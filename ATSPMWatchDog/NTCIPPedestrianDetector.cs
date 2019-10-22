using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSPMWatchDog
{
    /// <summary>
    /// ''' A table containing Actuated Controller Unit pedestrian detector parameters. The number of rows in this table is equal to the maxPedestrianDetectors object.
    /// ''' </summary>
    /// ''' <remarks></remarks>
    public class NTCIPPedestrianDetector
    {
        private byte m_Number;
        /// <summary>
        ///     ''' The pedestrianDetector number for objects in this row. The value shall not exceed the maxPedestrianDetectors object value.
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

        private byte m_CallPhase;
        /// <summary>
        ///     ''' This object contains assigned phase number for the pedestrian detector input associated with 
        ///     ''' this row. The associated detector call capability is enabled when this object is set to a 
        ///     ''' non-zero value. The value shall not exceed the value of maxPhases.
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
            }
        }

        private byte m_NoActivity;
        /// <summary>
        ///     ''' Pedestrian Detector No Activity diagnostic Parameter in minutes (0–255 min.) . If an 
        ///     ''' active detector does not exhibit an actuation in the specified period, it is considered 
        ///     ''' a fault by the diagnostics and the detector is classified as Failed. A value of 0 for 
        ///     ''' this object shall disable this diagnostic for this detector.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public byte NoActivity
        {
            get
            {
                return m_NoActivity;
            }
            set
            {
                m_NoActivity = value;
            }
        }

        private byte m_MaximumPresence;
        /// <summary>
        ///     ''' Pedestrian Detector Maximum Presence diagnostic Parameter in minutes (0-255 min.). If an
        ///     ''' active detector exhibits continuous detection for too long a period, it is considered a 
        ///     ''' fault by the diagnostics and the detector is classified as Failed. A value of 0 for this 
        ///     ''' object shall disable this diagnostic for this detector.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public byte MaximumPresence
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

        private byte m_ErraticCounts;
        /// <summary>
        ///     ''' Pedestrian Detector Erratic Counts diagnostic Parameter in counts/minute (0-255 cpm). If
        ///     ''' an active detector exhibits excessive actuations, it is considered a fault by the 
        ///     ''' diagnostics and the detector is classified as Failed. A value of 0 for this object shall 
        ///     ''' disable this diagnostic for this detector.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public byte ErraticCounts
        {
            get
            {
                return m_ErraticCounts;
            }
            set
            {
                m_ErraticCounts = value;
            }
        }

        private byte m_Alarms;
        /// <summary>
        ///     ''' This object shall return indications of detector alarms. Detector Alarms are indicated as
        ///     ''' individual bits of an 8-bit integer.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public byte Alarms
        {
            get
            {
                // ADD CODE TO CREATE INTEGER FROM INDIVIDUAL BOOLEAN VALUES
                return m_Alarms;
            }
            set
            {
                m_Alarms = value;

                string tempstring;
                byte tempbit;
                tempstring = Convert.ToString(value, 2).PadLeft(8, '0');

                // bit 7 = index 0 of string
                tempbit = System.Convert.ToByte(tempstring.Substring(0, 1));
                if (tempbit == 1)
                    AlarmOtherFault = true;
                else
                    AlarmOtherFault = false;

                // bits 6 & 5 reserved

                // bit 4 = index 3 of string
                tempbit = System.Convert.ToByte(tempstring.Substring(3, 1));
                if (tempbit == 1)
                    AlarmConfigurationFault = true;
                else
                    AlarmConfigurationFault = false;

                // bit 3 = index 4 of string
                tempbit = System.Convert.ToByte(tempstring.Substring(4, 1));
                if (tempbit == 1)
                    AlarmCommunicationFault = true;
                else
                    AlarmCommunicationFault = false;

                // bit 2 = index 5 of string
                tempbit = System.Convert.ToByte(tempstring.Substring(5, 1));
                if (tempbit == 1)
                    AlarmErraticOutputFault = true;
                else
                    AlarmErraticOutputFault = false;

                // bit 1 = index 6 of string
                tempbit = System.Convert.ToByte(tempstring.Substring(6, 1));
                if (tempbit == 1)
                    AlarmMaxPresenceFault = true;
                else
                    AlarmMaxPresenceFault = false;

                // bit 0 = index 7 of string
                tempbit = System.Convert.ToByte(tempstring.Substring(7, 1));
                if (tempbit == 1)
                    AlarmNoActivityFault = true;
                else
                    AlarmNoActivityFault = false;
            }
        }

        private bool m_AlarmOtherFault;
        /// <summary>
        ///     ''' The detector has failed due to some other cause.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public bool AlarmOtherFault
        {
            get
            {
                return m_AlarmOtherFault;
            }
            set
            {
                m_AlarmOtherFault = value;
            }
        }

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
        ///     ''' This detector has been flagged as non-operational due to erratic 
        ///     ''' outputs (excessive counts) by the CU detector diagnostic.
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
        ///     ''' This detector has been flagged as non-operational due to a presence 
        ///     ''' indicator that exceeded the maximum expected time by the CU detector diagnostic.
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
        ///     ''' This detector has been flagged as non-operational due to lower 
        ///     ''' than expected activity by the CU detector diagnostic
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
    }

}
