using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSPMWatchDog
{
    public class NTCIPChannelStatus : NTCIPBase
    {
        private byte m_Number;
        /// <summary>
        ///     ''' The channelStatusGroup number for objects in this row. This value shall not exceed the maxChannelStatusGroups object value.
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

        private bool m_Red;
        /// <summary>
        ///     ''' Channel Red Output Status Mask, when a bit=1, the Channel Red is currently active. 
        ///     ''' When abit=0, the Channel Red is NOT currently active.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public bool Red
        {
            get
            {
                return m_Red;
            }
            set
            {
                m_Red = value;
            }
        }

        private bool m_Yellow;
        /// <summary>
        ///     ''' Channel Yellow Output Status Mask, when a bit=1, the Channel Yellow is currently active. 
        ///     ''' When a bit=0, the Channel Yellow is NOT currently active.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public bool Yellow
        {
            get
            {
                return m_Yellow;
            }
            set
            {
                m_Yellow = value;
            }
        }

        private bool m_Green;
        /// <summary>
        ///     ''' Channel Green Output Status Mask, when a bit=1, the Channel Green is currently active. 
        ///     ''' When a bit=0, the Channel Green is NOT currently active.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public bool Green
        {
            get
            {
                return m_Green;
            }
            set
            {
                m_Green = value;
            }
        }
    }

}
