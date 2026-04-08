class CsvExporter{

    private readonly List<FeatureVector> _features;
    public CsvExporter(List<FeatureVector> list){
        _features = list;
    }
    public void Export(string filePath){
        var lines = new List<string>();

        //header line 
        lines.Add("SrcIP,DstIP,SrcPort,DstPort,Protocol,Duration,PacketCount,ByteCount,SynCount,RstCount,PacketsPerSecond,BytesPerSecond,AvgPacketSize");
        foreach (var fv in _features)
    {
        lines.Add($"{fv.SrcIP},{fv.DstIP},{fv.SrcPort},{fv.DstPort},{fv.Protocol},{fv.Duration},{fv.PacketCount},{fv.ByteCount},{fv.SynCount},{fv.RstCount},{fv.PacketsPerSecond},{fv.BytesPerSecond},{fv.AvgPacketSize}");
    }
    
    File.WriteAllLines(filePath, lines);
    }
}