using System.Reflection.Metadata;
using System.Collections.Generic;
using PacketDotNet;
using SharpPcap;

class PacketProcessor{
    
    private readonly FlowManager manager;

    public PacketProcessor(FlowManager manager)
    {
        this.manager = manager;
    }


    public void ProcessPacket(RawCapture rawCapture)
    {
        var linkType = rawCapture.LinkLayerType;
        var rawBytes = rawCapture.Data;

        var packet = Packet.ParsePacket(linkType,rawBytes);
        var ipPacket = packet.Extract<IPPacket>();
        if (ipPacket == null) return;

        string srcIP = ipPacket.SourceAddress.ToString();
        string dstIP = ipPacket.DestinationAddress.ToString();
        int packetLength = rawBytes.Length;

        string protocol = "OTHER";
        int srcPort = 0;
        int dstPort = 0;
        bool hasFin = false;
        bool hasRst = false;
        bool hasSyn = false;

        var tcpPacket = ipPacket.Extract<TcpPacket>();
        if (tcpPacket != null)
        {
            protocol = "TCP";
            srcPort = tcpPacket.SourcePort;
            dstPort = tcpPacket.DestinationPort;
            hasSyn = tcpPacket.Synchronize;
            hasRst = tcpPacket.Reset;
            hasFin = tcpPacket.Finished;

        }
        else{
            var udpPacket = ipPacket.Extract<UdpPacket>();
            if(udpPacket != null)
            {
                protocol = "UDP";
                srcPort = udpPacket.SourcePort;
                dstPort = udpPacket.DestinationPort;
            
            }
        }

        var key = new FlowKey(srcIP,dstIP,srcPort,dstPort,protocol);
        var packetTime = rawCapture.Timeval.Date;

        manager.UpdateFlow(key,packetTime, packetLength, hasSyn, hasFin,hasRst);
        manager.CheckTimeouts();
    }
}