using System;
using System.Collections.Generic;
using System.Linq;

namespace ATSPMWatchDog
{
    public class PlanCollectionUDOT
    {
        public byte Phase { get; }
        public int SignalID { get; }
        public List<PlanUDOT> PlanList { get; } = new List<PlanUDOT>();

        /// <summary>
        /// Default constructor used for PCDs
        /// </summary>
        /// <param name="signal_table"></param>
        /// <param name="detector_table"></param>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <param name="signal_id"></param>
        /// <param name="phase"></param>
        /// <param name="preempt_table"></param>
        public PlanCollectionUDOT(List<SignalPhaseEventsUDOT> signal_table, List<DetectorEventsUDOT> detector_table, DateTime start_date, DateTime end_date, int signal_id, byte phase, List<SignalPhaseEventsUDOT> preempt_table)
        {
            Phase = phase;
            SignalID = signal_id;
            GetPlanCollection(start_date, end_date, signal_id, signal_table, detector_table, preempt_table);
        }

        /// <summary>
        /// Alternate constructor to get just enough plan data to display on a chart
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <param name="signal_id"></param>
        public PlanCollectionUDOT(DateTime start_date, DateTime end_date, int signal_id)
        {
            GetSimplePlanCollection(start_date, end_date, signal_id);
        }

