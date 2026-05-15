using System.Dynamic;

class Flow{ 

    public Flow(DateTime packetTime, int packetLength, bool hasSyn, bool hasFin, bool hasRst)
{
    StartTime = packetTime;
    EndTime = packetTime;
    PacketCount = 1;
    ByteCount = packetLength;
    SYNCount = hasSyn ? 1 : 0;
    FINCount = hasFin ? 1 : 0;
    RSTCount = hasRst ? 1 : 0;
}
    public DateTime StartTime { get; set;}
    public DateTime EndTime {get; set; }
    public int PacketCount {get; set;}
    public int ByteCount { get; set;}
    public int SYNCount {get ; set;}
    public int FINCount { get; set;}
    public int RSTCount {get; set;}
    public double AveragePacketCount
    {   
        get{
            if (PacketCount == 0) { 
                return 0;
            }
            else{
                return PacketCount / ByteCount;
            }
        }
    }
    public double DurationSeconds => (EndTime - StartTime).TotalSeconds;
    public double PacketsPerSecond => DurationSeconds > 0 ? PacketCount / DurationSeconds : 0;
    public double BytesPerSecond => DurationSeconds > 0 ? ByteCount / DurationSeconds : 0;
    public double SynRatio => PacketCount > 0 ? (double)SYNCount / PacketCount : 0;
    public double RstRatio => PacketCount > 0 ? (double)RSTCount / PacketCount : 0;
    public double AvgPacketSize => PacketCount > 0 ? (double)ByteCount / PacketCount : 0;

}