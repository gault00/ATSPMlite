using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Net.Mail;
using System.Collections;

namespace ATSPMWatchDog
{
    class WatchDogScan
    {
        List<SPMWatchDogErrorEvent> ForceOffErrors = new List<SPMWatchDogErrorEvent>();
        List<SPMWatchDogErrorEvent> LowHitCountErrors = new List<SPMWatchDogErrorEvent>();
        List<SPMWatchDogErrorEvent> MaxOutErrors = new List<SPMWatchDogErrorEvent>();
        List<SPMWatchDogErrorEvent> MissingRecords = new List<SPMWatchDogErrorEvent>();
        List<SPMWatchDogErrorEvent> CannotFtpFiles = new List<SPMWatchDogErrorEvent>();
        List<SPMWatchDogErrorEvent> RecordsFromTheDayBefore = new List<SPMWatchDogErrorEvent>();
        List<int> SignalsNoRecords = new List<int>();
        List<int> SignalsWithRecords = new List<int>();
        List<SPMWatchDogErrorEvent> StuckPedErrors = new List<SPMWatchDogErrorEvent>();
        List<SPMWatchDogErrorEvent> Alarms = new List<SPMWatchDogErrorEvent>();

        WatchDogApplicationSettings Settings { get; set; }
        DateTime ScanDate { get; set; }
        DateTime PreviousDate { get; set; }
        int ErrorCount { get; set; }

        private static byte[] _ZlibHeaderNoCompression = { 120, 1 };
        private static byte[] _ZlibHeaderDefaultCompression = { 120, 156 };
        private static byte[] _ZlibHeaderBestCompression = { 120, 218 };
        private static byte[] _GZipHeader = { 31, 139 };

        public WatchDogScan(DateTime scanDate)
        {
            ScanDate = scanDate;
            Settings = new WatchDogApplicationSettings();
            if (Settings.WeekdayOnly && scanDate.DayOfWeek == DayOfWeek.Monday)
            {
                PreviousDate = scanDate.AddDays(-3).Date;
            }
            else
            {
                PreviousDate = scanDate.AddDays(-1).Date;
            }
        }

