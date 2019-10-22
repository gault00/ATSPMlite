using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Microsoft.VisualBasic;

namespace ATSPMWatchDog
{

    class Program
    {
        static void Main(string[] args)
        {

            DateTime startTime = DateTime.Today;

            if (args.Length > 0)
            {
                startTime = DateTime.Parse(args[0]);
                WatchDogScan scan = new WatchDogScan(startTime);
                scan.StartScan();
            }
            else
            {
                //testing date - flood
                startTime = new DateTime(2018, 9, 1);
                WatchDogScan scan = new WatchDogScan(startTime);
                scan.StartScan();
            }

        }

        
    }

    public static class Globals
    {
        public static List<ControllerEvent> UDOTEventLog = new List<ControllerEvent>();
        public static List<PlanUDOT> UDOTPlanTable = new List<PlanUDOT>();
        public static List<SplitsUDOT> UDOTSplitsData = new List<SplitsUDOT>();
        public static List<PatternChange> UDOTPatternChange = new List<PatternChange>();
        public static List<Graph_DetectorsUDOT> UDOTGraph_Detectors = new List<Graph_DetectorsUDOT>();
        public static List<Cycle> UDOTCycles = new List<Cycle>();
        public static List<SignalPhaseEventsUDOT> UDOTSignalPhaseEvents = new List<SignalPhaseEventsUDOT>();
        public static int UDOTSignalID;
        public static List<ATSPMsignal> signalsListATSPM = new List<ATSPMsignal>();
    }
}
