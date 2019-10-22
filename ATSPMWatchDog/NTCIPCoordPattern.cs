using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSPMWatchDog
{
    public class NTCIPCoordPattern
    {
        public enum patternTableType
        {
            other = 1,
            patterns = 2,
            offset3 = 3,
            offset5 = 4
        }

        // PROPERTIES------------------------------------------------------------------------------------

        private patternTableType m_TableType;
        /// <summary>
        ///     ''' This object provides information about any special organizational structure required for the pattern table. 
        ///     ''' The defined structures are as follows:
        ///     ''' other: The pattern table setup is not described in this standard, refer to device manual.
        ///     ''' patterns: Each row of the pattern table represents a unique pattern and has no dependencies on other rows.
        ///     ''' offset3: The pattern table is organized into plans which have three offsets. Each plan uses three consecutive rows. 
        ///     '''          Only patternOffsetTime and patternSequenceNumber values may vary between each of the three rows.
        ///     '''          Plan 1 is contained in rows 1, 2 and 3, Plan 2 is contained in rows 4, 5 and 6, Plan 3 is in rows 7, 8 and 9, etc.
        ///     ''' offset5: The pattern table is organized into plans which have five offsets. Each plan occupies five consecutive rows. 
        ///     '''          Only patternOffsetTime and patternSequenceNumber values may vary between each of the rows.
        ///     '''          Plan 1 is contained in rows 1, 2, 3, 4 and 5, Plan 2 is contained in rows 6, 7, 8, 9 and 10, 
        ///     '''          Plan 3 is contained in rows 11, 12, 13, 14 and 15, etc.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public patternTableType TableType
        {
            get
            {
                return m_TableType;
            }
            set
            {
                m_TableType = value;
            }
        }

        private byte m_Number;
        /// <summary>
        ///     ''' The pattern number for objects in this row. 
        ///     ''' This value shall not exceed the maxPatterns object value.
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

        private byte m_CycleTime;
        /// <summary>
        ///     ''' The patternCycleTime object specifies the length of the pattern cycle in seconds (NEMA TS 2 range: 30-255). 
        ///     ''' A pattern cycle time less than adequate to service the minimum requirements of all phases shall result in Free mode. 
        ///     ''' While this condition exists, the Local Free bit of unitAlarmStatus1 and the Local Override bit of shortAlarmStatus shall be set to one (1).
        ///     ''' 
        ///     ''' The minimum requirements of a phase with a not-actuated ped include Minimum Green, Walk, Pedestrian Clear, Yellow Clearance, and Red Clearance; 
        ///     ''' the minimum requirements of a phase with an actuated pedestrian include Minimum Green, Yellow Clearance, and Red Clearance. 
        ///     ''' If the pattern cycle time is zero and the associated split table (if any) contains values greater than zero, 
        ///     ''' then the CU shall utilize the split time values as maximum values for each phase.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public byte CycleTime
        {
            get
            {
                return m_CycleTime;
            }
            set
            {
                m_CycleTime = value;
            }
        }

        private byte m_OffsetTime;
        /// <summary>
        ///     ''' The patternOffsetTime defines by how many seconds (NEMA TS 2 range: 0-254) the local time zero 
        ///     ''' shall lag the system time zero (synchronization pulse) for this pattern. 
        ///     ''' An offset value equal to or greater than the patternCycleTime shall result in Free being the operational mode. 
        ///     ''' While this condition exists, the Local Free bit of unitAlarmStatus1 and the LocalOverride bit of shortAlarmStatus shall be set to one (1).
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public byte OffsetTime
        {
            get
            {
                return m_OffsetTime;
            }
            set
            {
                m_OffsetTime = value;
            }
        }

        private byte m_SplitPattern;
        /// <summary>
        ///     ''' This object is used to locate information in the splitTable to use for this pattern. 
        ///     ''' This value shall not exceed the maxSplits object value.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public byte SplitPattern
        {
            get
            {
                return m_SplitPattern;
            }
            set
            {
                m_SplitPattern = value;
            }
        }

        private byte m_SequenceNumber;
        /// <summary>
        ///     ''' This object is used to locate information in the sequenceTable to use with this pattern. 
        ///     ''' This value shall not exceed the maxSequences object value.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public byte SequenceNumber
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

        private byte m_CycleNumber;       // used for C/O/S determination
        public byte CycleNumber
        {
            get
            {
                return m_CycleNumber;
            }
            set
            {
                m_CycleNumber = value;
            }
        }

        private byte m_OffsetNumber;      // used for C/O/S determination
        public byte OffsetNumber
        {
            get
            {
                return m_OffsetNumber;
            }
            set
            {
                m_OffsetNumber = value;
            }
        }

        // constructor
        public NTCIPCoordPattern()
        {
            Number = 0;
            CycleTime = 0;
            OffsetTime = 0;
            SplitPattern = 0;
            CycleNumber = 0;
            OffsetNumber = 0;
        }
    }

}
