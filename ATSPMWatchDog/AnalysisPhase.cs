using System.Collections.Generic;
using System.Linq;

namespace ATSPMWatchDog
{
    public class AnalysisPhaseUDOT
    {
        public byte PhaseNumber { get; }
        public int SignalID { get; }
        public double PercentMaxOuts { get; }
        public double PercentForceOffs { get; }
        public int TotalPhaseTerminations { get; }
        public string Direction { get; set; }
        public bool IsOverlap { get; set; }

        public List<ControllerEvent> PedestrianEvents = new List<ControllerEvent>();
        public List<ControllerEvent> TerminationEvents = new List<ControllerEvent>();
        public List<ControllerEvent> ConsecutiveGapOuts = new List<ControllerEvent>();
        public List<ControllerEvent> ConsecutiveMaxOut = new List<ControllerEvent>();
        public List<ControllerEvent> ConsecutiveForceOff = new List<ControllerEvent>();
        public List<ControllerEvent> UnknownTermination = new List<ControllerEvent>();

        public AnalysisPhaseCycleCollectionUDOT Cycles;

        /// <summary>
        /// Constructor used for Phase Termination Chart
        /// </summary>
        /// <param name="phase_number"></param>
        /// <param name="TerminationEventsTable"></param>
        /// <param name="consecutive_count"></param>
        public AnalysisPhaseUDOT(byte phase_number, List<PhaseTerminationEvents> TerminationEventsTable, int consecutive_count)
        {
            PhaseNumber = phase_number;

            IEnumerable<PhaseTerminationEvents> termRow;
            termRow = TerminationEventsTable.Where(d => (d.EventParam == phase_number & (d.EventCode == 4 | d.EventCode == 5 | d.EventCode == 6))).OrderBy(d => d.TimeStamp);
            IEnumerable<PhaseTerminationEvents> pedRow;
            pedRow = TerminationEventsTable.Where(d => d.EventParam == phase_number & (d.EventCode == 21 | d.EventCode == 23)).OrderBy(d => d.TimeStamp);

            foreach (PhaseTerminationEvents row in termRow)
            {
                ControllerEvent tEvent = new ControllerEvent(row.TimeStamp, row.EventCode);
                TerminationEvents.Add(tEvent);
            }

            foreach (PhaseTerminationEvents row in pedRow)
            {
                ControllerEvent tEvent = new ControllerEvent(Globals.UDOTSignalID, row.TimeStamp, row.EventCode, row.EventParam);
                PedestrianEvents.Add(tEvent);
            }

            ConsecutiveGapOuts = FindConsecutiveEvents(TerminationEvents, 4, consecutive_count);
            ConsecutiveMaxOut = FindConsecutiveEvents(TerminationEvents, 5, consecutive_count);
            ConsecutiveForceOff = FindConsecutiveEvents(TerminationEvents, 6, consecutive_count);
            UnknownTermination = FindUnknownTerminationEvents(TerminationEvents);            // updated 10/2/18
                                                                                             // UnknownTermination = FindConsecutiveEvents(TerminationEvents, 7, consecutive_count)
            PercentMaxOuts = FindPercentageConsecutiveEvents(TerminationEvents, 5, consecutive_count);
            PercentForceOffs = FindPercentageConsecutiveEvents(TerminationEvents, 6, consecutive_count);
            TotalPhaseTerminations = TerminationEvents.Count;
        }

        /// <summary>
        ///     ''' Constructor used for split monitor
        ///     ''' </summary>
        ///     ''' <param name="phase_number"></param>
        ///     ''' <param name="signal_id"></param>
        ///     ''' <param name="TerminationEventsTable"></param>
        public AnalysisPhaseUDOT(byte phase_number, int signal_id, List<PhaseTerminationEvents> TerminationEventsTable)
        {
            PhaseNumber = phase_number;
            SignalID = signal_id;
            IsOverlap = false;
            Direction = "Northbound";

            Cycles = new AnalysisPhaseCycleCollectionUDOT(phase_number, signal_id, TerminationEventsTable);

            IEnumerable<Graph_DetectorsUDOT> temp;
            List<Graph_DetectorsUDOT> query = new List<Graph_DetectorsUDOT>();
            temp = Globals.UDOTGraph_Detectors.Where(a => a.SignalID == signal_id);

            foreach (Graph_DetectorsUDOT row in temp)
            {
                if (row.Phase == phase_number)
                    query.Add(row);
            }

            if (query.Count < 1)
                this.Direction = "Unknown";
            else
                this.Direction = query.FirstOrDefault().Direction;
        }

        private List<ControllerEvent> FindConsecutiveEvents(List<ControllerEvent> terminationEvents, byte event_type, int consecutive_count)
        {
            List<ControllerEvent> ConsecutiveEvents = new List<ControllerEvent>();
            int RunningConsecCount = 0;

            // order the events by datestamp
            IEnumerable<ControllerEvent> EventsInOrder;
            EventsInOrder = terminationEvents.OrderBy(d => d.TimeStamp);

            foreach (var termEvent in EventsInOrder)
            {
                if (termEvent.EventCode == event_type)
                    RunningConsecCount += 1;
                else
                    RunningConsecCount = 0;

                if (RunningConsecCount >= consecutive_count)
                    ConsecutiveEvents.Add(termEvent);
            }

            return ConsecutiveEvents;
        }

        // added UDOT v4 - 10/2/18
        private List<ControllerEvent> FindUnknownTerminationEvents(List<ControllerEvent> terminationEvents)
        {
            List<ControllerEvent> unknownTermEvents = new List<ControllerEvent>();
            for (var x = 0; x <= terminationEvents.Count - 2; x++)
            {
                ControllerEvent currentEvent = terminationEvents[x];
                ControllerEvent nextEvent = terminationEvents[x + 1];
                if (currentEvent.EventCode == 7 & nextEvent.EventCode == 7)
                    unknownTermEvents.Add(currentEvent);
            }
            return unknownTermEvents;
        }

        private double FindPercentageConsecutiveEvents(List<ControllerEvent> terminationEvents, byte event_type, int consecutive_count)
        {
            double percentile = 0;
            int total = terminationEvents.Count();

            // get all termination events fo the event type
            int terminationEventsOfType;
            terminationEventsOfType = terminationEvents.Where(d => d.EventCode == event_type).Count();
            if (terminationEvents.Count > 0)
                percentile = terminationEventsOfType / (double)total;
            return percentile;
        }
    }
}

