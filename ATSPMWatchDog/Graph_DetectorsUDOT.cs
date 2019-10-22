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
using Newtonsoft.Json;

public class Graph_DetectorsUDOT
{
    public string DetectorID { get; set; }
    public int SignalID { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Lane { get; set; }
    public byte Phase { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Loops { get; set; }
    public string Direction { get; set; }
    public byte Det_Channel { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string IPaddr { get; set; }
    public int DistanceFromStopBar { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public long Port { get; set; }
    public int MPH { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public int Decision_Point { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public int Region { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public int Movement_Delay { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public int Min_Speed_Filter { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public bool Has_Speed_Detector { get; set; }
    public bool Has_PCD { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public DateTime Monitor_Date { get; set; }
    public bool Has_Phase_Data { get; set; }
    public bool Is_Overlap { get; set; }
    public bool Has_TMC { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string TMC_Lane_Type { get; set; }
    public DateTime Date_Added { get; set; }
    public bool Has_RLM { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Comments { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public byte Perm_Phase { get; set; }
    public bool Has_SplitFail { get; set; }


    public Graph_DetectorsUDOT()
    {
    }
}