        public void StartScan()
        {
            if (!Settings.WeekdayOnly || Settings.WeekdayOnly && ScanDate.DayOfWeek != DayOfWeek.Saturday && ScanDate.DayOfWeek != DayOfWeek.Sunday)
            {
                List<SPMWatchDogErrorEvent> watchDogErrorEventRepository = new List<SPMWatchDogErrorEvent>();

                //Load Signal Configuration
                FileInfo finfo2 = new FileInfo(Properties.Settings.Default.SignalConfigFile);
                if (finfo2.Exists)
                {
                    StreamReader openFile = new StreamReader(Properties.Settings.Default.SignalConfigFile);
                    JsonTextReader reader = new JsonTextReader(openFile);
                    string jsonText = openFile.ReadToEnd();
                    Globals.signalsListATSPM = JsonConvert.DeserializeObject<List<ATSPMsignal>>(jsonText);
                    openFile.Close();
                }
                else
                {
                    Console.WriteLine("Invalid signal configuration file. Please select a new file in application settings.");
                    Console.ReadLine();
                }

                foreach (ATSPMsignal signal in Globals.signalsListATSPM)
                {
                    Globals.UDOTSignalID = signal.SignalID;

                    //import signal configuration
                    ATSPMsignal currentSignalConfig;
                    currentSignalConfig = signal;
                    Globals.UDOTGraph_Detectors.Clear();

                    //first determine if there are any detectors in new format by approach and convert them to the old format
                    foreach (ATSPMapproach app in currentSignalConfig.Approaches)
                    {
                        foreach (var approachDetector in app.Detectors)
                        {
                            bool hasPCD = false;
                            byte detPhase = app.ProtectedPhaseNumber;
                            bool hasSplitFail = false;
                            if (approachDetector.DetectionTypes.Contains(ATSPMdetectionType.AdvancedCount))
                            {
                                hasPCD = true;
                                if (app.PermissivePhaseNumber > 0)
                                    detPhase = (byte)app.PermissivePhaseNumber;

                            }
                            if (approachDetector.DetectionTypes.Contains(ATSPMdetectionType.StopBarPresence))
                                hasSplitFail = true;
                            Graph_DetectorsUDOT graphDet = new Graph_DetectorsUDOT()
                            {
                                DetectorID = approachDetector.DetectorID,
                                SignalID = app.SignalID,
                                Lane = app.Description + " " + approachDetector.MovementType,
                                Phase = detPhase,
                                Direction = app.DirectionTypeID.Abbreviation,
                                Det_Channel = approachDetector.DetChannel,
                                DistanceFromStopBar = approachDetector.DistanceFromStopBar,
                                MPH = (int)app.MPH,
                                Has_PCD = hasPCD,
                                Has_SplitFail = hasSplitFail,
                                Perm_Phase = (byte)app.PermissivePhaseNumber,
                                Is_Overlap = app.IsProtectedPhaseOverlap,
                                Date_Added = approachDetector.DateAdded
                            };
                            Globals.UDOTGraph_Detectors.Add(graphDet);

                            //set the approach to refer back
                            approachDetector.Approach = app;
                            if (approachDetector.LatencyCorrection == null)
                                approachDetector.LatencyCorrection = 0;
                        }
                    }

                    //if no detectors in new format were found, then look for old format in hte config file and add thm
                    if (Globals.UDOTGraph_Detectors.Count == 0)
                        Globals.UDOTGraph_Detectors = currentSignalConfig.Detectors;

                    string SearchDirectory = Properties.Settings.Default.DATfolder + Path.DirectorySeparatorChar + signal.SignalID;

                    if (Directory.Exists(SearchDirectory))
                    {
                        string FileTypes = "*.dat|*.datZ";
                        string OriginalFileName;
                        DateTime IntervalStart;

                        Globals.UDOTEventLog.Clear();
                        //clear UDOTSplitsData, UDOTPatternChange, UDOTPlanTable

                        foreach (var datFile in getFiles(SearchDirectory, FileTypes, SearchOption.TopDirectoryOnly))
                        {
                            string[] OriginalFilenameWords;
                            string FileType, FileExt, OriginalDatFile;
                            OriginalDatFile = datFile;
                            OriginalFileName = Path.GetFileName(datFile);
                            FileExt = Path.GetExtension(datFile);
                            OriginalFilenameWords = OriginalFileName.Split('_');

                            FileType = OriginalFilenameWords[0];     // ECON or INT or MCCN or SIEM

                            // determine if date of file is before the import cutoff date
                            if (FileType == "MCCN" & OriginalFilenameWords.Length == 4)
                                // new McCain filenaming format in firmware v1.10
                                // format is MCCN_ID_YYYYMMDD_HHMMSS.dat
                                IntervalStart = DateTime.Parse(OriginalFilenameWords[2].Substring(5, 2) + "/" + OriginalFilenameWords[2].Substring(OriginalFilenameWords[2].Length - 2, 2) + "/" + OriginalFilenameWords[2].Substring(0, 4) + " " + OriginalFilenameWords[3].Substring(0, 2) + ":" + OriginalFilenameWords[3].Substring(3, 2) + ":" + OriginalFilenameWords[3].Substring(5, 2));
                            //IntervalStart = (DateTime)Strings.Mid(OriginalFilenameWords[2], 5, 2) + "/" + Strings.Right(OriginalFilenameWords[2], 2) + "/" + Strings.Left(OriginalFilenameWords[2], 4) + " " + Strings.Left(OriginalFilenameWords[3], 2) + ":" + Strings.Mid(OriginalFilenameWords[3], 3, 2) + ":" + Strings.Mid(OriginalFilenameWords[3], 5, 2);
                            else
                                // date separated by underscore
                                IntervalStart = DateTime.Parse(OriginalFilenameWords[3] + "/" + OriginalFilenameWords[4] + "/" + OriginalFilenameWords[2] + " " + OriginalFilenameWords[5].Substring(0, 2) + ":" + OriginalFilenameWords[5].Substring(3, 2));
                                //IntervalStart = (DateTime)OriginalFilenameWords[3] + "/" + OriginalFilenameWords[4] + "/" + OriginalFilenameWords[2] + " " + Strings.Left(OriginalFilenameWords[5], 2) + ":" + Strings.Mid(OriginalFilenameWords[5], 3, 2);

                            if (IntervalStart.Date == ScanDate.Date | IntervalStart.Date == PreviousDate.Date)
                            {
                                // Decompress Econlite datZ files
                                if (FileExt == ".datZ" & FileType == "ECON")
                                {
                                    Encoding fileEncoding = Encoding.ASCII;

                                    var fileStream = File.Open(datFile, FileMode.Open);
                                    MemoryStream memoryStream = new MemoryStream();
                                    fileStream.CopyTo(memoryStream);
                                    fileStream.Close();

                                    // set the memory position to the beginning
                                    memoryStream.Position = 0;

                                    // check if data is compressed
                                    if (IsCompressed(memoryStream))
                                    {
                                        memoryStream = DecompressedStream(memoryStream);
                                        FileStream decompressedFile = new FileStream(OriginalDatFile.Replace(".datZ", ".dat"), FileMode.Create, FileAccess.Write);
                                        memoryStream.WriteTo(decompressedFile);
                                        decompressedFile.Close();
                                        memoryStream.Close();

                                        // delete the compressed file
                                        File.Delete(datFile);

                                        // set the datfile to the uncompressed file going forward
                                        OriginalFileName = OriginalFileName.Replace(".datZ", ".dat");
                                        OriginalDatFile = datFile.Replace(".datZ", ".dat");
                                    }
                                }

                                // Siemens files bypass the code below and go to the Siemens decoder
                                if (FileType == "SIEM")
                                {
                                }
                                else
                                {

                                    // convert binary reader to a memory stream to see if that's faster
                                    var fs = File.Open(OriginalDatFile, FileMode.Open);
                                    MemoryStream ms = new MemoryStream();
                                    fs.CopyTo(ms);
                                    fs.Close();

                                    // set the memory stream position to the beginning
                                    ms.Position = 0;

                                    // Dim binReader = New BinaryReader(File.Open(datFile, FileMode.Open))
                                    var binReader = new BinaryReader(ms);
                                    byte tempByte;
                                    int HeaderLineCount;
                                    byte EventCode, EventParmeter;     
                                    int TimerTenths, timerA, timerB;
                                    DateTime EventTime, PriorTime;
                                    byte numHeaders;

                                    PriorTime = IntervalStart.AddSeconds(-1);       //added 8/26/19

                                    HeaderLineCount = 0;

                                    if (FileType == "ECON")
                                        numHeaders = 7;
                                    else if (FileType == "INT")
                                        numHeaders = 6;
                                    else
                                        numHeaders = 0;

                                    // pass through header lines to get to binary data
                                    if (FileType == "ECON" | FileType == "INT")
                                    {
                                        while (HeaderLineCount < numHeaders & (binReader.BaseStream.Position < binReader.BaseStream.Length))     // added position<length per UDOT
                                        {
                                            try
                                            {
                                                tempByte = binReader.ReadByte();
                                                if (tempByte == 10)
                                                    HeaderLineCount += 1;      // 10 = line feed character "\n"
                                            }
                                            catch (Exception ex)
                                            {
                                                break;
                                            }
                                        }

                                        // 4 bytes per record
                                        // byte 1 = event code
                                        // byte 2 = event parameter
                                        // bytes 3 & 4 = 10th of a second from the beginning of the file interval (1 hour for INT and 15 minutes, :00, :15, :30, :45 for ECON)
                                        while ((binReader.BaseStream.Position + 4) <= binReader.BaseStream.Length)        // changed per UDOT - we need to make sure we are more than 4 characters from the end
                                        {
                                            try
                                            {
                                                EventCode = binReader.ReadByte();
                                                EventParmeter = binReader.ReadByte();
                                                timerA = binReader.ReadByte();
                                                timerB = binReader.ReadByte();
                                                TimerTenths = timerA * 256 + timerB;
                                                EventTime = IntervalStart.AddMilliseconds(TimerTenths * 100);
                                                if (EventTime < PriorTime & TimerTenths == 0)
                                                {
                                                    if (FileType == "ECON")
                                                        EventTime = IntervalStart.AddMinutes(15);        // this is really the end of the interval not the beginning
                                                    else if (FileType == "INT")
                                                        EventTime = IntervalStart.AddMinutes(60);
                                                }
                                                else
                                                    PriorTime = EventTime;// set prior event time for the next iteration


                                                if (FileType == "INT")
                                                {
                                                    // skip junk lines from INT files
                                                    if ((EventCode == 0 | EventCode == 1) & (EventParmeter == 0 | EventParmeter > 16))
                                                        continue;       // event code 0 and 1 are for phase events, must have a phase number, not zero and not 255 (can't be > 16 per InDOT)
                                                    if (EventCode == 0 & EventParmeter == 1 & timerA == 0 & timerB == 0)
                                                        continue;     // There are usually two of these at the beginning
                                                    if (EventCode == 2 & EventParmeter == 2 & timerA == 2 & timerB == 2)
                                                        continue;
                                                    if (TimerTenths > 36000)
                                                        continue;   // file intervals one hour, this would be more than an hour, so it's junk

                                                    // translate event code for INT files
                                                    EventCode = TranslateOldEconolite(EventCode);
                                                }

                                                // If EventTime.Date = ScanDate Then
                                                Globals.UDOTEventLog.Add(new ControllerEvent(signal.SignalID, EventTime, EventCode, EventParmeter));
                                            }
                                            // ElseIf EventTime.Date = PreviousDate Then
                                            // PreviousDateEventLog.Add(New ControllerEventUDOT(signal.SignalID, EventTime, EventCode, EventParmeter))
                                            // End If

                                            catch (Exception ex)
                                            {
                                                break;
                                            }
                                        }
                                    }
                                    else if (FileType == "MCCN")
                                    {
                                        binReader.BaseStream.Position = 80;
                                        while ((binReader.BaseStream.Position + 4) <= binReader.BaseStream.Length)
                                        {
                                            try
                                            {
                                                timerA = binReader.ReadByte();
                                                timerB = binReader.ReadByte();
                                                EventCode = binReader.ReadByte();
                                                EventParmeter = binReader.ReadByte();

                                                TimerTenths = timerA * 256 + timerB;
                                                EventTime = IntervalStart.AddMilliseconds(TimerTenths * 100);

                                                // skip junk lines
                                                if (EventCode == 0 & EventParmeter == 0 & TimerTenths == 0)
                                                    continue; // McCain usually has a record will all zeros at the end

                                                // If EventTime.Date = ScanDate Then
                                                Globals.UDOTEventLog.Add(new ControllerEvent(signal.SignalID, EventTime, EventCode, EventParmeter));
                                            }
                                            // ElseIf EventTime.Date = PreviousDate Then
                                            // PreviousDateEventLog.Add(New ControllerEventUDOT(signal.SignalID, EventTime, EventCode, EventParmeter))
                                            // End If

                                            // ExportFile.WriteLine(EventCode & "," & EventParmeter & ",," & EventTime.ToString("MM/dd/yyyy hh:mm:ss.f tt"))
                                            catch (Exception ex)
                                            {
                                                break;
                                            }
                                        }
                                    }
                                    binReader.Close();
                                }
                            }
                        }

                        // Store data for UDOT
                        IEnumerable<ControllerEvent> TempLog;
                        TempLog = Globals.UDOTEventLog.Where(a => a.SignalID == Globals.UDOTSignalID & (a.EventCode > 130 & a.EventCode < 150));

                        CheckApplicationEvents();
                        CheckForRecords();
                        CheckAllSignals();
                        CheckForLowDetectorHits();
                        CheckAlarms();
                    }
                }

                CreateAndSendEmail();
            }
        }

