// See https://aka.ms/new-console-template for more information
using System;
using PacketDotNet; // will take raw bytes and decodes all the information
using SharpPcap; // talks to os and gets the raw bytes

class Program
{
    static void Main(String[] args)
    {
        var devices = CaptureDeviceList.Instance;

        if  ( devices.Count < 1 ) {
            Console.WriteLine("No devices found. Make sure WinPcap/Npcap is installed.");
            return;
        }
        foreach (var dev in devices)
        {
            Console.WriteLine(dev.Name + " - " + dev.Description);
        }
        RunCapture();

    }
    static void RunCapture(){
        var flowManager = new FlowManager();
        var processor = new PacketProcessor(flowManager);
        int packetCount = 0;
        int lastCompleted = 0;
        var devices = CaptureDeviceList.Instance;
        var device = CaptureDeviceList.Instance[1];
        device.Open(DeviceModes.Promiscuous);

        device.OnPacketArrival += (sender, e) =>
        {
            processor.ProcessPacket(e.GetPacket());
            packetCount++;
            
            if (packetCount % 100 == 0)
            {
                Console.WriteLine($"Packets: {packetCount}, Active: {flowManager.ActiveFlows.Count}, Completed: {flowManager.CompletedFlows.Count}");
            }
            if (flowManager.CompletedFlows.Count > lastCompleted)
            {
                lastCompleted = flowManager.CompletedFlows.Count;
                Console.WriteLine($"New completed flow! Total completed: {lastCompleted}");
            }
        };

        device.StartCapture();
        Console.WriteLine("Capture started. Waiting for packets...");

        // Keep running until Ctrl+C
        Console.WriteLine("Capturing... Press enter to stop.");
        Console.ReadLine();
        device.StopCapture();
        device.Close();
        CsvExporter exporter = new CsvExporter(flowManager.CompletedFeatures);
        exporter.Export("flows.csv");
        Console.WriteLine("Finished export");


    }
}