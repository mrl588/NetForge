import pandas as pd
import numpy as np
import json
from pathlib import Path

data_dir = Path("data/MachineLearningCVE")
files = sorted(data_dir.glob("*.csv"))
dfs = [pd.read_csv(f, low_memory=False) for f in files]
df = pd.concat(dfs, ignore_index=True)

df.columns = df.columns.str.strip() # clean up the columns
df["Label"] = df["Label"].astype(str).str.strip()
print(df["Label"].value_counts())
unique_labels = df["Label"].unique()
label_to_id = {"BENIGN": 0} 
next_id = 1                     # next attack gets 1, then 2, 3, ...
for lab in unique_labels:       # every label string in the dataset
    if lab == "BENIGN":
        continue                # skip — already 0
    label_to_id[lab] = next_id  # assign this attack the next number
    next_id += 1                # bump for the next attack

df["LabelID"] = df["Label"].map(label_to_id)



# 11 features
df["Duration"] = df["Flow Duration"]
df["PacketCount"] = df["Total Fwd Packets"] + df["Total Backward Packets"]
df["ByteCount"] = df["Total Length of Fwd Packets"] + df["Total Length of Bwd Packets"]
df["SynCount"] = df["SYN Flag Count"]
df["RstCount"] = df["RST Flag Count"]
df["PacketsPerSecond"] = df["Flow Packets/s"]
df["BytesPerSecond"] = df["Flow Bytes/s"]
df["AvgPacketSize"] = df["Average Packet Size"]
df["FinCount"] = df["FIN Flag Count"]
df["SynRatio"] = df["SynCount"] / df["PacketCount"].replace(0, np.nan)
df["RstRatio"] = df["RstCount"] / df["PacketCount"].replace(0, np.nan)
FEATURES = [
    "Duration", "PacketCount", "ByteCount",
    "SynCount", "RstCount", "FinCount",
    "SynRatio", "RstRatio",
    "PacketsPerSecond", "BytesPerSecond", "AvgPacketSize",
]
df[FEATURES] = df[FEATURES].replace([np.inf, -np.inf], np.nan)
df = df.dropna(subset=FEATURES + ["LabelID"])
out_cols = FEATURES + ["Label", "LabelID"]
df[out_cols].to_csv("data/cicids_netforge_features.csv", index=False)
Path("data/label_to_id.json").write_text(json.dumps(label_to_id, indent=2))
print(f"Saved {len(df)} rows, {len(FEATURES)} features")