        public void CheckApplicationEvents()
        {
        }

        // created by Steve Gault 4/21/19
        public void CheckAlarms()
        {
            NTCIPController tempController = new NTCIPController();

            DateTime startTime, endTime;
            startTime = PreviousDate.Date;
            endTime = PreviousDate.AddDays(1);

            List<ControllerEvent> alarmEvents;

            // Unit Alarm status 1 would only be used for reporting flash but doens't have a good way to determine when flash starts and ends, so use UnitFlashStatus instead
            // 'Unit Alarm Status 1 (event code 174)
            // alarmEvents = UDOTEventLog.Where(Function(x) x.TimeStamp >= startTime And x.TimeStamp < endTime And x.EventCode = 174)

            // For Each alarm In alarmEvents
            // tempController.UnitAlarmStatus1 = alarm.EventParam
            // If tempController.AlarmLocalFlash = True Then
            // Dim SPMerror As New SPMWatchDogErrorEvent
            // SPMerror.SignalID = UDOTSignalID
            // SPMerror.DetectorID = "0"
            // SPMerror.Phase = 0
            // SPMerror.Direction = ""
            // SPMerror.TimeStamp = alarm.TimeStamp
            // SPMerror.Message = "Local flash Signal " & UDOTSignalID
            // SPMerror.ErrorCode = 10
            // Alarms.Add(SPMerror)
            // End If
            // If tempController.AlarmMMUFlash = True Then
            // Dim SPMerror As New SPMWatchDogErrorEvent
            // SPMerror.SignalID = UDOTSignalID
            // SPMerror.DetectorID = "0"
            // SPMerror.Phase = 0
            // SPMerror.Direction = ""
            // SPMerror.TimeStamp = alarm.TimeStamp
            // SPMerror.Message = "MMU flash Signal " & UDOTSignalID
            // SPMerror.ErrorCode = 10
            // Alarms.Add(SPMerror)
            // End If
            // Next

            // Unit Flash Status (event code 173)
            alarmEvents = Globals.UDOTEventLog.Where(x => x.TimeStamp >= startTime & x.TimeStamp < endTime & x.EventCode == 173).ToList();
            foreach (var alarm in alarmEvents)
            {
                tempController.UnitFlashStatus = (NTCIPController.UnitFlashStatusType)alarm.EventParam;
                // ignore notflash at midnight or 12:15 AM which can be a daily report
                if (alarm.TimeStamp.Hour == 0 & (alarm.TimeStamp.Minute == 0 | alarm.TimeStamp.Minute == 15) & alarm.EventParam == 2)
                {
                }
                else
                {
                    SPMWatchDogErrorEvent SPMerror = new SPMWatchDogErrorEvent();
                    SPMerror.SignalID = Globals.UDOTSignalID;
                    SPMerror.DetectorID = "0";
                    SPMerror.Phase = 0;
                    SPMerror.Direction = "";
                    SPMerror.TimeStamp = alarm.TimeStamp;

                    SPMerror.Message = "Signal " + Globals.UDOTSignalID + " flash condition: " + tempController.UnitFlashStatus.ToString() + " at " + alarm.TimeStamp.ToString("MM/dd/yyyy hh:mm:ss.f tt");
                    SPMerror.ErrorCode = 10;
                    Console.WriteLine(SPMerror.Message);
                    Alarms.Add(SPMerror);
                }
            }

            // Power Failures (event codes 182, 184)
            int[] EventCodes = new[] { 182, 184 };
            alarmEvents = Globals.UDOTEventLog.Where(x => x.TimeStamp >= startTime & x.TimeStamp < endTime & EventCodes.Contains(x.EventCode)).ToList();

            foreach (var alarm in alarmEvents)
            {
                SPMWatchDogErrorEvent SPMerror = new SPMWatchDogErrorEvent();
                SPMerror.SignalID = Globals.UDOTSignalID;
                SPMerror.DetectorID = "0";
                SPMerror.Phase = 0;
                SPMerror.Direction = "";
                SPMerror.TimeStamp = alarm.TimeStamp;
                if (alarm.EventCode == 182)
                    SPMerror.Message = "Power failure detected Signal " + Globals.UDOTSignalID + " at " + alarm.TimeStamp.ToString("MM/dd/yyyy hh:mm:ss.f tt");
                if (alarm.EventCode == 184)
                    SPMerror.Message = "Power restored Signal " + Globals.UDOTSignalID + " at " + alarm.TimeStamp.ToString("MM/dd/yyyy hh:mm:ss.f tt");
                SPMerror.ErrorCode = 10;
                Console.WriteLine(SPMerror.Message);
                Alarms.Add(SPMerror);
            }

            // Manual control (event code 178)
            alarmEvents = Globals.UDOTEventLog.Where(x => x.TimeStamp >= startTime & x.TimeStamp < endTime & x.EventCode == 178).ToList();

            foreach (var alarm in alarmEvents)
            {
                SPMWatchDogErrorEvent SPMerror = new SPMWatchDogErrorEvent();
                SPMerror.SignalID = Globals.UDOTSignalID;
                SPMerror.DetectorID = "0";
                SPMerror.Phase = 0;
                SPMerror.Direction = "";
                SPMerror.TimeStamp = alarm.TimeStamp;
                if (alarm.EventParam == 1)
                    SPMerror.Message = "Manual control enabled " + Globals.UDOTSignalID + " at " + alarm.TimeStamp.ToString("MM/dd/yyyy hh:mm:ss.f tt");
                if (alarm.EventParam == 0)
                    SPMerror.Message = "Manual control disabled " + Globals.UDOTSignalID + " at " + alarm.TimeStamp.ToString("MM/dd/yyyy hh:mm:ss.f tt");
                SPMerror.ErrorCode = 10;
                Console.WriteLine(SPMerror.Message);
                Alarms.Add(SPMerror);
            }

            // Alarm Status Group (event code 175)
            alarmEvents = Globals.UDOTEventLog.Where(x => x.TimeStamp >= startTime & x.TimeStamp < endTime & x.EventCode == 175).ToList();

            foreach (var alarm in alarmEvents)
            {
                // Alarm 1 (door open) is bit 0
                bool Alarm1Status = tempController.GetBit8(alarm.EventParam, 0);

                // ignore alarm inactive at midnight or 12:15 AM which are just daily reports
                // If Alarm1Status = False And alarm.TimeStamp.Hour = 0 And (alarm.TimeStamp.Minute = 0 Or alarm.TimeStamp.Minute = 15) Then
                // sometimes there's a daily report door open at midnight which is false. Sometimes the alarm happens at 12:15 because it gets incorrectly recorded at the end of the file interval instead of the beginning
                if (alarm.TimeStamp.Hour == 0 & (alarm.TimeStamp.Minute == 0 || alarm.TimeStamp.Minute == 15))
                {
                }
                else
                {
                    SPMWatchDogErrorEvent SPMerror = new SPMWatchDogErrorEvent();
                    SPMerror.SignalID = Globals.UDOTSignalID;
                    SPMerror.DetectorID = "0";
                    SPMerror.Phase = 0;
                    SPMerror.Direction = "";
                    SPMerror.TimeStamp = alarm.TimeStamp;
                    if (Alarm1Status == true)
                        SPMerror.Message = "Cabinet door opened " + Globals.UDOTSignalID + " at " + alarm.TimeStamp.ToString("MM/dd/yyyy hh:mm:ss.f tt");
                    if (Alarm1Status == false)
                        SPMerror.Message = "Cabinet door closed " + Globals.UDOTSignalID + " at " + alarm.TimeStamp.ToString("MM/dd/yyyy hh:mm:ss.f tt");
                    SPMerror.ErrorCode = 10;
                    Console.WriteLine(SPMerror.Message);
                    Alarms.Add(SPMerror);
                }
            }
        }


