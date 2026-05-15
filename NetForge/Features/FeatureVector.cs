class FeatureVector
{
    public string SrcIP { get; }
    public string DstIP { get; }
    public int SrcPort { get; }
    public int DstPort { get; }
    public string Protocol { get; }
    public double Duration { get; }
    public int PacketCount { get; }
    public int ByteCount { get; }
    public int SynCount { get; }
    public int RstCount { get; }
    public double PacketsPerSecond { get; }
    public double BytesPerSecond { get; }
    public double AvgPacketSize { get; }

    public int FinCount { get; }
    public double SynRatio { get; }
    public double RstRatio { get; }


    public FeatureVector(FlowKey key, Flow flow)
    {
        SrcIP = key.srcIP;
        DstIP = key.dstIP;
        SrcPort = key.srcPort;
        DstPort = key.dstPort;
        Protocol = key.protocol;
        Duration = flow.DurationSeconds;
        PacketCount = flow.PacketCount;
        ByteCount = flow.ByteCount;
        SynCount = flow.SYNCount;
        RstCount = flow.RSTCount;
        PacketsPerSecond = flow.PacketsPerSecond;
        BytesPerSecond = flow.BytesPerSecond;
        AvgPacketSize = flow.AvgPacketSize;
        FinCount = flow.FINCount;
        SynRatio = flow.SynRatio;
        RstRatio = flow.RstRatio;
    }
}