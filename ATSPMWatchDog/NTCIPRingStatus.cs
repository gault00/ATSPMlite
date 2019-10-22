using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSPMWatchDog
{
    public class NTCIPRingStatus : NTCIPBase
    {
        // PROPERTIES------------------------------------------------------------------------------------

        private byte m_ringStatusByte;
        /// <summary>
        ///     ''' The Ring Status for this ring.
        ///     ''' Bit 7: Reserved (always zero)
        ///     ''' Bit 6: Reserved (always zero)
        ///     ''' Bit 5: Force Off - When bit = 1, the active phase in the ring was terminated by Force Off
        ///     ''' Bit 4: Max Out - When bit = 1, the active phase in the ring was terminated by Max Out
        ///     ''' Bit 3: Gap Out - When bit = 1, the active phase in the ring was terminated by Gap Out
        ///     ''' Bit 2: Coded Status Bit C
        ///     ''' Bit 1: Coded Status Bit B
        ///     ''' Bit 0: Coded Status Bit A
        ///     ''' +======+=====+=====+=====+===============+
        ///     ''' | Code |    Bit States   | State         |
        ///     ''' |  ##  |  A  |  B  |  C  | Names         |
        ///     ''' +======+=====+=====+=====+===============+
        ///     ''' |   0  |  0  |  0  |  0  | Min Green     |
        ///     ''' |   1  |  1  |  0  |  0  | Extension     |
        ///     ''' |   2  |  0  |  1  |  0  | Maximum       |
        ///     ''' |   3  |  1  |  1  |  0  | Green Rest    |
        ///     ''' |   4  |  0  |  0  |  1  | Yellow Change |
        ///     ''' |   5  |  1  |  0  |  1  | Red Clearance |
        ///     ''' |   6  |  0  |  1  |  1  | Red Rest      |
        ///     ''' |   7  |  1  |  1  |  1  | Undefined     |
        ///     ''' +======+=====+=====+=====+===============+
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public byte ringStatusByte
        {
            get
            {
                bool bitC, bitB, bitA;
                if (MinGreen == true)
                {
                    bitA = false;
                    bitB = false;
                    bitC = false;
                }
                else if (Extension == true)
                {
                    bitA = true;
                    bitB = false;
                    bitC = false;
                }
                else if (Maximum == true)
                {
                    bitA = false;
                    bitB = true;
                    bitC = false;
                }
                else if (GreenRest == true)
                {
                    bitA = true;
                    bitB = true;
                    bitC = false;
                }
                else if (YellowChange == true)
                {
                    bitA = false;
                    bitB = false;
                    bitC = true;
                }
                else if (RedClear == true)
                {
                    bitA = true;
                    bitB = false;
                    bitC = true;
                }
                else if (RedRest == true)
                {
                    bitA = false;
                    bitB = true;
                    bitC = true;
                }
                else if (Undefined == true)
                {
                    bitA = true;
                    bitB = true;
                    bitC = true;
                }
                else
                {
                    bitA = false;
                    bitB = false;
                    bitC = false;
                }
                m_ringStatusByte = CreateBit8(bitA, bitB, bitC, GapOut, MaxOut, ForceOff, false, false);
                return m_ringStatusByte;
            }
            set
            {
                m_ringStatusByte = value;
                ForceOff = GetBit8(value, 5);
                MaxOut = GetBit8(value, 4);
                GapOut = GetBit8(value, 3);
                bool bitA, bitB, bitC;
                bitC = GetBit8(value, 2);
                bitB = GetBit8(value, 1);
                bitA = GetBit8(value, 0);

                if (bitA == false & bitB == false & bitC == false)
                    MinGreen = true;
                if (bitA == true & bitB == false & bitC == false)
                    Extension = true;
                if (bitA == false & bitB == true & bitC == false)
                    Maximum = true;
                if (bitA == true & bitB == true & bitC == false)
                    GreenRest = true;
                if (bitA == false & bitB == false & bitC == true)
                    YellowChange = true;
                if (bitA == true & bitB == false & bitC == true)
                    RedClear = true;
                if (bitA == false & bitB == true & bitC == true)
                    RedRest = true;
                if (bitA == true & bitB == true & bitC == true)
                    Undefined = true;
            }
        }

        private bool m_ForceOff;
        /// <summary>
        ///     ''' When bit = 1, the active phase in the ring was terminated by Force Off
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public bool ForceOff
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

        private bool m_MaxOut;
        /// <summary>
        ///     ''' When bit = 1, the active phase in the ring was terminated by Max Out
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public bool MaxOut
        {
            get
            {
                return m_MaxOut;
            }
            set
            {
                m_MaxOut = value;
            }
        }

        private bool m_GapOut;
        /// <summary>
        ///     ''' When bit = 1, the active phase in the ring was terminated by Gap Out
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public bool GapOut
        {
            get
            {
                return m_GapOut;
            }
            set
            {
                m_GapOut = value;
            }
        }

        private bool m_MinGreen;
        /// <summary>
        ///     ''' Ring status is min green
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public bool MinGreen
        {
            get
            {
                return m_MinGreen;
            }
            set
            {
                m_MinGreen = value;
            }
        }

        private bool m_Extension;
        /// <summary>
        ///     ''' ring status is extension
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public bool Extension
        {
            get
            {
                return m_Extension;
            }
            set
            {
                m_Extension = value;
            }
        }

        private bool m_Maximum;
        /// <summary>
        ///     ''' ring status is maximum
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public bool Maximum
        {
            get
            {
                return m_Maximum;
            }
            set
            {
                m_Maximum = value;
            }
        }

        private bool m_GreenRest;
        /// <summary>
        ///     ''' ring status is green rest
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public bool GreenRest
        {
            get
            {
                return m_GreenRest;
            }
            set
            {
                m_GreenRest = value;
            }
        }

        private bool m_YellowChange;
        /// <summary>
        ///     ''' ring status is yellow change
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public bool YellowChange
        {
            get
            {
                return m_YellowChange;
            }
            set
            {
                m_YellowChange = value;
            }
        }

        private bool m_RedClear;
        /// <summary>
        ///     ''' ring status is read clear
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public bool RedClear
        {
            get
            {
                return m_RedClear;
            }
            set
            {
                m_RedClear = value;
            }
        }

        private bool m_RedRest;
        /// <summary>
        ///     ''' ring status is red rest
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public bool RedRest
        {
            get
            {
                return m_RedRest;
            }
            set
            {
                m_RedRest = value;
            }
        }

        private bool m_Undefined;
        /// <summary>
        ///     ''' ring status is undefined
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public bool Undefined
        {
            get
            {
                return m_Undefined;
            }
            set
            {
                m_Undefined = value;
            }
        }
    }

}