        public void CheckAllSignals()
        {
            TimeSpan startHour = new TimeSpan(Settings.ScanDayStartHour, 0, 0);
            TimeSpan endHour = new TimeSpan(Settings.ScanDayEndHour, 0, 0);
            DateTime analysisStart = ScanDate.Date + startHour;
            DateTime analysisEnd = ScanDate.Date + endHour;

            AnalysisPhaseCollectionUDOT APcollection = new AnalysisPhaseCollectionUDOT(Globals.UDOTSignalID, analysisStart, analysisEnd, Settings.ConsecutiveCount);

            foreach (var phase in APcollection.Items)
            {
                CheckForMaxOut(phase);
                CheckForForceOff(phase);
                CheckForStuckPed(phase);
            }
        }

        public void CheckForRecords()
        {
            if (Settings.WeekdayOnly & ScanDate.DayOfWeek == DayOfWeek.Monday)
                CheckSignalRecordCount(ScanDate.AddDays(-2));
            else
                CheckSignalRecordCount(ScanDate);
        }

        public void CheckSignalRecordCount(DateTime dateToCheck)
        {
            long RecordCount = Globals.UDOTEventLog.Where(r => r.SignalID == Globals.UDOTSignalID & r.TimeStamp >= dateToCheck.AddDays(-1) & r.TimeStamp < dateToCheck).Count();
            if (RecordCount > Settings.MinimumRecords)
            {
                Console.WriteLine("Signal " + Globals.UDOTSignalID + " has current records");
                SignalsWithRecords.Add(Globals.UDOTSignalID);
            }
            else
            {
                Console.WriteLine("Signal " + Globals.UDOTSignalID + " does Not have current records.");
                SignalsNoRecords.Add(Globals.UDOTSignalID);
                SPMWatchDogErrorEvent SPMerror = new SPMWatchDogErrorEvent();
                SPMerror.SignalID = Globals.UDOTSignalID;
                SPMerror.DetectorID = "0";
                SPMerror.Phase = 0;
                SPMerror.Direction = "";
                SPMerror.TimeStamp = ScanDate;
                SPMerror.Message = "Missing Records Signal " + Globals.UDOTSignalID;
                SPMerror.ErrorCode = 1;
                MissingRecords.Add(SPMerror);
            }
        }

