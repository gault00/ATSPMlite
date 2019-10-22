using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSPMWatchDog
{
    public class NTCIPOverlap
    {
        public enum overlapTypes
        {
            other = 1,
            normal = 2,
            minusGreenYellow = 3
        }

        // PROPERTIES------------------------------------------------------------------------------------
        private byte m_Number;
        /// <summary>
        ///     ''' The overlap number for objects in this row. The value shall not exceed the maxOverlaps object.
        ///     ''' The value maps to the Overlap as follows: 1 = Overlap A, 2 = Overlap B etc.
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

        private overlapTypes m_Type;
        /// <summary>
        ///     ''' The type of overlap operation for this row. The types are as follows:
        ///     ''' 
        ///     ''' other: The overlap operates in another mode than those described herein.
        ///     ''' 
        ///     ''' normal: The overlap output shall be controlled by the overlapIncludedPhases 
        ///     ''' when this type is indicated. The overlap output shall be green in the following situations:
        ///     ''' (1) when an overlap included phase is green.
        ///     ''' (2) when an overlap included phase is yellow (or red clearance) and an overlap included phase is next.
        ///     ''' The overlap output shall be yellow when an included phase is yellow and an overlap included phase is not next. 
        ///     ''' The overlap output shall be red whenever the overlap green and yellow are not ON.
        ///     ''' 
        ///     ''' minusGreenYellow: The overlap output shall be controlled by the overlapIncludedPhases 
        ///     ''' and the overlapModifierPhases if this type is indicated.
        ///     ''' The overlap output shall be green in the following situations:
        ///     ''' (1) when an overlap included phase is green and an overlap modifier phase is NOT green.
        ///     ''' (2) when an overlap included phase is yellow (or red clearance) and an overlap included phase is next and an overlap modifier phase is NOT green
        ///     ''' The overlap output shall be yellow when an overlap included phase is yellow and an overlap modifier phase is NOT yellow 
        ///     ''' and an overlap included phase is not next. 
        ///     ''' The overlap output shall be red whenever the overlap green andyellow are not ON.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public overlapTypes Type
        {
            get
            {
                return m_Type;
            }
            set
            {
                m_Type = value;
            }
        }

        private string m_IncludedPhase;
        /// <summary>
        ///     ''' Each octet is a Phase (number) that shall be an included phase for the overlap. 
        ///     ''' The phase number value shall not exceed the maxPhases object value. 
        ///     ''' When an included phase output is green or when the CU is cycling between included phases,
        ///     ''' the overlap output shall be green.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public string IncludedPhase
        {
            get
            {
                return m_IncludedPhase;
            }
            set
            {
                m_IncludedPhase = value;
            }
        }

        private string m_ModifierPhase;
        /// <summary>
        ///     ''' Each octet is a Phase (number) that shall be a modifier phase for the overlap. 
        ///     ''' The phase number value shall not exceed the maxPhases object value.
        ///     ''' A null value provides a normal overlap type. A non-null value provides a minusGreenYellow overlap type.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public string ModifierPhase
        {
            get
            {
                return m_ModifierPhase;
            }
            set
            {
                m_ModifierPhase = value;
            }
        }

        private byte m_TrailingGreen;
        /// <summary>
        ///     ''' Overlap Trailing Green Parameter in seconds (0-255 sec). When this value is greater than zero and the overlap green would normally terminate,
        ///     ''' the overlap green shall be extended by this additional time.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public byte TrailingGreen
        {
            get
            {
                return m_TrailingGreen;
            }
            set
            {
                m_TrailingGreen = value;
            }
        }

        private double m_TrailingYellowChange;
        /// <summary>
        ///     ''' Overlap Trailing Yellow Change Parameter in tenth seconds (NEMA range: 3.0-25.5 sec). 
        ///     ''' When the overlap green has been extended (Trailing Green), this value shall determine the current length of the Yellow Change interval for the overlap.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public double TrailingYellowChange
        {
            get
            {
                return m_TrailingYellowChange / 10.0;
        }
            set
            {
                m_TrailingYellowChange = value * 10.0;
        }
        }

        private byte m_TrailingRedClear;
        /// <summary>
        ///     ''' Overlap Trailing Red Clear Parameter in tenth seconds (0-25.5 sec). 
        ///     ''' When the overlap green has been extended (Trailing Green), this value shall determine the current length of the Red Clearance interval for the overlap.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public double TrailingRedClear
        {
            get
            {
                return System.Convert.ToDouble(m_TrailingRedClear / 10.0);
        }
            set
            {
                m_TrailingRedClear = System.Convert.ToByte(value * 10.0);
        }
        }
    }

}
