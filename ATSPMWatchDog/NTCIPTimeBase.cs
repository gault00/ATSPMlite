using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSPMWatchDog
{
    public class NTCIPTimeBase : NTCIPBase
    {
        // PROPERTIES------------------------------------------------------------------------------------

        private byte m_Number;
        /// <summary>
        ///     ''' The time base Action number for objects in this row. This value shall not 
        ///     ''' exceed the maxTimebaseAscActions object value. This object may be defined 
        ///     ''' as a dayPlanActionOID (as defined in NTCIP 1201).
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

        private byte m_Pattern;
        /// <summary>
        ///     ''' The Pattern that shall be active when this Action is active. The value shall not 
        ///     ''' exceed the value of maxPatterns, except for flash or free. A pattern of zero 
        ///     ''' indicates that no pattern is being selected. A pattern = 0 relinquishes control 
        ///     ''' to entity of a lower priority than timebase and allows that entity to control 
        ///     ''' (i.e., interconnect if available).
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public byte Pattern
        {
            get
            {
                return m_Pattern;
            }
            set
            {
                m_Pattern = value;
            }
        }

        private byte m_AuxiliaryFunction;
        /// <summary>
        ///     ''' The Auxiliary functions that shall be active when this Action is active.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public byte AuxiliaryFunction
        {
            get
            {
                m_AuxiliaryFunction = CreateBit8(AuxFunction1, AuxFunction2, AuxFunction3, Dimming, false, false, false, false);
                return m_AuxiliaryFunction;
            }
            set
            {
                m_AuxiliaryFunction = value;
                Dimming = GetBit8(value, 3);
                AuxFunction3 = GetBit8(value, 2);
                AuxFunction2 = GetBit8(value, 1);
                AuxFunction1 = GetBit8(value, 0);
            }
        }

        private bool m_Dimming;
        /// <summary>
        ///     ''' Dimming enabled if set (non-zero), disabled if clear (zero). For dimming to occur, this control AND ('unitControl' OR a dimming input) must be True.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public bool Dimming
        {
            get
            {
                return m_Dimming;
            }
            set
            {
                m_Dimming = value;
            }
        }

        private bool m_AuxFunction3;
        /// <summary>
        ///     ''' Auxiliary Function 3 enabled if set (non-zero), disabled if clear (zero).
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public bool AuxFunction3
        {
            get
            {
                return m_AuxFunction3;
            }
            set
            {
                m_AuxFunction3 = value;
            }
        }

        private bool m_AuxFunction2;
        /// <summary>
        ///     ''' Auxiliary Function 2 enabled if set (non-zero), disabled if clear (zero).
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public bool AuxFunction2
        {
            get
            {
                return m_AuxFunction2;
            }
            set
            {
                m_AuxFunction2 = value;
            }
        }

        private bool m_AuxFunction1;
        /// <summary>
        ///     ''' Auxiliary Function 1 enabled if set (non-zero), disabled if clear (zero).
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public bool AuxFunction1
        {
            get
            {
                return m_AuxFunction1;
            }
            set
            {
                m_AuxFunction1 = value;
            }
        }

        // need to fully implement individual bits
        private byte m_SpecialFunction;
        /// <summary>
        ///     ''' The Special Functions that shall be active when this Action is active.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public byte SpecialFunction
        {
            get
            {
                return m_SpecialFunction;
            }
            set
            {
                m_SpecialFunction = value;
            }
        }
    }
}
