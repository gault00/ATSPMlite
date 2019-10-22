using System;

namespace ATSPMWatchDog
{
    public class SPMWatchDogErrorEvent
    {
        public int ID { get; set; }
        public DateTime TimeStamp { get; set; }
        public int SignalID { get; set; }
        public string DetectorID { get; set; }
        public string Direction { get; set; }
        public int Phase { get; set; }
        public int ErrorCode { get; set; }
        public string Message { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            var y = (SPMWatchDogErrorEvent)obj;

            return this != null && y != null && TimeStamp == y.TimeStamp && SignalID == y.SignalID && Phase == y.Phase && ErrorCode == y.ErrorCode;
        }

        public override int GetHashCode()
        {
            return this == null ? 0 : TimeStamp.GetHashCode() ^ SignalID.GetHashCode() ^ Phase.GetHashCode();
        }
    }
}
