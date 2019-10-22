using System;

namespace ATSPMWatchDog
{
    public class PhaseTerminationEvents
    {
        public int SignalID { get; set; }
        public DateTime TimeStamp { get; set; }
        public byte EventCode { get; set; }
        public byte EventParam { get; set; }
    }
}
