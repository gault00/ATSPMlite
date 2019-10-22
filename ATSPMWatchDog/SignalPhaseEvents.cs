using System;

namespace ATSPMWatchDog
{
    public class SignalPhaseEventsUDOT
    {
        public DateTime TimeStamp { get; set; }
        public byte EventCode { get; set; }
        public byte EventParam { get; set; }
    }

}
