using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace ATSPMWatchDog
{
    public class PlanUDOT
    {
        public List<Cycle> CycleCollection { get; } = new List<Cycle>();

        /// <summary>
        /// The start time of the plan
        /// </summary>
        /// <returns></returns>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// The end time of the plan
        /// </summary>
        /// <returns></returns>
        public DateTime EndTime { get; }
        public int CycleCount { get; set; }
        public int CycleLength { get; set; }
        public int TotalDetectorHits { get; set; }
        public int EightyFifth { get; set; }
        public int AvgSpeed { get; set; }
        public int StdDevAvgSpeed { get; set; }
        public int OffsetLength { get; set; }

        public double AvgDelay
        {
            get
            {
                return TotalDelay / TotalVolume;
            }
        }

        /// <summary>
        /// The percent of time the phase is green
        /// </summary>
        /// <returns></returns>
        public double PercentGreen
        {
            get
            {
                if (TotalTime > 0)
                    return Math.Round((TotalGreenTime / TotalTime) * 100);
                else
                    return 0;
            }
        }

        /// <summary>
        /// The percent of activations on green
        /// </summary>
        /// <returns></returns>
        public double PercentArrivalOnGreen
        {
            get
            {
                if (TotalVolume > 0)
                    return Math.Round((TotalArrivalOnGreen / TotalVolume) * 100);
                else
                    return 0;
            }
        }

        public double PlatoonRatio
        {
            get
            {
                if (TotalVolume > 0)
                    return Math.Round((PercentArrivalOnGreen / PercentGreen), 2);
                else
                    return 0;
            }
        }

        /// <summary>
        /// Returns the Arrival Type (1-6) calculated per HCM 2000. Added by Steve Gault.
        /// </summary>
        /// <returns></returns>
        public double ArrivalType
        {
            get
            {
                if (TotalVolume > 0)
                {
                    double pr = PlatoonRatio;
                    if (pr <= 0.5)
                        return Math.Round((2 * pr + 1), 2);
                    else if (pr > 0.5 & pr <= 0.85)
                        return Math.Round((pr / 0.35 + (3 - 0.85 / 0.35)), 2);
                    else if (pr > 0.85 & pr <= 1.15)
                        return Math.Round((pr / 0.3 + (4 - 1.15 / 0.3)), 2);
                    else if (pr > 1.15 & pr <= 1.5)
                        return Math.Round((pr / 0.35 + (5 - 1.5 / 0.35)), 2);
                    else if (pr > 1.5 & pr <= 2.0)
                        return Math.Round((2 * pr + 2), 2);
                    else
                        return 6;
                }
                else
                    return 0;
            }
        }

        private double mTotalArrivalOnGreen = -1;
        public double TotalArrivalOnGreen
        {
            get
            {
                if (mTotalArrivalOnGreen == -1)
                    mTotalArrivalOnGreen = CycleCollection.Sum(d => d.TotalArrivalOnGreen);
                return mTotalArrivalOnGreen;
            }
        }

        private double mTotalArrivalOnRed = -1;
        public double TotalArrivalOnRed
        {
            get
            {
                if (mTotalArrivalOnRed == -1)
                    mTotalArrivalOnRed = CycleCollection.Sum(d => d.TotalArrivalOnRed);
                return mTotalArrivalOnRed;
            }
        }

        public double TotalDelay
        {
            get
            {
                return CycleCollection.Sum(d => d.TotalDelay);
            }
        }

        private double mTotalVolume = -1;
        public double TotalVolume
        {
            get
            {
                if (mTotalVolume == -1)
                    mTotalVolume = CycleCollection.Sum(d => d.TotalVolume);
                return mTotalVolume;
            }
        }

        private double mTotalGreenTime = -1;
        public double TotalGreenTime
        {
            get
            {
                if (mTotalGreenTime == -1)
                    mTotalGreenTime = CycleCollection.Sum(d => d.TotalGreenTime);
                return mTotalGreenTime;
            }
        }

        private double mTotalYellowTime = -1;
        public double TotalYellowTime
        {
            get
            {
                if (mTotalYellowTime == -1)
                    mTotalYellowTime = CycleCollection.Sum(d => d.TotalYellowTime);
                return mTotalYellowTime;
            }
        }

        private double mTotalRedTime = -1;
        public double TotalRedTime
        {
            get
            {
                if (mTotalRedTime == -1)
                    mTotalRedTime = CycleCollection.Sum(d => d.TotalRedTime);
                return mTotalRedTime;
            }
        }

        public double TotalTime
        {
            get
            {
                return CycleCollection.Sum(d => d.TotalTime);
            }
        }

        public SortedDictionary<int, int> Splits = new SortedDictionary<int, int>();

        /// <summary>
        ///  The plan number
        ///  </summary>
        ///  <returns></returns>
        public byte PlanNumber { get; }

        /// <summary>
        /// The signal number
        /// </summary>
        /// <returns></returns>
        public int SignalID { get; }

        /// <summary>
        /// The plan number (Shoudl be phase number?)
        /// </summary>
        /// <returns></returns>
        public byte PhaseNumber { get; }

        /// <summary>
        /// Constructor that sets the start time, end time and plan, and creates the lower level objects and statistics
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="plan"></param>
        /// <param name="SignalTable"></param>
        /// <param name="PreemptTable"></param>
        /// <param name="signalID"></param>
        /// <param name="phase"></param>
        public PlanUDOT(DateTime startTime, DateTime endTime, byte plan, List<SignalPhaseEventsUDOT> SignalTable, List<DetectorEventsUDOT> DetectorTable, List<SignalPhaseEventsUDOT> PreemptTable, int signalID, byte phase)
        {
            SignalID = signalID;
            PhaseNumber = phase;
            StartTime = startTime;
            EndTime = endTime;
            PlanNumber = plan;

            GetGreenYellowRedCycle(startTime, endTime, SignalTable, DetectorTable, PreemptTable);
        }

        /// <summary>
        /// Constructor that sets the start time, end time and plan and nothing else
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="plan"></param>
        public PlanUDOT(DateTime starttime, DateTime endtime, byte plan)
        {
            StartTime = starttime;
            EndTime = endtime;
            PlanNumber = plan;
        }

        /// <summary>
        /// Translate the event code to enumerations
        /// </summary>
        /// <param name="event_code"></param>
        /// <returns></returns>
        private Cycle.EventType GetEventType(byte event_code)
        {
            switch (event_code)
            {
                case 1:
                    {
                        return Cycle.EventType.ChangeToGreen;
                    }

                case 61: // overlap green
                    {
                        return Cycle.EventType.ChangeToGreen;
                    }

                case 8:
                    {
                        return Cycle.EventType.ChangeToYellow;
                    }

                case 63: // overlap yellow
                    {
                        return Cycle.EventType.ChangeToYellow;
                    }

                case 10:
                    {
                        return Cycle.EventType.ChangeToRed;
                    }

                case 64: // overlap red
                    {
                        return Cycle.EventType.ChangeToRed;
                    }

                default:
                    {
                        return Cycle.EventType.Unknown;
                    }
            }
        }

        public void SetHighCycleCount(AnalysisPhaseCollectionUDOT phases)
        {
            // find all the phase cycles within the plan
            int HighCycleCount = 0;

            foreach (AnalysisPhaseUDOT Phase in phases.Items)
            {
                IEnumerable<AnalysisPhaseCycleUDOT> Cycles;
                Cycles = Phase.Cycles.Items.Where(d => d.StartTime > StartTime & d.EndTime < EndTime);

                if (Cycles.Count() > HighCycleCount)
                    HighCycleCount = Cycles.Count();
            }
            CycleCount = HighCycleCount;
        }

        // set programmed splits
        public void SetProgrammedSplits(string signal_id)
        {
            Splits.Clear();

            IEnumerable<SplitsUDOT> SplitsDT;
            // UDOT adds 2 seconds to capture difference between when pattern changes and when split change is recorded
            // SplitsDT = UDOTSplitsData.Where(Function(d) d.Timestamp >= mStartTime And d.Timestamp <= mStartTime.AddSeconds(2))

            // Econolite records new splits at the system zero time, which is the nearest multiple of the cycle length after midnight.
            // For simplicity, just add one cycle length and it should be fine.
            SplitsDT = Globals.UDOTSplitsData.Where(d => d.Timestamp >= StartTime & d.Timestamp <= StartTime.AddSeconds(CycleLength));

            foreach (SplitsUDOT row in SplitsDT)
            {
                if (row.EventCode == 132)
                    CycleLength = row.EventParam;
                if (row.EventCode == 133)
                    OffsetLength = row.EventParam;
                if (row.EventCode == 134 & !Splits.ContainsKey(1))
                    Splits.Add(1, row.EventParam);
                if (row.EventCode == 134 & row.EventParam > 0)
                    Splits[1] = row.EventParam;
                if (row.EventCode == 135 & !Splits.ContainsKey(2))
                    Splits.Add(2, row.EventParam);
                if (row.EventCode == 135 & row.EventParam > 0)
                    Splits[2] = row.EventParam;
                if (row.EventCode == 136 & !Splits.ContainsKey(3))
                    Splits.Add(3, row.EventParam);
                if (row.EventCode == 136 & row.EventParam > 0)
                    Splits[3] = row.EventParam;
                if (row.EventCode == 137 & !Splits.ContainsKey(4))
                    Splits.Add(4, row.EventParam);
                if (row.EventCode == 137 & row.EventParam > 0)
                    Splits[4] = row.EventParam;
                if (row.EventCode == 138 & !Splits.ContainsKey(5))
                    Splits.Add(5, row.EventParam);
                if (row.EventCode == 138 & row.EventParam > 0)
                    Splits[5] = row.EventParam;
                if (row.EventCode == 139 & !Splits.ContainsKey(6))
                    Splits.Add(6, row.EventParam);
                if (row.EventCode == 139 & row.EventParam > 0)
                    Splits[6] = row.EventParam;
                if (row.EventCode == 140 & !Splits.ContainsKey(7))
                    Splits.Add(7, row.EventParam);
                if (row.EventCode == 140 & row.EventParam > 0)
                    Splits[7] = row.EventParam;
                if (row.EventCode == 141 & !Splits.ContainsKey(8))
                    Splits.Add(8, row.EventParam);
                if (row.EventCode == 141 & row.EventParam > 0)
                    Splits[8] = row.EventParam;
                if (row.EventCode == 142 & !Splits.ContainsKey(9))
                    Splits.Add(9, row.EventParam);
                if (row.EventCode == 142 & row.EventParam > 0)
                    Splits[9] = row.EventParam;
                if (row.EventCode == 143 & !Splits.ContainsKey(10))
                    Splits.Add(10, row.EventParam);
                if (row.EventCode == 143 & row.EventParam > 0)
                    Splits[10] = row.EventParam;
                if (row.EventCode == 144 & !Splits.ContainsKey(11))
                    Splits.Add(11, row.EventParam);
                if (row.EventCode == 144 & row.EventParam > 0)
                    Splits[11] = row.EventParam;
                if (row.EventCode == 145 & !Splits.ContainsKey(12))
                    Splits.Add(12, row.EventParam);
                if (row.EventCode == 145 & row.EventParam > 0)
                    Splits[12] = row.EventParam;
                if (row.EventCode == 146 & !Splits.ContainsKey(13))
                    Splits.Add(13, row.EventParam);
                if (row.EventCode == 146 & row.EventParam > 0)
                    Splits[13] = row.EventParam;
                if (row.EventCode == 147 & !Splits.ContainsKey(14))
                    Splits.Add(14, row.EventParam);
                if (row.EventCode == 147 & row.EventParam > 0)
                    Splits[14] = row.EventParam;
                if (row.EventCode == 148 & !Splits.ContainsKey(15))
                    Splits.Add(15, row.EventParam);
                if (row.EventCode == 148 & row.EventParam > 0)
                    Splits[15] = row.EventParam;
                if (row.EventCode == 149 & !Splits.ContainsKey(16))
                    Splits.Add(16, row.EventParam);
                if (row.EventCode == 149 & row.EventParam > 0)
                    Splits[16] = row.EventParam;
            }

            if (Splits.Count == 0)
            {
                for (var i = 0; i <= 15; i++)
                    Splits.Add(i, 0);
            }
        }

        // find highest recorded split phase
        public int FindHighestRecordedSplitPhase()
        {
            int phase = 0;
            int MaxKey;
            MaxKey = Splits.Max(d => d.Key);
            phase = MaxKey;
            return phase;
        }

        // fill missing splits
        public void FillMissingSplits(int highestSplit)
        {
            for (var counter = 0; counter <= highestSplit + 1; counter++)
            {
                if (Splits.ContainsKey(counter))
                {
                }
                else
                    Splits.Add(counter, 0);
            }
        }

        private void GetGreenYellowRedCycle(DateTime start_time, DateTime end_time, List<SignalPhaseEventsUDOT> cycleEvents, List<DetectorEventsUDOT> detectorEvents, List<SignalPhaseEventsUDOT> preemptEvents)
        {
            Cycle pcd;
            pcd = null/* TODO Change to default(_) if this is not a reference type */;

            // use a counter to help determine when we are on the last row
            int counter = 0;

            foreach (SignalPhaseEventsUDOT row in cycleEvents)
            {
                // use a counter to help determine when we are on the last row
                counter += 1;
                if ((row.TimeStamp >= start_time & row.TimeStamp <= end_time))
                {
                    // if this is the first PCD group we need to handle a special case
                    // where the PCD starts at the start of the requested period to
                    // make sure we include all data
                    if (pcd != null)
                    {
                        // make the first group start on the exact start of the requested period
                        pcd = new Cycle(start_time);
                        // add a green event if the first event is yellow
                        if (GetEventType(row.EventCode) == Cycle.EventType.ChangeToYellow)
                            pcd.NextEvent(Cycle.EventType.ChangeToGreen, start_time.AddMilliseconds(1));
                        else if (GetEventType(row.EventCode) == Cycle.EventType.ChangeToRed)
                        {
                            pcd.NextEvent(Cycle.EventType.ChangeToGreen, start_time.AddMilliseconds(1));
                            pcd.NextEvent(Cycle.EventType.ChangeToYellow, start_time.AddMilliseconds(2));
                        }
                    }

                    // check to see if the event is a change to red
                    // the 64 event is for overlaps
                    if (row.EventCode == 10 | row.EventCode == 64)
                    {
                        // if it is red and the pcd group is empty create a new one
                        if (pcd != null)
                            pcd = new Cycle(row.TimeStamp);
                        else
                        {
                            pcd.NextEvent(GetEventType(row.EventCode), row.TimeStamp);
                            // if the next event response is complete add it and start the next group
                            if (pcd.Status == Cycle.NextEventResponse.GroupComplete)
                            {
                                CycleCollection.Add(pcd);
                                pcd = new Cycle(row.TimeStamp);
                            }
                        }
                    }
                    else if (pcd != null)
                    {
                        // if the event is not red and the group is not empty
                        // add the event and set the next event
                        pcd.NextEvent(GetEventType(row.EventCode), row.TimeStamp);
                        if (pcd.Status == Cycle.NextEventResponse.GroupComplete)
                        {
                            CycleCollection.Add(pcd);
                            pcd = new Cycle(row.TimeStamp);
                        }
                    }

                    if (pcd != null & pcd.Status == Cycle.NextEventResponse.GroupMissingData)
                        pcd = null/* TODO Change to default(_) if this is not a reference type */;
                }
                else if (counter == cycleEvents.Count & pcd != null)
                {
                    // if the last event is red create a new group to consume the remaining time in the period
                    if (GetEventType(row.EventCode) == Cycle.EventType.ChangeToRed)
                    {
                        pcd.NextEvent(Cycle.EventType.ChangeToGreen, end_time.AddMilliseconds(-2));
                        pcd.NextEvent(Cycle.EventType.ChangeToYellow, end_time.AddMilliseconds(-1));
                        pcd.NextEvent(Cycle.EventType.ChangeToRed, end_time);
                    }
                    else if (GetEventType(row.EventCode) == Cycle.EventType.ChangeToGreen)
                    {
                        pcd.NextEvent(Cycle.EventType.ChangeToYellow, end_time.AddMilliseconds(-1));
                        pcd.NextEvent(Cycle.EventType.ChangeToRed, end_time);
                    }
                    else if (GetEventType(row.EventCode) == Cycle.EventType.ChangeToYellow)
                        pcd.NextEvent(Cycle.EventType.ChangeToRed, end_time);

                    if (pcd.Status != Cycle.NextEventResponse.GroupMissingData)
                        CycleCollection.Add(pcd);
                }
            }

            // if there are no records at all for the selected time, then the line and counts don't show.
            // this next bit fixes that
            if ((CycleCollection.Count == 0 & start_time != end_time))
            {
                // then we need to make a dummy PCD group
                // then PCD assumes it starts on red
                pcd = new Cycle(start_time);

                // and find out what phase state the controller was in by looking for the next phase event
                // after the end of the plan
                List<SignalPhaseEventsUDOT> eventTable = new List<SignalPhaseEventsUDOT>();
                SignalPhaseEventsUDOT eventBeforePattern = new SignalPhaseEventsUDOT();
                eventBeforePattern = null/* TODO Change to default(_) if this is not a reference type */;

                IEnumerable<ControllerEvent> TempEvents;
                DateTime yesterday;
                yesterday = start_time.AddDays(-1);
                TempEvents = Globals.UDOTEventLog.Where(a => a.SignalID == SignalID & a.TimeStamp >= yesterday & a.TimeStamp <= start_time & a.EventParam == PhaseNumber & (a.EventCode == 1 | a.EventCode == 8 | a.EventCode == 10)).OrderByDescending(a => a.TimeStamp);

                foreach (ControllerEvent x in TempEvents)
                {
                    SignalPhaseEventsUDOT tempRow = new SignalPhaseEventsUDOT();
                    tempRow.EventCode = (byte)x.EventCode;
                    tempRow.EventParam = (byte)x.EventParam;
                    tempRow.TimeStamp = x.TimeStamp;
                    eventTable.Add(tempRow);
                }

                eventTable.OrderByDescending(a => a.TimeStamp);
                eventBeforePattern = eventTable.FirstOrDefault();

                if (eventBeforePattern != null)
                {
                    if (GetEventType(eventBeforePattern.EventCode) == Cycle.EventType.ChangeToRed)
                    {
                        // let it dwell in red (we don't have to do anything)

                        // then add a green phase, a yellow phase and a red phase at the end to complete the cycle
                        pcd.NextEvent(Cycle.EventType.ChangeToGreen, end_time.AddMilliseconds(-2));
                        pcd.NextEvent(Cycle.EventType.ChangeToYellow, end_time.AddMilliseconds(-1));
                        pcd.NextEvent(Cycle.EventType.ChangeToRed, end_time);
                    }
                    else if (GetEventType(eventBeforePattern.EventCode) == Cycle.EventType.ChangeToYellow)
                    {
                        // we were in yellow, though this will probably never happen
                        // we have to add a green to our dummy phase
                        pcd.NextEvent(Cycle.EventType.ChangeToGreen, start_time.AddMilliseconds(1));
                        // then make it dwell in yellow
                        pcd.NextEvent(Cycle.EventType.ChangeToYellow, start_time.AddMilliseconds(2));
                        // then add a red phase at the end to complete the cycle
                        pcd.NextEvent(Cycle.EventType.ChangeToRed, end_time);
                    }
                    else if (GetEventType(eventBeforePattern.EventCode) == Cycle.EventType.ChangeToGreen)
                    {
                        // make it dwell in green
                        pcd.NextEvent(Cycle.EventType.ChangeToGreen, start_time.AddMilliseconds(1));

                        // then add a yellow phase and a red phase at the end to complete the cycle
                        pcd.NextEvent(Cycle.EventType.ChangeToYellow, end_time.AddMilliseconds(-1));
                        pcd.NextEvent(Cycle.EventType.ChangeToRed, end_time);
                    }
                }

                if (pcd.Status == Cycle.NextEventResponse.GroupComplete)
                    // If pcd.EndTime = DateTime.MinValue Then pcd.EndTime = Me.EndTime
                    CycleCollection.Add(pcd);
            }

            AddDetectorData(detectorEvents);
            AddPreemptData(preemptEvents);
        }

        public void LinkPivotAddDetectorData(List<DetectorEventsUDOT> detectorEvents)
        {
            mTotalArrivalOnRed = -1;
            mTotalVolume = -1;
            mTotalGreenTime = -1;
            mTotalArrivalOnGreen = -1;

            foreach (Cycle pcd in CycleCollection)
                pcd.ClearDetectorData();
            AddDetectorData(detectorEvents);
        }

        private void AddDetectorData(List<DetectorEventsUDOT> detector_table)
        {
            mTotalArrivalOnRed = -1;
            mTotalVolume = -1;
            mTotalGreenTime = -1;
            mTotalArrivalOnGreen = -1;

            foreach (DetectorEventsUDOT row in detector_table)
            {
                IEnumerable<Cycle> query;
                query = CycleCollection.Where(a => a.CycleStart < row.Timestamp & a.CycleEnd > row.Timestamp);

                foreach (Cycle pcd in query)
                {
                    DetectorDataPoint ddp = new DetectorDataPoint(pcd.CycleStart, row.Timestamp, pcd.ChangeToGreen, pcd.CycleEnd);
                    pcd.AddDetector(ddp);
                }
            }
        }

        private void AddPreemptData(List<SignalPhaseEventsUDOT> preempt_table)
        {
            foreach (SignalPhaseEventsUDOT row in preempt_table)
            {
                IEnumerable<Cycle> query;
                query = CycleCollection.Where(a => a.CycleStart < row.TimeStamp & a.CycleEnd > row.TimeStamp);

                foreach (Cycle pcd in query)
                {
                    DetectorDataPoint ddp = new DetectorDataPoint(pcd.CycleStart, row.TimeStamp, pcd.ChangeToGreen, pcd.CycleEnd);
                    pcd.AddPreempt(ddp);
                }
            }
        }

        //public void SetSpeedStatistics(int minSpeedFilter)
        //{
        //    List<int> rawSpeeds = new List<int>();

        //    // get the speed hits for the plan
        //    IEnumerable<CycleUDOT> cycles;
        //    cycles = CycleCollection.Where(a => a.CycleStart > mStartTime & a.CycleEnd < mEndTime);

        //    foreach (CycleUDOT cy in cycles)
        //    {
        //        foreach (Speed_EventsUDOT speed in cy.SpeedsForCycle)
        //        {
        //            if (speed.MPH > minSpeedFilter)
        //                rawSpeeds.Add(speed.MPH);
        //        }
        //    }

        //    // find stdev of average
        //    if (rawSpeeds.Count > 0)
        //    {
        //        double rawaverage = rawSpeeds.Average();
        //        AvgSpeed = System.Convert.ToInt32(Math.Round(rawaverage));
        //        StdDevAvgSpeed = System.Convert.ToInt32(Math.Round(Math.Sqrt(rawSpeeds.Average(v => Math.Pow(v - rawaverage, 2)))));
        //    }

        //    // find 85% of raw speeds
        //    rawSpeeds.Sort();
        //    if (rawSpeeds.Count > 3)
        //    {
        //        double EightyFiveIndex = ((rawSpeeds.Count * 85) + 0.5);
        //        int EightyFiveIndexInt = 0;
        //        if ((EightyFiveIndex % 1) == 0)
        //        {
        //            EightyFiveIndexInt = System.Convert.ToInt32(EightyFiveIndex);
        //            this.EightyFifth = rawSpeeds.ElementAt(EightyFiveIndexInt - 2);
        //        }
        //        else
        //        {
        //            double IndexMod = (EightyFiveIndex % 1);
        //            EightyFiveIndexInt = System.Convert.ToInt32(EightyFiveIndex);
        //            int Speed1 = rawSpeeds.ElementAt(EightyFiveIndexInt - 2);
        //            int Speed2 = rawSpeeds.ElementAt(EightyFiveIndexInt - 1);
        //            double RawEightyFifth = (1 - IndexMod) * Speed1 + IndexMod * Speed2;
        //            this.EightyFifth = System.Convert.ToInt32(Math.Round(RawEightyFifth));
        //        }
        //    }
        //}
    }
}
