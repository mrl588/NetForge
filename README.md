# NetForge (Phase 1)

NetForge is a C# network flow capture and feature extraction project.
The current version is an early prototype inspired by Wireshark-style traffic inspection, focused on collecting per-flow telemetry for future analytics and detection work.

## Current Scope

Phase 1 currently includes:

- live packet capture using `SharpPcap`
- packet parsing using `PacketDotNet`
- flow aggregation using a 5-tuple key (`srcIP`, `dstIP`, `srcPort`, `dstPort`, `protocol`)
- per-flow counters and rates (packets, bytes, duration, PPS, BPS, average packet size, SYN/RST counts)
- CSV export of completed flow feature vectors (`NetForge/flows.csv`)

## Project Structure

- `NetForge/Program.cs`: entry point, device listing, capture lifecycle
- `NetForge/Capture/PacketProcessor.cs`: parses packets and updates flows
- `NetForge/Flow/Flowkey.cs`: immutable flow key (used in dictionary lookups)
- `NetForge/Flow/Flow.cs`: mutable flow state and derived metrics
- `NetForge/Flow/FlowManager.cs`: active/completed flow management and timeout handling
- `NetForge/Features/FeatureVector.cs`: final extracted feature schema
- `NetForge/Export/CsvExporter.cs`: writes feature vectors to CSV

## How It Works

1. Capture a packet from selected network interface.
2. Parse IP/TCP/UDP metadata.
3. Build a `FlowKey` and update or create a flow.
4. On timeout, move flow from active to completed.
5. Convert completed flow into `FeatureVector`.
6. Export all completed features to CSV when capture stops.

## Run Locally

From the repo root:

```bash
dotnet run --project NetForge
```

Notes:

- Packet capture may require elevated permissions depending on your OS.
- Make sure packet capture dependencies are installed and available.
- The current code selects `CaptureDeviceList.Instance[1]` directly; update this if your interface index differs.

## Output

By default, output is written to:

- `NetForge/flows.csv`

CSV columns:

- `SrcIP,DstIP,SrcPort,DstPort,Protocol,Duration,PacketCount,ByteCount,SynCount,RstCount,PacketsPerSecond,BytesPerSecond,AvgPacketSize`

## Known Limitations (Phase 1)

- minimal error handling and no retries
- fixed capture interface selection
- basic timeout-based flow completion
- no labeling or anomaly/classification stage yet
- no visualization/dashboard layer yet

## Next Steps (Phase 2+)

- richer bidirectional/statistical features
- protocol-specific feature expansion
- flow labeling pipeline for supervised tasks
- anomaly or intrusion detection model integration
- real-time monitoring UI and alerting
- performance and memory optimization under high packet rates

## Status

This repository is intentionally in active development.
Phase 1 is focused on reliable packet-to-feature extraction and export.