        public void GetPlanCollection(DateTime start_date, DateTime end_date, int signal_id, List<SignalPhaseEventsUDOT> signal_table, List<DetectorEventsUDOT> detector_table, List<SignalPhaseEventsUDOT> preempt_table)
        {
            Globals.UDOTPatternChange.Clear();

            // new v4 code - doesn't work well
            // Dim ds As New PlanBaseUDOT(signal_id, start_date, end_date)

            // For i = 0 To ds.Events.Count - 1
            // 'if this is the last plan then we want the end of the plan
            // 'to coincide with the end of the graph
            // If ds.Events.Count - 1 = i Then
            // If ds.Events(i).TimeStamp <> end_date Then
            // Dim plan As New PlanUDOT(ds.Events(i).TimeStamp, end_date, ds.Events(i).EventParam, signal_table, detector_table, preempt_table, mSignalID, mPhase)
            // AddItem(plan)
            // Else
            // If ds.Events(i).TimeStamp <> ds.Events(i + 1).TimeStamp Then
            // 'else we add the plan with the next plan's timestamp as the end of the plan
            // Dim plan As New PlanUDOT(ds.Events(i).TimeStamp, ds.Events(i + 1).TimeStamp, ds.Events(i).EventParam, signal_table, detector_table, preempt_table, mSignalID, mPhase)
            // AddItem(plan)
            // End If

            // End If
            // End If
            // Next


            IEnumerable<ControllerEvent> tempPattern;
            tempPattern = Globals.UDOTEventLog.Where(a => a.TimeStamp >= start_date & a.TimeStamp <= end_date & a.SignalID == signal_id & a.EventCode == 131).OrderBy(a => a.TimeStamp);

            // 'assume free at midnight for now
            // Dim firstpattern As New PatternChangeUDOT
            // firstpattern.TimeStamp = start_date
            // firstpattern.EventParam = 254

            // UDOTPatternChange.Add(firstpattern)

            foreach (ControllerEvent y in tempPattern)
            {
                PatternChange z = new PatternChange();
                z.EventParam = y.EventParam;
                z.TimeStamp = y.TimeStamp;
                // ignore unknown plan starting at midnight - this will be assumed to be free
                if (!(y.EventParam == 0 & y.TimeStamp == start_date))
                    Globals.UDOTPatternChange.Add(z);       // updated 3/24/19 to fix logic
            }

            // check for plan at start time
            int startPatterns = Globals.UDOTPatternChange.Where(a => Math.Abs((a.TimeStamp - start_date).TotalMinutes) <= 1).Count();

            if (startPatterns == 0)
                // assume free to start if there are no other plans then
                Globals.UDOTPatternChange.Add(new PatternChange() { EventParam = 254, TimeStamp = start_date });


            // Added 9/29/18 to override running plan with flash
            List<ControllerEvent> flashPatterns = Globals.UDOTEventLog.Where(a => a.TimeStamp >= start_date & a.TimeStamp <= end_date & a.SignalID == signal_id & a.EventCode == 173).OrderBy(a => a.TimeStamp).ToList();

            foreach (var fp in flashPatterns)
            {
                if (fp.EventParam == 2)
                {
                    if (fp.TimeStamp.Hour != 0 & fp.TimeStamp.Minute != 0)
                    {
                        // exit flash and return to normal operation
                        // lookup previous plan (can't be flash plan, which can happen if it's alternating in and out of flash)
                        byte previousPlan = Globals.UDOTPatternChange.Where(a => a.TimeStamp < fp.TimeStamp & a.EventParam != 255).OrderByDescending(a => a.TimeStamp).Select(a => a.EventParam).FirstOrDefault();
                        PatternChange z = new PatternChange();
                        z.EventParam = previousPlan;
                        z.TimeStamp = fp.TimeStamp;
                        Globals.UDOTPatternChange.Add(z);

                        // remove plan entries while it's in flash
                        ControllerEvent startFlash = flashPatterns.Where(a => a.TimeStamp < fp.TimeStamp & a.EventParam != 2).OrderByDescending(a => a.TimeStamp).FirstOrDefault();
                        if (startFlash == null)
                            startFlash = new ControllerEvent(start_date, 1);
                        else
                        {
                        }
                        List<PatternChange> removeEvents = Globals.UDOTPatternChange.Where(a => a.TimeStamp >= startFlash.TimeStamp & a.TimeStamp < fp.TimeStamp & a.EventParam != 255).ToList();
                        foreach (var e in removeEvents)
                            Globals.UDOTPatternChange.Remove(e);
                    }
                }
                else
                {
                    // controller started in flash
                    PatternChange z = new PatternChange();
                    z.EventParam = 255;
                    z.TimeStamp = fp.TimeStamp;
                    Globals.UDOTPatternChange.Add(z);
                }
            }

            // Override manual control - added 9/30/18
            List<ControllerEvent> manualControl = Globals.UDOTEventLog.Where(a => a.TimeStamp >= start_date & a.TimeStamp <= end_date & a.SignalID == signal_id & a.EventCode == 178).OrderBy(a => a.TimeStamp).ToList();

            foreach (var mc in manualControl)
            {
                if (mc.EventParam == 0)
                {
                    // end of manual control, find when it started
                    // look up previous plan
                    byte previousPlan = Globals.UDOTPatternChange.Where(a => a.TimeStamp < mc.TimeStamp & a.EventParam != 253).OrderByDescending(a => a.TimeStamp).Select(a => a.EventParam).FirstOrDefault();
                    PatternChange z = new PatternChange();
                    z.EventParam = previousPlan;
                    z.TimeStamp = mc.TimeStamp;
                    Globals.UDOTPatternChange.Add(z);

                    // remove plan entries while it's in manual control
                    ControllerEvent startManual = manualControl.Where(a => a.TimeStamp < mc.TimeStamp & a.EventParam == 0).OrderByDescending(a => a.TimeStamp).FirstOrDefault();
                    if (startManual == null)
                        startManual = new ControllerEvent(start_date, 1);
                    List<PatternChange> removeEvents = Globals.UDOTPatternChange.Where(a => a.TimeStamp >= mc.TimeStamp & a.TimeStamp < mc.TimeStamp & a.EventParam != 253).ToList();
                    foreach (var e in removeEvents)
                        Globals.UDOTPatternChange.Remove(e);
                }
                else
                {
                    // controller started in manual control
                    PatternChange z = new PatternChange();
                    z.EventParam = 253;
                    z.TimeStamp = mc.TimeStamp;
                    Globals.UDOTPatternChange.Add(z);
                }
            }

            // sort the list
            Globals.UDOTPatternChange = Globals.UDOTPatternChange.OrderBy(a => a.TimeStamp).ToList();

            // remove duplicate plan entries
            int x = -1;
            // Dim i As Integer = 0    'use to determine index 
            // For Each PCrow As PatternChangeUDOT In UDOTPatternChange
            // If x = -1 Then
            // x = PCrow.EventParam
            // i += 1
            // Continue For
            // ElseIf x <> PCrow.EventParam Then
            // x = PCrow.EventParam
            // i += 1
            // Continue For
            // ElseIf x = PCrow.EventParam Then
            // x = PCrow.EventParam
            // 'UDOTPatternChange.RemoveAt(i)
            // i += 1
            // Continue For
            // 'UDOTPatternChange.Remove(PCrow)
            // End If
            // i += 1
            // Next

            for (int i = Globals.UDOTPatternChange.Count - 1; i >= 0; i += -1)
            {
                if (x == -1)
                {
                    x = Globals.UDOTPatternChange[i].EventParam;
                    continue;
                }
                else if (x != Globals.UDOTPatternChange[i].EventParam)
                {
                    x = Globals.UDOTPatternChange[i].EventParam;
                    continue;
                }
                else if (x == Globals.UDOTPatternChange[i].EventParam)
                {
                    x = Globals.UDOTPatternChange[i].EventParam;
                    Globals.UDOTPatternChange.RemoveAt(i + 1);
                    continue;
                }
            }

            for (int i = 0; i <= Globals.UDOTPatternChange.Count - 1; i++)
            {
                // if this is the last plan then we want the end of the plan
                // to coincide with the end of the graph
                if (Globals.UDOTPatternChange.Count() - 1 == i)
                {
                    if (Globals.UDOTPatternChange[i].TimeStamp != end_date)
                    {
                        PlanUDOT plan = new PlanUDOT(Globals.UDOTPatternChange[i].TimeStamp, end_date, Globals.UDOTPatternChange[i].EventParam, signal_table, detector_table, preempt_table, SignalID, Phase);
                        AddItem(plan);
                    }
                }
                else if (Globals.UDOTPatternChange[i].TimeStamp != Globals.UDOTPatternChange[i + 1].TimeStamp)
                {
                    PlanUDOT plan = new PlanUDOT(Globals.UDOTPatternChange[i].TimeStamp, Globals.UDOTPatternChange[i + 1].TimeStamp, Globals.UDOTPatternChange[i].EventParam, signal_table, detector_table, preempt_table, SignalID, Phase);
                    AddItem(plan);
                }
            }
        }