        public void CheckForLowDetectorHits()
        {
            var detectors = Globals.UDOTGraph_Detectors.Where(a => a.Has_PCD == true & a.SignalID == Globals.UDOTSignalID);

            foreach (var detector in detectors)
            {
                var channel = detector.Det_Channel;
                var direction = detector.Direction;
                DateTime startTime;
                DateTime endTime;
                if (Settings.WeekdayOnly == true & ScanDate.DayOfWeek == DayOfWeek.Monday)
                {
                    startTime = ScanDate.AddDays(-3).Date.AddHours(Settings.PreviousDayPMPeakStart);
                    endTime = ScanDate.AddDays(-3).Date.AddHours(Settings.PreviousDayPMPeakEnd);
                }
                else
                {
                    startTime = ScanDate.AddDays(-1).Date.AddHours(Settings.PreviousDayPMPeakStart);
                    endTime = ScanDate.AddDays(-1).Date.AddHours(Settings.PreviousDayPMPeakEnd);
                }

                long currentVolume = Globals.UDOTEventLog.Where(a => a.TimeStamp >= startTime & a.TimeStamp <= endTime & a.SignalID == Globals.UDOTSignalID & a.EventParam == channel & a.EventCode == 82).Count();

                if (currentVolume < Settings.LowHitThreshold)
                {
                    SPMWatchDogErrorEvent SPMerror = new SPMWatchDogErrorEvent();
                    SPMerror.SignalID = Globals.UDOTSignalID;
                    SPMerror.DetectorID = detector.DetectorID;
                    SPMerror.Phase = detector.Phase;
                    SPMerror.TimeStamp = ScanDate;
                    SPMerror.Direction = direction;
                    SPMerror.Message = "CH: " + channel + " - Count: " + currentVolume;
                    SPMerror.ErrorCode = 2;
                    if (!LowHitCountErrors.Contains(SPMerror))
                        LowHitCountErrors.Add(SPMerror);
                }
            }
        }

        public void CheckForStuckPed(AnalysisPhaseUDOT phase)
        {
            if (phase.PedestrianEvents.Count > Settings.MaximumPedestrianEvents)
            {
                SPMWatchDogErrorEvent SPMerror = new SPMWatchDogErrorEvent();
                SPMerror.SignalID = Globals.UDOTSignalID;
                SPMerror.Phase = phase.PhaseNumber;
                SPMerror.TimeStamp = ScanDate;
                SPMerror.Direction = phase.Direction ?? "";
                SPMerror.Message = phase.PedestrianEvents.Count + " Pedestrian Activations";
                SPMerror.ErrorCode = 3;
                if (!StuckPedErrors.Contains(SPMerror))
                {
                    Console.WriteLine("Signal " + Globals.UDOTSignalID + " " + phase.PedestrianEvents.Count + " Pedestrian Activations");
                    StuckPedErrors.Add(SPMerror);
                }
            }
        }

