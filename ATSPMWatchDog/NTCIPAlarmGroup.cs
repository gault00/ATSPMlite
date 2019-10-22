using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSPMWatchDog
{
    public class NTCIPAlarmGroup
    {
        /// <summary>
        /// The alarm group number for objects in this row. This value shall not exceed the maxAlarmGroups object value.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public byte Number { get; set; }

        /// <summary>
        /// Alarm input state bit field. When a bit = 1, the associated physical alarm input is active.
        /// <para>When a bit = 0, the associated alarm input is NOT active.</para>
        /// <para>Bit 7: Alarm Input # = ( alarmGroupNumber * 8)</para>
        /// <para>Bit 6: Alarm Input # = ( alarmGroupNumber * 8) -1</para>
        /// <para>Bit 5: Alarm Input # = ( alarmGroupNumber * 8) -2</para>
        /// <para>Bit 4: Alarm Input # = ( alarmGroupNumber * 8) -3</para>
        /// <para>Bit 3: Alarm Input # = ( alarmGroupNumber * 8) -4</para>
        /// <para>Bit 2: Alarm Input # = ( alarmGroupNumber * 8) -5</para>
        /// <para>Bit 1: Alarm Input # = ( alarmGroupNumber * 8) -6</para>
        /// <para>Bit 0: Alarm Input # = ( alarmGroupNumber * 8) -7</para>
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public byte State { get; set; }
    }

}
