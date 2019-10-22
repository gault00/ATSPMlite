using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSPMWatchDog
{
    public class NTCIPPreemptControl
    {
        // PROPERTIES------------------------------------------------------------------------------------

        private int m_Number;
        /// <summary>
        ///     ''' This object shall indicate the preempt input number controlled by the associated preemptControlState object in this row.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public int Number
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

        private bool m_State;
        /// <summary>
        ///     ''' This object when set to ON (one) shall cause the associated preempt actions to occur 
        ///     ''' unless the actions have already been started by the physical preempt input. 
        ///     ''' The preempt shall remain active as long as this object is ON or the physical
        ///     ''' preempt input is ON. This object when set to OFF (zero) shall cause the physical preempt input to
        ///     ''' control the associated preempt actions.
        ///     ''' The device shall reset this object to ZERO when in BACKUP Mode. 
        ///     ''' A write to this object shall reset the Backup timer to ZERO (see unitBackupTime).
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public bool State
        {
            get
            {
                return m_State;
            }
            set
            {
                m_State = value;
            }
        }
    }

}
