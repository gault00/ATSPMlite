using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace ATSPMWatchDog
{

    public class AnalysisPhaseCycleCollectionUDOT
    {
        public List<AnalysisPhaseCycleUDOT> Items = new List<AnalysisPhaseCycleUDOT>();

        public AnalysisPhaseCycleCollectionUDOT(byte phase_number, int signal_id, List<PhaseTerminationEvents> terminationeventstable)
        {
            AnalysisPhaseCycleUDOT Cycle;
            Cycle = null/* TODO Change to default(_) if this is not a reference type */;

            foreach (var row in terminationeventstable)
            {
                if (row.EventCode == 1 & row.EventParam == phase_number)
                    Cycle = new AnalysisPhaseCycleUDOT(signal_id, phase_number, row.TimeStamp);
                if (Cycle != null & row.EventParam == phase_number & (row.EventCode == 4 | row.EventCode == 5 | row.EventCode == 6))
                    Cycle.SetTerminationEvent(row.EventCode);
                if (Cycle != null & row.EventParam == phase_number & row.EventCode == 11)
                {
                    Cycle.SetEndTime(row.TimeStamp);
                    Items.Add(Cycle);
                }
                if (Cycle != null & row.EventParam == phase_number & row.EventCode == 21)
                    Cycle.SetPedStart(row.TimeStamp);
                if (Cycle != null & row.EventParam == phase_number & row.EventCode == 23)
                {
                    if (Cycle.hasPed)
                        Cycle.SetPedEnd(row.TimeStamp);
                }
            }
        }
    }
}