        public void CheckForForceOff(AnalysisPhaseUDOT phase)
        {
            if (phase.PercentForceOffs > Settings.PercentThreshold & phase.TerminationEvents.Where(t => t.EventCode != 7).Count() > Settings.MinPhaseTerminations)
            {
                SPMWatchDogErrorEvent SPMerror = new SPMWatchDogErrorEvent();
                SPMerror.SignalID = Globals.UDOTSignalID;
                SPMerror.Phase = phase.PhaseNumber;
                SPMerror.TimeStamp = ScanDate;
                SPMerror.Direction = phase.Direction ?? "";
                SPMerror.Message = "Force Offs " + Math.Round(phase.PercentForceOffs * 100, 1) + "%";
                SPMerror.ErrorCode = 4;
                if (!ForceOffErrors.Contains(SPMerror))
                    ForceOffErrors.Add(SPMerror);
            }
        }

        public void CheckForMaxOut(AnalysisPhaseUDOT phase)
        {
            // added filter for phases that don't have any detection which will be required to run max recall
            int detectionChannelsForPhase = Globals.UDOTGraph_Detectors.Where(x => x.SignalID == Globals.UDOTSignalID & x.Phase == phase.PhaseNumber).Count();
            if (detectionChannelsForPhase > 0 & phase.PercentMaxOuts > Settings.PercentThreshold & phase.TotalPhaseTerminations > Settings.MinPhaseTerminations)
            {
                SPMWatchDogErrorEvent SPMerror = new SPMWatchDogErrorEvent();
                SPMerror.SignalID = Globals.UDOTSignalID;
                SPMerror.Phase = phase.PhaseNumber;
                SPMerror.TimeStamp = ScanDate;
                SPMerror.Direction = phase.Direction ?? "";
                SPMerror.Message = "Max Outs " + Math.Round(phase.PercentMaxOuts * 100, 1) + "%";
                SPMerror.ErrorCode = 5;
                if (MaxOutErrors.Count == 0 | !MaxOutErrors.Contains(SPMerror))
                {
                    Console.WriteLine("Signal " + Globals.UDOTSignalID + " has MaxOut errors");
                    MaxOutErrors.Add(SPMerror);
                }
            }
        }

        public void CreateAndSendEmail()
        {
            MailMessage message = new MailMessage();
            string[] users = Properties.Settings.Default.EmailNotificationsTo.Split(';');
            foreach (var user in users)
                message.To.Add(user.Trim());
            message.To.Add(Settings.DefaultEmailAddress);
            message.Subject = Properties.Settings.Default.EmailSubject + " for " + ScanDate.ToShortDateString();
            message.From = new MailAddress(Settings.FromEmailAddress);
            var missingErrors = SortAndAddToMessage(MissingRecords);
            var forceErrors = SortAndAddToMessage(ForceOffErrors);
            var maxErrors = SortAndAddToMessage(MaxOutErrors);
            var countErrors = SortAndAddToMessage(LowHitCountErrors);
            var stuckPedErrorsMsg = SortAndAddToMessage(StuckPedErrors);
            var ftpErrors = SortAndAddToMessage(CannotFtpFiles);
            var alarmErrors = SortAndAddToMessage(Alarms);

            if (Alarms.Count > 0 & alarmErrors != "")
            {
                message.Body += " <br> <b>--The following signals had CRITICAL ALARMS on ";
                if (Settings.WeekdayOnly & ScanDate.DayOfWeek == DayOfWeek.Monday)
                    message.Body += ScanDate.AddDays(-3).Date.ToShortDateString() + ": </b><br>";
                else
                    message.Body += ScanDate.AddDays(-1).Date.ToShortDateString() + ": </b><br>";
                message.Body += alarmErrors;
            }
            else
            {
                message.Body += " <br> <i>--No critical alarms were found on ";
                if (Settings.WeekdayOnly & ScanDate.DayOfWeek == DayOfWeek.Monday)
                    message.Body += ScanDate.AddDays(-3).Date.ToShortDateString() + ": </i><br>";
                else
                    message.Body += ScanDate.AddDays(-1).Date.ToShortDateString() + ": </i><br>";
            }

            if (MissingRecords.Count > 0 & missingErrors != "")
            {
                message.Body += " <br> <b>--The following signals had too few records in the database on ";
                if (Settings.WeekdayOnly & ScanDate.DayOfWeek == DayOfWeek.Monday)
                    message.Body += ScanDate.AddDays(-3).Date.ToShortDateString() + ": </b><br>";
                else
                    message.Body += ScanDate.AddDays(-1).Date.ToShortDateString() + ": </b><br>";
                message.Body += "This error could indicate a communication issue at the intersection or the data logger may have been disabled.<br>";
                message.Body += missingErrors;
            }
            else
            {
                message.Body += " <br> <i>--No new missing record errors were found on ";
                if (Settings.WeekdayOnly & ScanDate.DayOfWeek == DayOfWeek.Monday)
                    message.Body += ScanDate.AddDays(-3).Date.ToShortDateString() + ": </i><br>";
                else
                    message.Body += ScanDate.AddDays(-1).Date.ToShortDateString() + ": </i><br>";
            }

            if (ForceOffErrors.Count > 0 & forceErrors != "")
            {
                message.Body += " <br> <b>--The following signals had too many force off occurrence between " + Settings.ScanDayStartHour + ":00 and " + Settings.ScanDayEndHour + ":00: </b><br>";
                message.Body += "This error could indicate a signal is running in coordination overnight when free operation could be more effective.<br>";
                message.Body += forceErrors;
            }
            else
                message.Body += "<br><i> --No new force off errors were found between " + Settings.ScanDayStartHour + ":00 and " + Settings.ScanDayEndHour + ":00 </i><br>";

            if (MaxOutErrors.Count > 0 & maxErrors != "")
            {
                message.Body += " <br><b> --The following signals had too many max out occurrences between " + Settings.ScanDayStartHour + ":00 and " + Settings.ScanDayEndHour + ":00: </b><br>";
                message.Body += "This error could indicate a detector malfunction causing a signal to provide excessive time to a phase.<br>";
                message.Body += maxErrors;
            }
            else
                message.Body += "<br><i> --No new max out errors were found between " + Settings.ScanDayStartHour + ":00 and " + Settings.ScanDayEndHour + ":00 </i><br>";

            if (LowHitCountErrors.Count > 0 & countErrors != "")
            {
                message.Body += " <br><b> --The following signals had unusually low advanced detection counts on ";
                if (Settings.WeekdayOnly & ScanDate.DayOfWeek == DayOfWeek.Monday)
                    message.Body += ScanDate.AddDays(-3).ToShortDateString() + " between ";
                else
                    message.Body += ScanDate.AddDays(-3).ToShortDateString() + " between ";
                message.Body += Settings.PreviousDayPMPeakStart + ":00 and " + Settings.PreviousDayPMPeakEnd + ":00: </b><br>";
                message.Body += "This error could indicate a detector malfunction causing a signal to provide excessive time to a phase. This error can also occur when a communication error causes there to be no data.<br>";
                message.Body += countErrors;
            }
            else
            {
                message.Body += "<br><i> --No new low advanced detection count errors on ";
                if (Settings.WeekdayOnly & ScanDate.DayOfWeek == DayOfWeek.Monday)
                    message.Body += ScanDate.AddDays(-3).ToShortDateString() + " between ";
                else
                    message.Body += ScanDate.AddDays(-3).ToShortDateString() + " between ";
                message.Body += Settings.PreviousDayPMPeakStart + ":00 and " + Settings.PreviousDayPMPeakEnd + ":00 </i><br>";
            }

            if (StuckPedErrors.Count > 0 & stuckPedErrorsMsg != "")
            {
                message.Body += " <br><b> --The following signals have high pedestrian activation occurrences between " + Settings.ScanDayStartHour + ":00 and " + Settings.ScanDayEndHour + ":00: </b><br>";
                message.Body += "This error could indicate a pedestrian pushbutton malfunction causing excessive time for pedestrian walk and clearance intervals when there are no pedestrians present.<br>";
                message.Body += stuckPedErrorsMsg;
            }
            else
                message.Body += "<br><i> --No new high pedestrian activation errors between " + Settings.ScanDayStartHour + ":00 and " + Settings.ScanDayEndHour + ":00 </i><br>";

            // If CannotFtpFiles.Count > 0 And ftpErrors <> "" Then
            // message.Body &= " <br><b> --The following signals have had FTP problems. No log files were obtained. </b><br>"
            // message.Body &= ftpErrors
            // Else
            // message.Body &= "<br><i> --No new controllers had problems retrieving log files.</i><br>"
            // End If
            message.IsBodyHtml = true;

            // message.Attachments.Add(New Attachment("mypdffile.pdf"))

            if (Properties.Settings.Default.GenerateEmail == true)
                SendMessage(message);
            else
            {
                // print message to console, replacing HTML elements for plain text
                var consoleMessage = message.Body.Replace("<br>", @"\n");
                consoleMessage = consoleMessage.Replace( "<b>", "");
                consoleMessage = consoleMessage.Replace( "</b>", "");
                consoleMessage = consoleMessage.Replace( "<i>", "");
                consoleMessage = consoleMessage.Replace( "</i>", "");
                Console.WriteLine(consoleMessage);
                Console.ReadLine();
            }
        }

