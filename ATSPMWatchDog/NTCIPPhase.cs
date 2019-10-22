namespace ATSPMWatchDog
{
    public class NTCIPPhase : NTCIPBase
    {
        public enum phaseStartupType
        {
            other = 1,
            phaseNotOn = 2,
            greenWalk = 3,
            greenNoWalk = 4,
            yellowChange = 5,
            redClear = 6
        }

        /// <summary>
        /// The phase number for objects in this row. This value shall not exceed the maxPhases object value.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public byte Number { get; set; }

        /// <summary>
        /// Phase Walk Parameter in seconds. This shall control the amount of time the Walk indication shall be displayed.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public byte Walk { get; set; }

        /// <summary>
        /// Phase Pedestrian Clear Parameter in seconds. This shall control the duration of the Pedestrian Clearance output (if present) and the flashing period of the Don’t Walk output.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public byte PedestrianClear { get; set; }

        /// <summary>
        /// Phase Minimum Green Parameter in seconds (NEMA TS 2 range: 1-255 sec). The first timed portion of the Green interval which may be set in consideration of the storage of vehicles between the zone of detection for the approach vehicle detector(s) and the stop line.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public byte MinimumGreen { get; set; }

        private byte m_Passage;
        /// <summary>
        /// Phase Passage Parameter in tenth seconds (0-25.5 sec). Passage Time, Vehicle Interval, Preset Gap, Vehicle Extension: the extensible portion of the Green shall be a function of vehicle actuations that occur during the Green interval. The phase shall remain in the extensible portion of the Green interval as long as the passage timer is not timed out. The timing of this portion of the green interval shall be reset with each subsequent vehicle actuation and shall not commence to time again until the vehicle actuation is removed.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double Passage
        {
            get
            {
                return System.Convert.ToDouble(m_Passage / 10.0d);
        }
            set
            {
                m_Passage = System.Convert.ToByte(value * 10.0); // stores value as integer with tenths
        }
        }

        /// <summary>
        /// Phase Maximum 1 Parameter in seconds (NEMA TS 2 range: 1-255 sec). This time setting shall determine the maximum length of time this phase may be held Green in the presence of a serviceable conflicting call. In the absence of a serviceable conflicting call the Maximum Green timer shall be held reset unless Max Vehicle Recall is enabled for this phase. This is the default maximum value to use. It may be overridden via an external input, coordMaximumMode or other method.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public byte Max1 { get; set; }

        /// <summary>
        /// Phase Maximum 2 Parameter in seconds (NEMA TS 2 range: 1-255 sec). This time setting shall determine the maximum length of time this phase may be held Green in the presence of a serviceable conflicting call. In the absence of a serviceable conflicting call the Maximum Green timer shall be held reset unless Max Vehicle Recall is enabled for this phase. This may be implemented as the max green timer via an external input, coordMaximumMode or other method.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public byte Max2 { get; set; }

        private byte m_YellowChange;   // tenth of second
        /// <summary>
        /// Phase Yellow Change Parameter in tenth seconds (NEMA TS 2 range: 3-25.5 sec). Following the Green interval of each phase the CU shall provide a Yellow Change interval which is timed according to the Yellow Change parameter for that phase.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double YellowChange
        {
            get
            {
                return m_YellowChange / 10.0;
        }
            set
            {
                m_YellowChange = System.Convert.ToByte(value * 10.0);     // stores value as integer with tenths
        }
        }

        private byte m_RedClear;       // tenth of second
        /// <summary>
        /// Phase Red Clearance Parameter in tenth seconds (0-25.5 sec).Following the Yellow Change interval for each phase, the CU shall provide a Red Clearance interval which is timed according to the Red Clearance parameter for that phase.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double RedClear
        {
            get
            {
                return m_RedClear / 10.0;
        }
            set
            {
                m_RedClear = System.Convert.ToByte(value * 10.0);
        }
        }

        private byte m_RedRevert;
        /// <summary>
        /// Red revert time parameter in tenth seconds . A minimum Red indication to be timed following the 
        /// Yellow Change interval and prior to the next display of Green on the same signal output driver group.
        /// The unitRedRevert parameter shall act as a minimum red revert time for all signal displays. 
        /// The phaseRedRevert parameter may increase the red revert time for a specific phase. 
        /// If the phaseRedRevert parameter is less than the unitRedRevert the unitRedRevert time shall be used.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double RedRevert
        {
            get
            {
                return m_RedRevert / 10.0;
        }
            set
            {
                m_RedRevert = System.Convert.ToByte(value * 10.0);
        }
        }


        private byte m_AddedInitial;   // tenth of second    'aka seconds per actuation
        /// <summary>
        /// Phase Added Initial Parameter in tenths of seconds (0-25.5 sec). Added Initial parameter (Seconds / Actuation) shall determine the time by which the variable initial time period will be increased from zero with each vehicle actuation received during the associated phase Yellow and Red intervals.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double AddedInitial
        {
            get
            {
                return m_AddedInitial / 10.0;
        }
            set
            {
                m_AddedInitial = System.Convert.ToByte(value * 10.0);
        }
        }

        /// <summary>
        /// Phase Maximum Initial Parameter in seconds (0-255 sec). The maximum value of the variable initial timing period. Variable Initial timing shall equal the lesser of [added initial (seconds / actuation) * number of actuations] or [ Max Initial ]. The variable initial time shall not be less than Minimum Green.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public byte MaximumInitial { get; set; }

        /// <summary>
        /// Phase Time Before Reduction (TBR) Parameter in seconds (0-255 sec). The Time Before Reduction period shall begin when the phase is Green and there is a serviceable conflicting call. If the serviceable conflicting call is removed before completion of this time (or time to reduce), the timer shall reset. Upon completion of the TBR period or the CarsBeforeReduction (CBR) parameter is satisfied, whichever occurs first, the linear reduction of the allowable gap from the Passage Time shall begin.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public byte TimeBeforeReduction { get; set; }

        /// <summary>
        /// Phase Cars Before Reduction (CBR) Parameter (0-255 vehicles). When the phase is Green and the sum of the cars waiting (vehicle actuations during Yellow and Red intervals) on serviceable conflicting phases equals or exceeds the CBR parameter or the Time Before Reduction (TBR) parameter is satisfied, whichever occurs first, the linear reduction of the allowable gap from the Passage Time shall begin.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public byte CarsBeforeReduction { get; set; }

        /// <summary>
        /// Phase Time To Reduce Parameter in seconds (0-255 sec). This parameter shall control the rate of reduction of the allowable gap between the Passage Time and Minimum Gap setting.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public byte TimeToReduce { get; set; }

        private byte m_ReduceBy;
        /// <summary>
        /// This object may be used for volume density gap reduction as an alternate to the linear reduction defined by NEMA TS 1 and TS 2. It contains the tenths of seconds to reduce the gap by (0.0 - 25.5 seconds). The frequency of reduction shall produce the Minimum Gap after a time equal to the 'phaseTimeToReduce’ object.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double ReduceBy
        {
            get
            {
                return m_ReduceBy / 10.0;
        }
            set
            {
                m_ReduceBy = System.Convert.ToByte(value * 10.0);
        }
        }

        private byte m_MinimumGap;     // tenth of second
        /// <summary>
        /// Phase Minimum Gap Parameter in tenth seconds (0-25.5 sec). The reduction of the allowable gap shall continue until the gap reaches a value equal to or less than the minimum gap as set on the Minimum Gap control after which the allowable gap shall remain fixed at the values set on the Minimum Gap control.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double MinimumGap
        {
            get
            {
                return m_MinimumGap / 10.0;
        }
            set
            {
                m_MinimumGap = System.Convert.ToByte(value * 10.0);
        }
        }

        /// <summary>
        /// This object shall determine either the upper or lower limit of the running max in seconds (0-255 sec) during dynamic max operation.
        /// The normal maximum (i.e. Max1, Max2, etc.) shall determine the other limit as follows:
        /// When dynamicMaxLimit is larger than the normal maximum, it shall become the upper limit.
        /// When dynamicMaxLimit is smaller than the normal maximum, it shall become the lower limit.
        /// Setting dynamicMaxLimit greater than zero enables dynamic max operation with the normal maximum used as the initial maximum setting. See dynamicMaxStep for details on dynamic max operation.
        /// Maximum recall or a failed detector that is assigned to the associated phase shall disable dynamic max operation for the phase.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public byte DynamicMaxLimit { get; set; }

        private byte m_DynamicMaxStep;
        /// <summary>
        /// This object shall determine the automatic adjustment to the running max in tenth seconds (0-25.5)
        /// When a phase maxes out twice in a row, and on each successive max out thereafter, one dynamic max step value shall be added to the running max until such addition would mean the running max was greater than the larger of normal max or dynamic max limit.
        /// When a phase gaps out twice in a row, and on each successive gap out thereafter, one dynamic max step value shall be subtracted from the running max until such subtraction would mean the running max was less than the smaller of the normal max or the dynamic max limit.
        /// If a phase gaps out in one cycle and maxes out in the next cycle, or vice versa, the running max will not change.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double DynamicMaxStep
        {
            get
            {
                return m_DynamicMaxStep / 10.0;
        }
            set
            {
                m_DynamicMaxStep = System.Convert.ToByte(value * 10.0);
        }
        }

        /// <summary>
        /// The Phase Startup parameter is an enumerated integer which selects the startup state for each phase after restoration of a defined power interruption or activation of the external start input. The following entries are defined:
        /// other: this phase is not enabled (phaseOptions bit 0=0 or phaseRing=0) or initializes in a state not defined by this standard.
        /// phaseNotOn: this phase initializes in a Red state (the phase is not active and no intervals are timing).
        /// greenWalk: this phase initializes at the beginning of the minimum green and walk timing intervals.
        /// greenNoWalk: this phase initializes at the beginning of the minimum green timing interval.
        /// yellowChange: this phase initializes at the beginning of the Yellow Change interval.
        /// redClear: this phase initializes at the beginning of the Red Clearance interval.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public phaseStartupType Startup { get; set; }

        private int m_PhaseOptions;
        /// <summary>
        /// Optional phase functions (boolean based on individual 16 bits in the integer)
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int PhaseOptions
        {
            get
            {
                m_PhaseOptions = CreateBit16(Enabled, AutomaticFlashEntry, AutomaticFlashExit, NonActuated1, NonActuated2, NonLock, MinVehicleRecall, MaxVehicleRecall, PedRecall, SoftVehicleRecall, DualEntry, SimultaneousGap, GuaranteedPassage, ActuatedRestInWalk, ConditionalServiceEnable, AddedInitialCalculation);
                return m_PhaseOptions;
            }
            set
            {
                m_PhaseOptions = value;
                AddedInitialCalculation = GetBit16(value, 15);
                ConditionalServiceEnable = GetBit16(value, 14);
                ActuatedRestInWalk = GetBit16(value, 13);
                GuaranteedPassage = GetBit16(value, 12);
                SimultaneousGap = GetBit16(value, 11);
                DualEntry = GetBit16(value, 10);
                SoftVehicleRecall = GetBit16(value, 9);
                PedRecall = GetBit16(value, 8);
                MaxVehicleRecall = GetBit16(value, 7);
                MinVehicleRecall = GetBit16(value, 6);
                NonLock = GetBit16(value, 5);
                NonActuated2 = GetBit16(value, 4);
                NonActuated1 = GetBit16(value, 3);
                AutomaticFlashExit = GetBit16(value, 2);
                AutomaticFlashEntry = GetBit16(value, 1);
                Enabled = GetBit16(value, 0);
            }
        }

        /// <summary>
        /// If set (1) the CU shall compare counts from all associated AddedInitial detectors and use the largest count value for the calculations. If clear (0) the CU shall sum all associated AddedInitial detector counts and use this sum for the calculations. The ability to modify the setting of this bit is optional.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool AddedInitialCalculation { get; set; }

        /// <summary>
        /// in multi-ring configurations when set to 1 causes a gapped/maxed phase to conditionally service a preceding actuated vehicle phase when sufficient time remains before max time out of the phase(s) not prepared to terminate. Support is optional.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool ConditionalServiceEnable { get; set; }

        /// <summary>
        /// when set to 1 causes an actuated phase to rest in Walk when there is no serviceable conflicting call at the end of Walk Timing.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool ActuatedRestInWalk { get; set; }

        /// <summary>
        /// when set to 1 enables an actuated phase operating in volume density mode (using gap reduction) to retain the right of way for the unexpired portion of the Passage time following the decision to terminate the green due to a reduced gap. Support is optional
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool GuaranteedPassage { get; set; }

        private bool m_SimultaneousGapDisable;
        /// <summary>
        /// in multi-ring configurations when set to 1 disables a gapped out phase from reverting to the extensible portion. Support is optional
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool SimultaneousGap
        {
            get
            {
                return !m_SimultaneousGapDisable;
            }
            set
            {
                m_SimultaneousGapDisable = !value;    // NTCIP defines as opposite of common sense
            }
        }

        /// <summary>
        /// in multi-ring configurations when set to 1 causes the phase to become active upon entry into a concurrency group (crossing a barrier) when no calls exist in its ring within its concurrency group.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool DualEntry { get; set; }

        /// <summary>
        /// when set to 1 causes a call on a phase when all conflicting phases are in green dwell or red dwell and there are no serviceable conflicting calls. Support is optional.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool SoftVehicleRecall { get; set; }

        /// <summary>
        /// when set to 1 causes a recurring pedestrian demand which shall function in the same manner as an external pedestrian call except that it shall not recycle the pedestrian service until a conflicting phase is serviced
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool PedRecall { get; set; }

        /// <summary>
        /// when set to 1 causes a call on a phase such that the timing of the Green interval for that phase shall be extended to Maximum Green time.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool MaxVehicleRecall { get; set; }

        /// <summary>
        /// when set to 1 causes recurring demand for vehicle service on the phase when that phase is not in its Green interval.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool MinVehicleRecall { get; set; }

        /// <summary>
        /// when set to 0 will cause the call to be locked at the beginning of the yellow interval. When set to 1 call locking will depend on the detectorOptions object.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool NonLock { get; set; }

        /// <summary>
        /// when set to 1 causes a phase to respond to the Call To Non-Actuated 2 input (if present) or other method. Support is optional
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool NonActuated2 { get; set; }

        /// <summary>
        /// when set to 1 causes a phase to respond to the Call To Non-Actuated 1 input (if present) or other method. Support is optional
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool NonActuated1 { get; set; }

        /// <summary>
        /// The CU shall move immediately to the beginning of the phase(s) programmed as Exit Phase(s) when Automatic Flash terminates. Support is optional
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool AutomaticFlashExit { get; set; }

        /// <summary>
        /// When Automatic Flash is called, the CU shall service the Entry Phase(s), clear to an All Red, then initiate flashing operation. Support is optional.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool AutomaticFlashEntry { get; set; }

        /// <summary>
        /// provide a means to define whether this phase is used in the current configuration. 
        /// A disabled phase shall not provide any outputs nor respond to any phase inputs. 
        /// The object phaseRing = 0 has the same effect.
        /// Econolite term: In Use
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool Enabled {
            // Econolite: "In Use"
            get; set; }

        //Synchro barrier
        public byte Barrier { get; set; }

        // Synchro ring number
        private byte m_Ring;          
        /// <summary>
        /// Phase ring number (1..maxRings) that identified the ring which contains the associated phase. This value must not exceed the maxRings object value. If the ring number is zero, the phase is disabled (phaseOptions Bit 0 = 0 has the same effect).
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public byte Ring
        {
            get
            {
                return m_Ring;
            }
            set
            {
                m_Ring = value;
                if (value == 0)
                    Enabled = false;
            }
        }

        //Synchro ring position
        public byte RingPosition { get; set; }

        /// <summary>
        /// Each octet contains a phase number (binary value) that may run concurrently with the associated phase. Phases that are contained in the same ring may NOT run concurrently.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Concurrency { get; set; }


        // constructor
        public NTCIPPhase()
        {
            Number = 0;
            Walk = 0;
            PedestrianClear = 16;
            MinimumGreen = 5;
            Passage = 3;
            Max1 = 35;
            Max2 = 40;
            YellowChange = 3;
            RedClear = 1;
            AddedInitial = 0;
            MaximumInitial = 30;
            TimeBeforeReduction = 0;
            TimeToReduce = 0;
            MinimumGap = 0;
            DynamicMaxLimit = 0;
            DynamicMaxStep = 0;
            AddedInitialCalculation = true;
            ConditionalServiceEnable = false;
            ActuatedRestInWalk = false;
            GuaranteedPassage = false;
            SimultaneousGap = false;
            DualEntry = false;
            SoftVehicleRecall = false;
            PedRecall = false;
            MaxVehicleRecall = false;
            MinVehicleRecall = false;
            NonLock = true;
            NonActuated2 = false;
            NonActuated1 = false;
            AutomaticFlashExit = false;
            AutomaticFlashEntry = false;
            Enabled = false;
            Barrier = 0;
            Ring = 0;
            RingPosition = 0;
        }

        // events

        // functions
        public void DefaultVolumeDensity()
        {
            // set phases 2 and 6 to default volume density parameters
            if (Number == 2 | Number == 6)
            {
                MinimumGap = 3;
                AddedInitial = 2;
                MinVehicleRecall = true;
                Passage = 6;
                TimeBeforeReduction = 25;
                TimeToReduce = 13;
                AutomaticFlashEntry = true;
                AutomaticFlashExit = true;
            }
        }
    }
}
