using System.Collections.Generic;
class FlowManager{
    public Dictionary<FlowKey, Flow> ActiveFlows { get; } = new();
    public List<Flow> CompletedFlows { get; } = new();
    public List<FeatureVector> CompletedFeatures { get; } = new();

    public void UpdateFlow(FlowKey key,DateTime packetTime, int packetLength, bool hasSyn, bool hasFin, bool hasRst){
        if (ActiveFlows.ContainsKey(key)){
            ActiveFlows[key].PacketCount += 1;
            ActiveFlows[key].ByteCount += packetLength;
            ActiveFlows[key].EndTime = packetTime;
            if (hasSyn == true){
                ActiveFlows[key].SYNCount += 1;
            }
            if (hasFin == true){
                ActiveFlows[key].FINCount += 1;
            }
            if (hasRst == true){
                ActiveFlows[key].RSTCount += 1;
            } 
        }
        else{
            ActiveFlows[key] = new Flow(packetTime, packetLength, hasSyn, hasFin, hasRst);
        }
    }
    public void CheckTimeouts(){
        var remove = new List<FlowKey>();
        foreach( var key in ActiveFlows.Keys){
            if ((DateTime.UtcNow - ActiveFlows[key].EndTime).TotalSeconds > 100)
            {
                var flow = ActiveFlows[key];
                CompletedFlows.Add(flow);
                CompletedFeatures.Add(new FeatureVector(key, flow));
                remove.Add(key);
            }
        }
        foreach(var key in remove){
            ActiveFlows.Remove(key);
        }
    }
}