        private void SendMessage(MailMessage message)
        {
            SmtpClient smtp = new SmtpClient(Settings.EmailServer);
            // these are specific GMAIL settings
            smtp.EnableSsl = true;
            smtp.Port = 587;
            //see https://support.google.com/mail/answer/185833?hl=en to generate a Gmail App Password that can be used here
            smtp.Credentials = new System.Net.NetworkCredential("ChangeMe@gmail.com", "password");
            // 
            try
            {
                Console.WriteLine("Sent message to: " + message.To.ToString() + @"\nMessage text: " + message.Body + @"\n");
                smtp.Send(message);
            }
            catch (Exception ex)
            {
            }
        }

        private string SortAndAddToMessage(List<SPMWatchDogErrorEvent> SPMerrors)
        {
            var sortedErrors = SPMerrors.OrderBy(x => x.SignalID).ThenBy(x => x.Phase).ThenBy(x => x.TimeStamp).ToList();
            var ErrorMessage = "";

            foreach (var spmError in sortedErrors)
            {
                if (!Settings.EmailAllErrors)
                {
                }

                if (Settings.EmailAllErrors)
                {
                    ATSPMsignal signal = Globals.signalsListATSPM.Where(a => a.SignalID == spmError.SignalID).FirstOrDefault();
                    ErrorMessage += spmError.SignalID;
                    ErrorMessage += " - ";
                    ErrorMessage += signal.PrimaryName + " & " + signal.SecondaryName;
                    if (spmError.Phase > 0)
                        ErrorMessage += " - Phase " + spmError.Phase;
                    ErrorMessage += " (" + spmError.Message + ")";
                    ErrorMessage += "<br>";
                }
            }

            return ErrorMessage;
        }

        private bool IsCompressed(MemoryStream fileStream)
        {
            // read the magic header
            byte[] header = new byte[2];
            fileStream.Read(header, 0, 2);

            // let seek to back of file
            fileStream.Seek(0, SeekOrigin.Begin);

            // let's check for zLib compression
            if (AreEqual(header, _ZlibHeaderNoCompression) | AreEqual(header, _ZlibHeaderDefaultCompression) | AreEqual(header, _ZlibHeaderBestCompression))
                return true;
            else if (AreEqual(header, _GZipHeader))
                return true;

            return false;
        }