        // Link pivot add detetor data

        public void GetSimplePlanCollection(DateTime start_date, DateTime end_date, int signal_id)
        {
            Globals.UDOTPatternChange.Clear();
            IEnumerable<ControllerEvent> tempPattern;
            tempPattern = Globals.UDOTEventLog.Where(a => a.TimeStamp >= start_date & a.TimeStamp <= end_date & a.SignalID == signal_id & a.EventCode == 131).OrderBy(a => a.TimeStamp);

            // 'assume free at midnight for now
            // Dim firstpattern As New PatternChangeUDOT
            // firstpattern.TimeStamp = start_date
            // firstpattern.EventParam = 254

            // UDOTPatternChange.Add(firstpattern)

            foreach (ControllerEvent y in tempPattern)
            {
                PatternChange z = new PatternChange();
                z.EventParam = y.EventParam;
                z.TimeStamp = y.TimeStamp;
                // ignore unknown plan starting at midnight - this will be assumed to be free
                if (!(y.EventParam == 0 & y.TimeStamp == start_date))
                    Globals.UDOTPatternChange.Add(z);       // updated 3/24/19 to fix logic
            }

            // check for plan at start time
            int startPatterns = Globals.UDOTPatternChange.Where(a => Math.Abs((a.TimeStamp - start_date).TotalMinutes) <= 1).Count();

            if (startPatterns == 0)
                // assume free to start if there are no other plans then
                Globals.UDOTPatternChange.Add(new PatternChange() { EventParam = 254, TimeStamp = start_date });

            // Added 9/29/18 to override running plan with flash
            List<ControllerEvent> flashPatterns = Globals.UDOTEventLog.Where(a => a.TimeStamp >= start_date & a.TimeStamp <= end_date & a.SignalID == signal_id & a.EventCode == 173).OrderBy(a => a.TimeStamp).ToList();

            foreach (var fp in flashPatterns)
            {
                if (fp.EventParam == 2)
                {
                    if (fp.TimeStamp.Hour != 0 & fp.TimeStamp.Minute != 0)
                    {
                        // exit flash and return to normal operation
                        // lookup previous plan (can't be flash plan, which can happen if it's alternating in and out of flash)
                        byte previousPlan = Globals.UDOTPatternChange.Where(a => a.TimeStamp < fp.TimeStamp & a.EventParam != 255).OrderByDescending(a => a.TimeStamp).Select(a => a.EventParam).FirstOrDefault();
                        PatternChange z = new PatternChange();
                        z.EventParam = previousPlan;
                        z.TimeStamp = fp.TimeStamp;
                        Globals.UDOTPatternChange.Add(z);

                        // remove plan entries while it's in flash
                        ControllerEvent startFlash = flashPatterns.Where(a => a.TimeStamp < fp.TimeStamp & a.EventParam != 2).OrderByDescending(a => a.TimeStamp).FirstOrDefault();
                        if (startFlash == null)
                            startFlash = new ControllerEvent(start_date, 1);
                        List<PatternChange> removeEvents = Globals.UDOTPatternChange.Where(a => a.TimeStamp >= startFlash.TimeStamp & a.TimeStamp < fp.TimeStamp & a.EventParam != 255).ToList();
                        foreach (var e in removeEvents)
                            Globals.UDOTPatternChange.Remove(e);
                    }
                }
                else
                {
                    // controller started in flash
                    PatternChange z = new PatternChange();
                    z.EventParam = 255;
                    z.TimeStamp = fp.TimeStamp;
                    Globals.UDOTPatternChange.Add(z);
                }
            }

            // Override manual control - added 9/30/18
            List<ControllerEvent> manualControl = Globals.UDOTEventLog.Where(a => a.TimeStamp >= start_date & a.TimeStamp <= end_date & a.SignalID == signal_id & a.EventCode == 178).OrderBy(a => a.TimeStamp).ToList();

            foreach (var mc in manualControl)
            {
                if (mc.EventParam == 0)
                {
                    // end of manual control, find when it started
                    // look up previous plan
                    byte previousPlan = Globals.UDOTPatternChange.Where(a => a.TimeStamp < mc.TimeStamp & a.EventParam != 253).OrderByDescending(a => a.TimeStamp).Select(a => a.EventParam).FirstOrDefault();
                    PatternChange z = new PatternChange();
                    z.EventParam = previousPlan;
                    z.TimeStamp = mc.TimeStamp;
                    Globals.UDOTPatternChange.Add(z);

                    // remove plan entries while it's in manual control
                    ControllerEvent startManual = manualControl.Where(a => a.TimeStamp < mc.TimeStamp & a.EventParam == 0).OrderByDescending(a => a.TimeStamp).FirstOrDefault();
                    if (startManual == null)
                        startManual = new ControllerEvent(start_date, 1);
                    List<PatternChange> removeEvents = Globals.UDOTPatternChange.Where(a => a.TimeStamp >= mc.TimeStamp & a.TimeStamp < mc.TimeStamp & a.EventParam != 253).ToList();
                    foreach (var e in removeEvents)
                        Globals.UDOTPatternChange.Remove(e);
                }
                else
                {
                    // controller started in manual control
                    PatternChange z = new PatternChange();
                    z.EventParam = 253;
                    z.TimeStamp = mc.TimeStamp;
                    Globals.UDOTPatternChange.Add(z);
                }
            }

            // sort the list
            Globals.UDOTPatternChange = Globals.UDOTPatternChange.OrderBy(a => a.TimeStamp).ToList();

            // remove duplicate plan entries
            int x = -1;
            // Dim i As Integer = 0    'use to determine index 
            // For Each PCrow As PatternChangeUDOT In UDOTPatternChange
            // If x = -1 Then
            // x = PCrow.EventParam
            // i += 1
            // Continue For
            // ElseIf x <> PCrow.EventParam Then
            // x = PCrow.EventParam
            // i += 1
            // Continue For
            // ElseIf x = PCrow.EventParam Then
            // x = PCrow.EventParam
            // 'UDOTPatternChange.RemoveAt(i)
            // i += 1
            // Continue For
            // 'UDOTPatternChange.Remove(PCrow)
            // End If
            // i += 1
            // Next

            for (int i = Globals.UDOTPatternChange.Count() - 1; i >= 0; i += -1)
            {
                if (x == -1)
                {
                    x = Globals.UDOTPatternChange[i].EventParam;
                    continue;
                }
                else if (x != Globals.UDOTPatternChange[i].EventParam)
                {
                    x = Globals.UDOTPatternChange[i].EventParam;
                    continue;
                }
                else if (x == Globals.UDOTPatternChange[i].EventParam)
                {
                    x = Globals.UDOTPatternChange[i].EventParam;
                    Globals.UDOTPatternChange.RemoveAt(i + 1);
                    continue;
                }
            }

            for (int i = 0; i <= Globals.UDOTPatternChange.Count() - 1; i++)
            {
                // if this is the last plan then we want the end of the plan
                // to coincide with the end of the graph
                if (Globals.UDOTPatternChange.Count() - 1 == i)
                {
                    if (Globals.UDOTPatternChange[i].TimeStamp != end_date)
                    {
                        PlanUDOT plan = new PlanUDOT(Globals.UDOTPatternChange[i].TimeStamp, end_date, Globals.UDOTPatternChange[i].EventParam);
                        AddItem(plan);
                    }
                }
                else if (Globals.UDOTPatternChange[i].TimeStamp != Globals.UDOTPatternChange[i + 1].TimeStamp)
                {
                    PlanUDOT plan = new PlanUDOT(Globals.UDOTPatternChange[i].TimeStamp, Globals.UDOTPatternChange[i + 1].TimeStamp, Globals.UDOTPatternChange[i].EventParam);
                    AddItem(plan);
                }
            }
        }

        public void FillMissingSplits()
        {
            int highestSplit = 0;

            foreach (PlanUDOT plan in PlanList)
            {
                var testSplit = plan.FindHighestRecordedSplitPhase();
                if (highestSplit < testSplit)
                    highestSplit = testSplit;
            }

            foreach (PlanUDOT plan in PlanList)
                plan.FillMissingSplits(highestSplit);
        }

        public void AddItem(PlanUDOT item)
        {
            PlanList.Add(item);
        }
    }
}
