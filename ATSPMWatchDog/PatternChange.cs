using System;

namespace ATSPMWatchDog
{
    public class PatternChange
    {
        private DateTime mTimeStamp;
        /// <summary>
        ///     ''' Timestamp of pattern change
        ///     ''' </summary>
        ///     ''' <returns></returns>
        public DateTime TimeStamp
        {
            get
            {
                return mTimeStamp;
            }
            set
            {
                mTimeStamp = value;
            }
        }

        private byte mEventParam;
        public byte EventParam
        {
            get
            {
                return mEventParam;
            }
            set
            {
                mEventParam = value;
            }
        }
    }
}