        private MemoryStream DecompressedStream(MemoryStream fileStream)
        {
            // read past the first two bytes of the zlib header
            fileStream.Seek(2, SeekOrigin.Begin);

            // decompress the file
            using (DeflateStream deflatedStream = new DeflateStream(fileStream, CompressionMode.Decompress))
            {
                // copy decompressed data into return stream
                MemoryStream returnStream = new MemoryStream();
                deflatedStream.CopyTo(returnStream);
                returnStream.Position = 0;

                return returnStream;
            }
        }

        private bool AreEqual(byte[] a1, byte[] b1)
        {
            if (a1.Length != b1.Length)
                return false;

            for (var i = 0; i <= a1.Length - 1; i++)
            {
                if (a1[i] != b1[i])
                    return false;
            }

            return true;
        }

        private byte TranslateOldEconolite(byte OldCode)
        {
            byte e;
            switch (OldCode)
            {
                case 0:  // Phase off
                    {
                        e = 12;
                        break;
                    }

                case 1:  // Phase green
                    {
                        e = 1;
                        break;
                    }

                case 2:  // Phase yellow
                    {
                        e = 8;
                        break;
                    }

                case 3:  // Phase red clear
                    {
                        e = 10;
                        break;
                    }

                case 4:  // Ped off
                    {
                        e = 23;
                        break;
                    }

                case 5:  // Ped walk
                    {
                        e = 21;
                        break;
                    }

                case 6:  // Ped clear
                    {
                        e = 22;
                        break;
                    }

                case 8:  // Detector off
                    {
                        e = 81;
                        break;
                    }

                case 9:  // detector on
                    {
                        e = 82;
                        break;
                    }

                case 12: // overlap off
                    {
                        e = 65;
                        break;
                    }

                case 13: // overlap green
                    {
                        e = 61;
                        break;
                    }

                case 14: // overlap green extension
                    {
                        e = 62;
                        break;
                    }

                case 15: // overlap yellow
                    {
                        e = 63;
                        break;
                    }

                case 16: // overlap red clear
                    {
                        e = 64;
                        break;
                    }

                case 20: // preempt active
                    {
                        e = 102;
                        break;
                    }

                case 21: // preempt off
                    {
                        e = 104;
                        break;
                    }

                case 24: // phase hold active
                    {
                        e = 41;
                        break;
                    }

                case 25: // phase hold released
                    {
                        e = 42;
                        break;
                    }

                case 26: // ped call on phase
                    {
                        e = 45;
                        break;
                    }

                case 27: // ped call cleared (this event doesn't exist in new enumerations, just call it phase call dropped
                    {
                        e = 44;
                        break;
                    }

                case 32: // phase min complete
                    {
                        e = 3;
                        break;
                    }

                case 33: // phase termination gap out
                    {
                        e = 4;
                        break;
                    }

                case 34: // phase temrination max out
                    {
                        e = 5;
                        break;
                    }

                case 35: // phase termination force off
                    {
                        e = 6;
                        break;
                    }

                case 40: // coord pattern change
                    {
                        e = 131;
                        break;
                    }

                case 41: // cycle length change
                    {
                        e = 132;
                        break;
                    }

                case 42: // offset length change
                    {
                        e = 133;
                        break;
                    }

                case 43: // split 1 change
                    {
                        e = 134;
                        break;
                    }

                case 44: // split 2 change
                    {
                        e = 135;
                        break;
                    }

                case 45: // split 3 change
                    {
                        e = 136;
                        break;
                    }

                case 46: // split 4 change
                    {
                        e = 137;
                        break;
                    }

                case 47: // split 5 change
                    {
                        e = 138;
                        break;
                    }

                case 48: // split 6 change
                    {
                        e = 139;
                        break;
                    }

                case 49: // split 7 change
                    {
                        e = 140;
                        break;
                    }

                case 50: // split 8 change
                    {
                        e = 141;
                        break;
                    }

                case 51: // split 9 change
                    {
                        e = 142;
                        break;
                    }

                case 52: // split 10 change
                    {
                        e = 143;
                        break;
                    }

                case 53: // split 11 change
                    {
                        e = 144;
                        break;
                    }

                case 54: // split 12 change
                    {
                        e = 145;
                        break;
                    }

                case 55: // split 13 change
                    {
                        e = 146;
                        break;
                    }

                case 56: // split 14 change
                    {
                        e = 147;
                        break;
                    }

                case 57: // split 15 change
                    {
                        e = 148;
                        break;
                    }

                case 58: // split 16 change
                    {
                        e = 149;
                        break;
                    }

                case 62: // coord cycle state change
                    {
                        e = 150;
                        break;
                    }

                case 63: // coord phase yield point
                    {
                        e = 151;
                        break;
                    }

                default:
                    {
                        e = 255;
                        break;
                    }
            }
            return e;
        }

        /// <summary>
        /// Returns file names from given folder that comply to given filters
        /// https://stackoverflow.com/questions/7039580/multiple-file-extensions-searchpattern-for-system-io-directory-getfiles
        /// </summary>
        /// <param name="SourceFolder">Folder with files to retrieve</param>
        /// <param name="Filter">Multiple file filters separated by | character</param>
        /// <param name="searchOption">File.Io.SearchOption, could be AllDirectors or TopDirectoryOnly</param>
        /// <returns>Array of FileInfo objects that presents collection of file names that meet given filters</returns>
        /// <remarks>Added 8/26/19</remarks>
        private string[] getFiles(string SourceFolder, string Filter, System.IO.SearchOption searchOption)
        {
            ArrayList alFiles = new ArrayList();

            //create an array of filter string
            string[] MultipleFilters = Filter.Split('|');

            //for each filter fint matching file names
            foreach (string FileFilter in MultipleFilters)
            {
                alFiles.AddRange(Directory.GetFiles(SourceFolder, FileFilter, searchOption));
            }

            //returns string array of relevant file names
            return (string[])alFiles.ToArray(typeof(string));
        }
    }


}

