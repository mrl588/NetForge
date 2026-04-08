class FlowKey{
    public string srcIP { get;}
    public string dstIP { get; }
    public int srcPort {get ; }
    public int dstPort {get; }
    public string protocol {get; }

    public FlowKey(string SrcIP , string DstIP, int SrcPort, int DstPort, string Protocol){
        srcIP = SrcIP;
        dstIP = DstIP;
        srcPort = SrcPort;
        dstPort = DstPort;
        protocol = Protocol;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not FlowKey other){
            return false;
        }
        else{
            return other.srcIP == srcIP && other.dstIP == dstIP && other.srcPort == srcPort && other.dstPort == dstPort && other.protocol == protocol;
        }
    }
    //if two keys are equal they must have same gethash code else dictonary might look into new bucket
    public override int GetHashCode(){
        return HashCode.Combine(srcIP,dstIP,dstPort,srcPort,protocol);
    } 

}