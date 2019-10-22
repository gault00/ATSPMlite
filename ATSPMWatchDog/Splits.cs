using System;

namespace ATSPMWatchDog
{
    public class SplitsUDOT
    {
        public int SignalID { get; set; }
        public DateTime Timestamp { get; set; }
        public byte EventCode { get; set; }
        public byte EventParam { get; set; }
    }

}
