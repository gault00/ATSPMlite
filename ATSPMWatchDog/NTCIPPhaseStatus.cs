namespace ATSPMWatchDog
{
    public class NTCIPPhaseStatus
    {
        /// <summary>
        /// The Phase StatusGroup number for objects in this row. This value shall not exceed the maxPhaseGroups object value.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int Number { get; set; }

        /// <summary>
        /// Phase Red Output Status Mask, when a bit = 1, the Phase Red is currently active. When a bit = 0, the Phase Red is NOT currently active.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool Red { get; set; }

        /// <summary>
        /// Phase Yellow Output Status Mask, when a bit = 1, the Phase Yellow is currently active. When a bit = 0, the Phase Yellow is NOT currently active.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool Yellow { get; set; }

        /// <summary>
        /// Phase Green Output Status Mask, when a bit = 1, the Phase Green is currently active. When a bit = 0, the Phase Green is NOT currently active.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool Green { get; set; }

        /// <summary>
        ///     ''' Phase Dont Walk Output Status Mask, when a bit = 1, the Phase Dont Walk is currently active. When a bit = 0, the Phase Dont Walk is NOT currently active.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public bool DontWalk { get; set; }

        /// <summary>
        /// Phase Ped Clear Output Status Mask, when a bit = 1, the Phase Ped Clear is currently active. When a bit = 0, the Phase Ped Clear is NOT currently active.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool PedClear { get; set; }

        /// <summary>
        /// Phase Walk Output Status Mask, when a bit = 1, the Phase Walk is currently active. When a bit = 0, the Phase Walk is NOT currently active.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool Walk { get; set; }

        /// <summary>
        /// Phase Vehicle Call Status Mask, when a bit = 1, the Phase vehicle currently has a call for service. When a bit = 0, the Phase vehicle currently does NOT have a call for service.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool VehicleCall { get; set; }

        /// <summary>
        /// Phase Pedestrian Call Status Mask, when a bit = 1, the Phase pedestrian currently has a call for service.
        /// When a bit = 0, the Phase pedestrian currently does NOT have a call for service.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool PedestrianCall { get; set; }

        /// <summary>
        /// Phase On Status Mask, when a bit = 1, the Phase is currently active.
        /// When a bit = 0, the Phase currently is NOT active.
        /// The phase is ON during the Green, Yellow, and Red Clearance intervals of that phase.
        /// It shall be permissible for this status to be True (bit=1) during the Red Dwell state.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool PhaseOn { get; set; }

        /// <summary>
        /// Phase Next Status Mask, when a bit = 1, the Phase currently is committed to be NEXT in sequence and remains present until the phase becomes active (On/Timing). 
        /// When a bit = 0, the Phase currently is NOT committed to be NEXT in sequence. 
        /// The phase next to be serviced shall be determined at the end of the green interval of the terminating phase; 
        /// except that if the decision cannot be made at the end of the Green interval, it shall not be made until after the end of all Vehicle Change and Clearance intervals.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool PhaseNext { get; set; }

        public NTCIPPhaseStatus()
        {
            // Dim i As Integer

            Number = 0;
            Red = true;
        }
    }

}
