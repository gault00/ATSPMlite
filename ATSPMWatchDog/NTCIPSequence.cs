using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSPMWatchDog
{
    public class NTCIPSequence
    {
        // PROPERTIES------------------------------------------------------------------------------------

        private byte m_SequenceNumber;
        /// <summary>
        ///     ''' This number identifies a sequence plan.
        ///     ''' Each row of the table contains the phase sequence for a ring. 
        ///     ''' A sequence plan shall consist of one row for each ring that defines the phase sequences for that ring.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public byte Number
        {
            get
            {
                return m_SequenceNumber;
            }
            set
            {
                m_SequenceNumber = value;
            }
        }

        private byte m_RingNumber;
        /// <summary>
        ///     ''' This number identifies the ring number this phase sequence applies to.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public byte RingNumber
        {
            get
            {
                return m_RingNumber;
            }
            set
            {
                m_RingNumber = value;
            }
        }

        private string m_SequenceData;
        /// <summary>
        ///     ''' Each octet is a Phase Number (binary value) within the associated ring 
        ///     ''' number. The phase number value shall not exceed the maxPhases object
        ///     ''' value. The order of phase numbers determines the phase sequence for the 
        ///     ''' ring. The phase numbers shall not be ordered in a manner that would violate
        ///     ''' the Consistency Checks defined in Annex B of NTCIP 1202.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public string SequenceData
        {
            get
            {
                return m_SequenceData;
            }
            set
            {
                m_SequenceData = value;
            }
        }
    }

}
