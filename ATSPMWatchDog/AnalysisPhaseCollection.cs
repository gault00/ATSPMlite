using System;
using System.Collections.Generic;
using System.Linq;

namespace ATSPMWatchDog
{
    public class AnalysisPhaseCollectionUDOT
    {
        public List<AnalysisPhaseUDOT> Items = new List<AnalysisPhaseUDOT>();
        public PlanCollectionUDOT Plans;

        public byte MaxPhaseInUse { get; }

        /// <summary>
        ///     ''' Constructor used for termination chart
        ///     ''' </summary>
        ///     ''' <param name="signal_id"></param>
        ///     ''' <param name="start_time"></param>
        ///     ''' <param name="end_time"></param>
        ///     ''' <param name="consecutive_count"></param>
        public AnalysisPhaseCollectionUDOT(int signal_id, DateTime start_time, DateTime end_time, int consecutive_count)
        {
            List<DistinctAnalysisPhasesUDOT> PhasesInUse = new List<DistinctAnalysisPhasesUDOT>();
            List<DistinctAnalysisPhasesUDOT> TempPhasesInUse = new List<DistinctAnalysisPhasesUDOT>();
            IEnumerable<ControllerEvent> DistinctPhases;
            DistinctPhases = Globals.UDOTEventLog.Where(a => a.SignalID == signal_id & a.TimeStamp >= start_time & a.TimeStamp <= end_time & a.EventCode == 1).Distinct();
            foreach (ControllerEvent x in DistinctPhases)
            {
                DistinctAnalysisPhasesUDOT y = new DistinctAnalysisPhasesUDOT();
                y.EventParam = x.EventParam;
                TempPhasesInUse.Add(y);
            }
            // PhasesInUse = TempPhasesInUse.Distinct().ToList()
            PhasesInUse = TempPhasesInUse.GroupBy(x => x.EventParam).Select(x => x.First()).ToList();

            List<PhaseTerminationEvents> TerminationEventsTable = new List<PhaseTerminationEvents>();
            IEnumerable<ControllerEvent> TempTerminationEvents;
            TempTerminationEvents = Globals.UDOTEventLog.Where(a => a.SignalID == signal_id & a.TimeStamp >= start_time & a.TimeStamp <= end_time & (a.EventCode == 1 | a.EventCode == 11 | a.EventCode == 4 | a.EventCode == 5 | a.EventCode == 6 | a.EventCode == 21 | a.EventCode == 23));
            foreach (ControllerEvent x in TempTerminationEvents)
            {
                PhaseTerminationEvents y = new PhaseTerminationEvents();
                y.EventCode = x.EventCode;
                y.EventParam = x.EventParam;
                y.SignalID = x.SignalID;
                y.TimeStamp = x.TimeStamp;
                TerminationEventsTable.Add(y);
            }

            Plans = new PlanCollectionUDOT(start_time, end_time, signal_id);

            foreach (DistinctAnalysisPhasesUDOT row in PhasesInUse)
            {
                AnalysisPhaseUDOT aPhase = new AnalysisPhaseUDOT(row.EventParam, TerminationEventsTable, consecutive_count);
                Items.Add(aPhase);
            }

            MaxPhaseInUse = FindMaxPhase(Items);
        }

        /// <summary>
        ///     ''' Constructor used for split monitor
        ///     ''' </summary>
        ///     ''' <param name="signal_id"></param>
        ///     ''' <param name="start_time"></param>
        ///     ''' <param name="end_time"></param>
        public AnalysisPhaseCollectionUDOT(int signal_id, DateTime start_time, DateTime end_time)
        {
            List<DistinctAnalysisPhasesUDOT> PhasesInUse = new List<DistinctAnalysisPhasesUDOT>();
            List<DistinctAnalysisPhasesUDOT> TempPhasesInUse = new List<DistinctAnalysisPhasesUDOT>();
            IEnumerable<ControllerEvent> DistinctPhases;
            DistinctPhases = Globals.UDOTEventLog.Where(a => a.SignalID == signal_id & a.TimeStamp >= start_time & a.TimeStamp <= end_time & a.EventCode == 1).Distinct();
            foreach (ControllerEvent x in DistinctPhases)
            {
                DistinctAnalysisPhasesUDOT y = new DistinctAnalysisPhasesUDOT();
                y.EventParam = (byte)x.EventParam;
                TempPhasesInUse.Add(y);
            }
            // PhasesInUse = TempPhasesInUse.Distinct().ToList()
            PhasesInUse = TempPhasesInUse.GroupBy(x => x.EventParam).Select(x => x.First()).ToList();

            List<PhaseTerminationEvents> TerminationEventsTable = new List<PhaseTerminationEvents>();
            IEnumerable<ControllerEvent> TempTerminationEvents;
            TempTerminationEvents = Globals.UDOTEventLog.Where(a => a.SignalID == signal_id & a.TimeStamp >= start_time & a.TimeStamp <= end_time & (a.EventCode == 1 | a.EventCode == 11 | a.EventCode == 4 | a.EventCode == 5 | a.EventCode == 6 | a.EventCode == 21 | a.EventCode == 23));
            foreach (ControllerEvent x in TempTerminationEvents)
            {
                PhaseTerminationEvents y = new PhaseTerminationEvents();
                y.EventCode = x.EventCode;
                y.EventParam = x.EventParam;
                y.SignalID = x.SignalID;
                y.TimeStamp = x.TimeStamp;
                TerminationEventsTable.Add(y);
            }

            Plans = new PlanCollectionUDOT(start_time, end_time, signal_id);

            foreach (DistinctAnalysisPhasesUDOT row in PhasesInUse)
            {
                AnalysisPhaseUDOT aPhase = new AnalysisPhaseUDOT(row.EventParam, signal_id, TerminationEventsTable);
                Items.Add(aPhase);
            }
        }

        private byte FindMaxPhase(List<AnalysisPhaseUDOT> Items)
        {
            byte maxPhaseNumber = 0;

            foreach (AnalysisPhaseUDOT phase in Items)
            {
                if (phase.PhaseNumber > maxPhaseNumber)
                    maxPhaseNumber = phase.PhaseNumber;
            }

            return maxPhaseNumber;
        }
    }
}