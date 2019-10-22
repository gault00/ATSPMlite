using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSPMWatchDog
{
    public class NTCIPTimeBaseSchedule : NTCIPBase
    {
        private byte m_Number;
        /// <summary>
        ///     ''' The time base schedule number for objects in this row. 
        ///     ''' The value of this object shall not exceed the value of the
        ///     ''' maxTimeBaseScheduleEntries object. The activation of a scheduled
        ///     ''' entry shall occur whenever allowed by all other objects within this table.
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

        private int m_Month;
        /// <summary>
        ///     ''' The Month(s) Of the Year that the schedule entry shall be allowed. 
        ///     ''' Each bit represents a specific month. If the bit is set to one (1), 
        ///     ''' then the scheduled entry shall be allowed during the associated month. 
        ///     ''' If the bit is zero (0), then the scheduled entry shall not be allowed 
        ///     ''' during the associated month.
        ///     ''' The bits are defined as:
        ///     ''' Bit    Month of Year
        ///     ''' 0       Reserved
        ///     ''' 1       January
        ///     ''' 2       February
        ///     ''' 3       March
        ///     ''' 4       April
        ///     ''' 5       May
        ///     ''' 6       June
        ///     ''' 7       July
        ///     ''' 8       August
        ///     ''' 9       September
        ///     ''' 10      October
        ///     ''' 11      November
        ///     ''' 12      December
        ///     ''' 13 - 15 Reserved
        ///     ''' Thus, a value of six (6) would indicate that the entry would only
        ///     ''' be allowed during the months of January and February. A value of
        ///     ''' zero (0) shall indicate that this row has been disabled.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public int Month
        {
            get
            {
                m_Month = CreateBit16(false, MonthActive[1], MonthActive[2], MonthActive[3], MonthActive[4], MonthActive[5], MonthActive[6], MonthActive[7], MonthActive[8], MonthActive[9], MonthActive[10], MonthActive[11], MonthActive[12], false, false, false); 
                return m_Month;
            }
            set
            {
                m_Month = value;
                MonthActive[1] = GetBit16(value, 1);
                MonthActive[2] = GetBit16(value, 2);
                MonthActive[3] = GetBit16(value, 3);
                MonthActive[4] = GetBit16(value, 4);
                MonthActive[5] = GetBit16(value, 5);
                MonthActive[6] = GetBit16(value, 6);
                MonthActive[7] = GetBit16(value, 7);
                MonthActive[8] = GetBit16(value, 8);
                MonthActive[9] = GetBit16(value, 9);
                MonthActive[10] = GetBit16(value, 10);
                MonthActive[11] = GetBit16(value, 11);
                MonthActive[12] = GetBit16(value, 12);
            }
        }

        private bool[] MonthActive = new bool[13];
        /// <summary>
        ///     ''' Determines whether the time base action plan is active for the specified month
        ///     ''' </summary>
        ///     ''' <param name="MonthNumber">The number of the month (January=1, December=12, etc.)</param>
        ///     ''' <value></value>
        ///     ''' <returns>true or false</returns>
        ///     ''' <remarks></remarks>
        //public bool MonthActive
        //{
        //    get
        //    {
        //        return m_MonthActive[MonthNumber];
        //    }
        //    set
        //    {
        //        m_MonthActive[MonthNumber] = value;
        //    }
        //}

        private byte m_DayOfWeek;
        /// <summary>
        ///     ''' The Day(s) Of Week that the schedule entry shall be allowed. 
        ///     ''' Each bit represents a specific day of the week. 
        ///     ''' If the bit is set to one (1), then the scheduled entry shall be allowed during the associated DOW. 
        ///     ''' If the bit is set to zero (0), then the scheduled entry shall not be allowed during the associated DOW. 
        ///     ''' The bits are defined as:
        ///     ''' 
        ///     ''' Bit Day of Week
        ///     ''' 0   Reserved ('Holiday', not defined by this standard)
        ///     ''' 1   Sunday
        ///     ''' 2   Monday
        ///     ''' 3   Tuesday
        ///     ''' 4   Wednesday
        ///     ''' 5   Thursday
        ///     ''' 6   Friday
        ///     ''' 7   Saturday
        ///     ''' 
        ///     ''' Thus, a value of six (6) would indicate that the entry would only be allowed on Sundays and Mondays. 
        ///     ''' A value of zero (0) shall indicate that this row has been disabled.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public byte DayOfWeek
        {
            get
            {
                m_DayOfWeek = CreateBit8(false, DayOfWeekActive[1], DayOfWeekActive[2], DayOfWeekActive[3], DayOfWeekActive[4], DayOfWeekActive[5], DayOfWeekActive[6], DayOfWeekActive[7]);
                return m_DayOfWeek;
            }
            set
            {
                m_DayOfWeek = value;
                DayOfWeekActive[1] = GetBit8(value, 1);
                DayOfWeekActive[2] = GetBit8(value, 2);
                DayOfWeekActive[3] = GetBit8(value, 3);
                DayOfWeekActive[4] = GetBit8(value, 4);
                DayOfWeekActive[5] = GetBit8(value, 5);
                DayOfWeekActive[6] = GetBit8(value, 6);
                DayOfWeekActive[7] = GetBit8(value, 7);
            }
        }

        private bool[] DayOfWeekActive = new bool[8];
        /// <summary>
        ///     ''' Determines whether the TOD action plan is active for the specified day of the week.
        ///     ''' </summary>
        ///     ''' <param name="DayNumber">Day of week (Sunday=1, Monday=2, ... ,Saturday=7)</param>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        //public bool DayOfWeekActive
        //{
        //    get
        //    {
        //        return m_DayOfWeekActive[DayNumber];
        //    }
        //    set
        //    {
        //        m_DayOfWeekActive[DayNumber] = value;
        //    }
        //}

        private uint m_DateOfMonth;
        /// <summary>
        /// The Day(s) Of a Month that the schedule entry shall be allowed. 
        /// Each bit represents a specific date of the month. 
        /// If the bit is set to one (1), then the scheduled entry shall be allowed during the associated date. 
        /// If the bit is set to zero (0), then the scheduled entry shall not be allowed during the associated date. 
        /// The bits are defined as:
        /// Bit Day Number;
        /// 0   Reserved;
        /// 1   Day 1;
        /// 2   Day 2;
        /// ||
        /// 31  Day 31;
        /// Thus, a value of six (6) would indicate that the entry would only be allowed on the first and second of the allowed months. 
        /// A value of zero (0) shall indicate that this row has been disabled.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public uint DateOfMonth
        {
            get
            {
                m_DateOfMonth = CreateBit32(false, DateActive[1], DateActive[2], DateActive[3], DateActive[4], DateActive[5], DateActive[6], DateActive[7], DateActive[8], DateActive[9], DateActive[10], DateActive[11], DateActive[12], DateActive[13], DateActive[14], DateActive[15], DateActive[16], DateActive[17], DateActive[18], DateActive[19], DateActive[20], DateActive[21], DateActive[22], DateActive[23], DateActive[24], DateActive[25], DateActive[26], DateActive[27], DateActive[28], DateActive[29], DateActive[30], DateActive[31]);
                return m_DateOfMonth;
            }
            set
            {
                m_DateOfMonth = value;
                DateActive[1] = GetBit32(value, 1);
                DateActive[2] = GetBit32(value, 2);
                DateActive[3] = GetBit32(value, 3);
                DateActive[4] = GetBit32(value, 4);
                DateActive[5] = GetBit32(value, 5);
                DateActive[6] = GetBit32(value, 6);
                DateActive[7] = GetBit32(value, 7);
                DateActive[8] = GetBit32(value, 8);
                DateActive[9] = GetBit32(value, 9);
                DateActive[10] = GetBit32(value, 10);
                DateActive[11] = GetBit32(value, 11);
                DateActive[12] = GetBit32(value, 12);
                DateActive[13] = GetBit32(value, 13);
                DateActive[14] = GetBit32(value, 14);
                DateActive[15] = GetBit32(value, 15);
                DateActive[16] = GetBit32(value, 16);
                DateActive[17] = GetBit32(value, 17);
                DateActive[18] = GetBit32(value, 18);
                DateActive[19] = GetBit32(value, 19);
                DateActive[20] = GetBit32(value, 20);
                DateActive[21] = GetBit32(value, 21);
                DateActive[22] = GetBit32(value, 22);
                DateActive[23] = GetBit32(value, 23);
                DateActive[24] = GetBit32(value, 24);
                DateActive[25] = GetBit32(value, 25);
                DateActive[26] = GetBit32(value, 26);
                DateActive[27] = GetBit32(value, 27);
                DateActive[28] = GetBit32(value, 28);
                DateActive[29] = GetBit32(value, 29);
                DateActive[30] = GetBit32(value, 30);
                DateActive[31] = GetBit32(value, 31);
            }
        }

        private bool[] DateActive = new bool[32];

        /// <summary>
        ///This object specifies what Plan number shall be associated with this timeBaseScheduleDayPlan -object. 
        ///A value of zero (0) shall indicate that this row has been disabled.
        ///</summary>
        ///<value></value>
        ///<returns></returns>
        ///<remarks></remarks>
        public byte DayPlan { get; set; }

        /// <summary>
        /// This object indicates the number of the TimeBaseSchedule which is currently selected by the scheduling logic; 
        /// the device may or may not be using the selected schedule. 
        /// The value of zero (0) indicates that there is no timeBaseScheduleNumber that is currently selected.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public byte Status { get; }
    }
}
