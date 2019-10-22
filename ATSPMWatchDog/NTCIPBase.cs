using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSPMWatchDog
{
    public class NTCIPBase
    {
        /// <summary>
        ///     ''' Determines boolean value based on a specified bit value within an 8-bit integer (byte)
        ///     ''' </summary>
        ///     ''' <param name="value">An 8-bit integer with individual values specified by bits</param>
        ///     ''' <param name="bitNumber">The bit number to be returned (bit 7=first position of string; bit 0=last position of string)</param>
        ///     ''' <returns>The value of the bit (boolean; 1=true, 0=false)</returns>
        ///     ''' <remarks></remarks>
        public bool GetBit8(byte value, byte bitNumber)
        {
            string tempstring;
            byte tempbit;
            int stringPositionIndex;
            tempstring = Convert.ToString(value, 2).PadLeft(8, '0');
            stringPositionIndex = 7 - bitNumber;
            tempbit = System.Convert.ToByte(tempstring.Substring(stringPositionIndex, 1));
            if (tempbit == 1)
                return true;
            else
                return false;
        }

        /// <summary>
        ///     ''' Determines boolean value based on a specified bit within a 16-bit integer
        ///     ''' </summary>
        ///     ''' <param name="value">A 16-bit integer with individual values specified by bits.</param>
        ///     ''' <param name="bitNumber">The bit number to be returned (bit 15=first position of string; bit 0=last position of string)</param>
        ///     ''' <returns>The value of the bit (boolean; 1=true, 0=false)</returns>
        ///     ''' <remarks></remarks>
        public bool GetBit16(int value, byte bitNumber)
        {
            string tempstring;
            byte tempbit;
            int stringPositionIndex;
            tempstring = Convert.ToString(value, 2).PadLeft(16, '0');
            stringPositionIndex = 15 - bitNumber;
            tempbit = System.Convert.ToByte(tempstring.Substring(stringPositionIndex, 1));
            if (tempbit == 1)
                return true;
            else
                return false;
        }

        public bool GetBit32(uint value, byte bitNumber)
        {
            string tempstring;
            byte tempbit;
            int stringPositionIndex;
            tempstring = Convert.ToString(value, 2).PadLeft(32, '0');
            stringPositionIndex = 31 - bitNumber;
            tempbit = System.Convert.ToByte(tempstring.Substring(stringPositionIndex, 1));
            if (tempbit == 1)
                return true;
            else
                return false;
        }

        public byte CreateBit8(bool Bit0, bool Bit1, bool Bit2, bool Bit3, bool Bit4, bool Bit5, bool Bit6, bool Bit7)
        {
            byte tempByte;
            tempByte = Convert.ToByte(Convert.ToByte(Bit7) * 128 + Convert.ToByte(Bit6) * 64 + Convert.ToByte(Bit5) * 32 + Convert.ToByte(Bit4) * 16 + Convert.ToByte(Bit3) * 8 + Convert.ToByte(Bit2) * 4 + Convert.ToByte(Bit1) + Convert.ToByte(Bit0));
            return tempByte;
        }

        public int CreateBit16(bool Bit0, bool Bit1, bool Bit2, bool Bit3, bool Bit4, bool Bit5, bool Bit6, bool Bit7, bool Bit8, bool Bit9, bool Bit10, bool Bit11, bool Bit12, bool Bit13, bool Bit14, bool Bit15)
        {
            byte tempByte;
            tempByte = Convert.ToByte(Convert.ToInt32(Bit15) * 32768 + Convert.ToInt32(Bit14) * 16384 + Convert.ToInt32(Bit13) * 8192 + Convert.ToInt32(Bit12) * 4096 + Convert.ToInt32(Bit11) * 2048 + Convert.ToInt32(Bit10) * 1024 + Convert.ToInt32(Bit9) * 512 + Convert.ToInt32(Bit8) * 256 + Convert.ToInt32(Bit7) * 128 + Convert.ToInt32(Bit6) * 64 + Convert.ToInt32(Bit5) * 32 + Convert.ToInt32(Bit4) * 16 + Convert.ToInt32(Bit3) * 8 + Convert.ToInt32(Bit2) * 4 + Convert.ToInt32(Bit1) + Convert.ToInt32(Bit0));
            return tempByte;
        }

        public uint CreateBit32(bool Bit0, bool Bit1, bool Bit2, bool Bit3, bool Bit4, bool Bit5, bool Bit6, bool Bit7, bool Bit8, bool Bit9, bool Bit10, bool Bit11, bool Bit12, bool Bit13, bool Bit14, bool Bit15, bool Bit16, bool Bit17, bool Bit18, bool Bit19, bool Bit20, bool Bit21, bool Bit22, bool Bit23, bool Bit24, bool Bit25, bool Bit26, bool Bit27, bool Bit28, bool Bit29, bool Bit30, bool Bit31)
        {
            byte tempByte;
            tempByte = Convert.ToByte(Convert.ToUInt32(Bit31) * 2147483648 + Convert.ToUInt32(Bit30) * 1073741824 + Convert.ToUInt32(Bit29) * 536870912 + Convert.ToUInt32(Bit28) * 268435456 + Convert.ToUInt32(Bit27) * 134217728 + Convert.ToUInt32(Bit26) * 67108864 + Convert.ToUInt32(Bit25) * 33554432 + Convert.ToUInt32(Bit24) * 16777216 + Convert.ToUInt32(Bit23) * 8388608 + Convert.ToUInt32(Bit22) * 4194304 + Convert.ToUInt32(Bit21) * 2097152 + Convert.ToUInt32(Bit20) * 1048576 + Convert.ToUInt32(Bit19) * 524288 + Convert.ToUInt32(Bit18) * 262144 + Convert.ToUInt32(Bit17) * 131072 + Convert.ToUInt32(Bit16) * 65536 + Convert.ToUInt32(Bit15) * 32768 + Convert.ToUInt32(Bit14) * 16384 + Convert.ToUInt32(Bit13) * 8192 + Convert.ToUInt32(Bit12) * 4096 + Convert.ToUInt32(Bit11) * 2048 + Convert.ToUInt32(Bit10) * 1024 + Convert.ToUInt32(Bit9) * 512 + Convert.ToUInt32(Bit8) * 256 + Convert.ToUInt32(Bit7) * 128 + Convert.ToUInt32(Bit6) * 64 + Convert.ToUInt32(Bit5) * 32 + Convert.ToUInt32(Bit4) * 16 + Convert.ToUInt32(Bit3) * 8 + Convert.ToUInt32(Bit2) * 4 + Convert.ToUInt32(Bit1) + Convert.ToUInt32(Bit0));
            return tempByte;
        }

        /// <summary>
        /// Returns the group number for a specified value (for example; phase 1 is in group 1, phase 8 is in group 1, phase 9 is in group 2, etc.)
        /// </summary>
        /// <param name="Number">A phase number, detector number, overlap number, or channel number</param>
        /// <returns>The group number assuming 8 per group</returns>
        /// <remarks></remarks>
        public int getGroupNumber(int Number)
        {
            return ((Number - 1) / 8) + 1;
        }

        /// <summary>
        /// Returns the bit in which a specified value occurs in groups of 8 (for example: phase 1 = bit 0, phase 8 = bit 7; phase 9 = bit 0, etc.)
        /// </summary>
        /// <param name="number">A phase number, detector channel, channel number, overlap number, etc.</param>
        /// <returns>The bit number assuming 8 values per group</returns>
        /// <remarks></remarks>
        public int getGroupBitNumber(int number)
        {
            return ((number - 1) % 8);
        }
    }
}
