using System;

public class AnalysisPhaseCycleUDOT
{
    public enum NextEventResponse
    {
        CycleOK,
        CycleMissingData,
        CycleComplete
    }

    public enum TerminationType : byte
    {
        GapOut = 4,
        MaxOut = 5,
        ForceOff = 6,
        Unknown = 0
    }

    public byte PhaseNumber { get; set; }
    public int SignalID { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public DateTime pedStartTime { get; set; }
    public DateTime PedEndTime { get; set; }
    public byte TerminationEvent { get; set; }

    private TimeSpan mDuration;
    public TimeSpan Duration
    {
        get
        {
            return mDuration;
        }
        set
        {
            mDuration = value;
        }
    }

    private TimeSpan mPedDuration;
    public TimeSpan PedDuration
    {
        get
        {
            return mPedDuration;
        }
        set
        {
            mPedDuration = value;
        }
    }

    public bool hasPed { get; set; }

    public AnalysisPhaseCycleUDOT(int mysignalID, byte myphasenumber, DateTime mystarttime)
    {
        SignalID = mysignalID;
        PhaseNumber = myphasenumber;
        StartTime = mystarttime;
        hasPed = false;
        TerminationEvent = 0;
    }

    public void SetTerminationEvent(byte TerminationCode)
    {
        TerminationEvent = TerminationCode;
    }

    public void SetEndTime(DateTime myEndTime)
    {
        EndTime = myEndTime;
        Duration = EndTime.Subtract(StartTime);
    }

    public void SetPedStart(DateTime starttime)
    {
        pedStartTime = starttime;
        hasPed = true;
    }

    public void SetPedEnd(DateTime endtime)
    {
        PedEndTime = endtime;
        PedDuration = PedEndTime.Subtract(pedStartTime);
    }
}
