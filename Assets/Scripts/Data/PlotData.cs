using System;
using Newtonsoft.Json;

[Serializable]
public class PlotData
{
    [JsonProperty] public int row;
    [JsonProperty] public int col;
    [JsonProperty] public bool hasCrop;
    [JsonProperty] public DirtData dirtData;

    // Constructor rỗng
    public PlotData()
    {
        dirtData = new DirtData();
    }

    // Constructor tiện lợi khi tạo runtime
    public PlotData(int row, int col, bool hasCrop, DirtData dirtData = null)
    {
        this.row = row;
        this.col = col;
        this.hasCrop = hasCrop;
        this.dirtData = dirtData ?? new DirtData();
    }
    public PlotData Clone()
    {
        return (PlotData)this.MemberwiseClone();
    }

}
