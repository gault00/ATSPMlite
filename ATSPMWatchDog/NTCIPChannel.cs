namespace ATSPMWatchDog
{
    public class NTCIPChannel : NTCIPBase
    {
       public enum ControlTypes
        {
            other = 1,
            phaseVehicle = 2,
            phasePedestrian = 3,
            overlap = 4
        }

        /// <summary>
        /// The channel number for objects in this row. This value shall not exceed the maxChannels object value.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public byte Number { get; set; }

        /// <summary>
        /// This object defines the channel control source (which Phase or Overlap). 
        /// The value shall not exceed maxPhases or maxOverlaps as determined by channelControlType object:
        /// <para>Value 00 = No Control (Not In Use)</para>
        /// <para>Value 01 = Phase 01 or Overlap A</para>
        /// <para>Value 02 = Phase 02 or Overlap B</para>
        /// <para>              ||</para>
        /// <para>Value 15 = Phase 15 or Overlap O</para>
        /// <para>Value 16 = Phase 16 or Overlap P</para>
        /// <para>              etc.</para>
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public byte ControlSource { get; set; }

        /// <summary>
        /// This object defines the channel control type (Vehicle Phase, Pedestrian Phase, or Overlap):
        /// other: The channel controls an other type of display.
        /// phaseVehicle: The channel controls a vehicle phase display.
        /// phasePedestrian: The channel controls a pedestrian phase display.
        /// overlap: The channel controls an overlap display.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public ControlTypes ControlType { get; set; }

        private byte m_ChannelFlash;
        /// <summary>
        /// This object defines the channel state during Automatic Flash.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public byte ChannelFlash
        {
            get
            {
                m_ChannelFlash = CreateBit8(false, FlashYellow, FlashRed, FlashAlternateHalfHertz, false, false, false, false);
                return m_ChannelFlash;
            }
            set
            {
                m_ChannelFlash = value;
                // bits 7, 6, 5, 4 reserved
                FlashAlternateHalfHertz = GetBit8(value, 3);
                FlashRed = GetBit8(value, 2);
                FlashYellow = GetBit8(value, 1);
                if (FlashRed == true & FlashYellow == true)
                {
                    FlashRed = true;
                    FlashYellow = false;
                }
            }
        }

        /// <summary>
        /// Bit=0: Off/Disabled and Bit=1: On/Enabled
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool FlashAlternateHalfHertz { get; set; }

        /// <summary>
        /// Bit=0: Off/Red Dark and Bit=1: On/Flash Red
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool FlashRed { get; set; }

        /// <summary>
        /// Bit=0: Off/Yellow Dark and Bit=1: On/Flash Yellow
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool FlashYellow { get; set; }

        private byte m_Dimming;
        /// <summary>
        /// This object defines the channel state during Dimming. 
        /// Dimming shall be accomplished by the elimination of alternate one half segments from the
        /// AC sinusoid applied to the field terminals.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public byte Dimming
        {
            get
            {
                m_Dimming = CreateBit8(DimGreen, DimYellow, DimRed, DimAlternateHalfLineCycle, false, false, false, false);
                return m_Dimming;
            }
            set
            {
                m_Dimming = value;
                // bits 7, 6, 5, 4 reserved
                DimAlternateHalfLineCycle = GetBit8(value, 3);
                DimRed = GetBit8(value, 2);
                DimYellow = GetBit8(value, 1);
                DimGreen = GetBit8(value, 0);
            }
        }

        /// <summary>
        /// Bit=0: Off/+ half cycle and Bit=1: On/- half cycle
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool DimAlternateHalfLineCycle { get; set; }

        /// <summary>
        /// Bit=0: Off/Red Not Dimmed and Bit=1: On/Dimmed Red
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool DimRed { get; set; }

        /// <summary>
        /// Bit=0: Off / Yellow Not Dimmed and Bit=1: On / Dimmed Yellow
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool DimYellow { get; set; }

        /// <summary>
        /// Bit=0: Off / Green Not Dimmed and Bit=1: On / Dimmed Green
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool DimGreen { get; set; }
    }
}
