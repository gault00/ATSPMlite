using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSPMWatchDog
{
    public class NTCIPRingControl
    {
        /// <summary>
        /// The Ring Control Group number for objects in this row. 
        /// This value shall not exceed the maxRingControlGroups object value.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public byte Number { get; set; }

        /// <summary>
        /// This object is used to allow a remote entity to stop timing in the device. 
        /// The device shall activate/deactivate the System Stop Time control for a ring according to the respective bit value as follows:
        /// bit = 0 - deactivate the ring control
        /// bit = 1 - activate the ring control
        /// Bit 7: Ring # = (ringControlGroupNumber * 8)
        /// Bit 6: Ring # = (ringControlGroupNumber * 8) - 1
        /// Bit 5: Ring # = (ringControlGroupNumber * 8) - 2
        /// Bit 4: Ring # = (ringControlGroupNumber * 8) - 3
        /// Bit 3: Ring # = (ringControlGroupNumber * 8) - 4
        /// Bit 2: Ring # = (ringControlGroupNumber * 8) - 5
        /// Bit 1: Ring # = (ringControlGroupNumber * 8) - 6
        /// Bit 0: Ring # = (ringControlGroupNumber * 8) - 7
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public byte StopType { get; set; }

        public bool[] ForceOff = new bool[9];
        /// <summary>
        ///     ''' This object is used to allow a remote entity to terminate phases via a force off command in the device. 
        ///     ''' The device shall activate/deactivate the System Force Off control for a ring according to the respective bit value as follows:
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        //public bool ForceOff[int RingNumber]
        //{
        //    get
        //    {
        //        return m_ForceOff[RingNumber];
        //    }
        //    set
        //    {
        //        m_ForceOff[RingNumber] = value;
        //    }
        //}

        public bool[] Max2control = new bool[9];
        /// <summary>
        ///     ''' This object is used to allow a remote entity to request Maximum 2 timings in the device.
        ///     ''' The device shall activate/deactivate the System Maximum 2 control for a ring according to the
        ///     ''' respective bit value as follows:
        ///     ''' bit = 0 - deactivate the ring control
        ///     ''' bit = 1 - activate the ring control
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        //public bool Max2control
        //{
        //    get
        //    {
        //        return m_Max2control[RingNumber];
        //    }
        //    set
        //    {
        //        m_Max2control[RingNumber] = value;
        //    }
        //}

        public bool[] MaxInhibit = new bool[9];
        /// <summary>
        ///     ''' This object is used to allow a remote entity to request internal maximum timings be inhibited in the device. 
        ///     ''' The device shall activate/deactivate the System Max Inhibit control for a ring according to the respective bit value as follows:
        ///     ''' bit = 0 - deactivate the ring control
        ///     ''' bit = 1 - activate the ring control
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        //public bool MaxInhibit
        //{
        //    get
        //    {
        //        return m_MaxInhibit[RingNumber];
        //    }
        //    set
        //    {
        //        m_MaxInhibit[RingNumber] = value;
        //    }
        //}

        public bool[] PedRecycle = new bool[9];
        /// <summary>
        ///     ''' This object is used to allow a remote entity to request a pedestrian recycle in the device. 
        ///     ''' The device shall activate/deactivate the System Ped Recycle control for a ring according to the respective bit value as follows:
        ///     ''' bit = 0 - deactivate the ring control
        ///     ''' bit = 1 - activate the ring control
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        //public bool PedRecycle
        //{
        //    get
        //    {
        //        return m_PedRecycle[RingNumber];
        //    }
        //    set
        //    {
        //        m_PedRecycle[RingNumber] = value;
        //    }
        //}

        public bool[] RedRest = new bool[9];
        /// <summary>
        ///     ''' This object is used to allow a remote entity to request red rest in the device. 
        ///     ''' The device shall activate/deactivate the System Red Rest control for a ring according to the respective bit value as follows:
        ///     ''' bit = 0 - deactivate the ring control
        ///     ''' bit = 1 - activate the ring control
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        //public bool RedRest
        //{
        //    get
        //    {
        //        return m_RedRest[RingNumber];
        //    }
        //    set
        //    {
        //        m_RedRest[RingNumber] = value;
        //    }
        //}

        public bool[] OmitRed = new bool[9];
        /// <summary>
        ///     ''' This object is used to allow a remote entity to omit red clearances in the device. 
        ///     ''' The device shall activate/deactivate the System Omit Red Clear control for a ring according to the respective bit value as follows:
        ///     ''' bit = 0 - deactivate the ring control
        ///     ''' bit = 1 - activate the ring control
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        //public bool OmitRed
        //{
        //    get
        //    {
        //        return m_OmitRed[RingNumber];
        //    }
        //    set
        //    {
        //        m_OmitRed[RingNumber] = value;
        //    }
        //}
    }

}
