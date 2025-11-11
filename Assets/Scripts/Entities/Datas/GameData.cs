

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Unity.VisualScripting;
[Serializable]
public class GameData
{
    public GameData() { }
    [JsonProperty]
    public long coins;
    [JsonProperty]
    public int level = 1;
    [JsonProperty]
    public int farmAmount = 1;
    [JsonProperty]
    public List<PlotData> plotDatas = new();
    [JsonProperty]
    public List<FarmProductData> farmProductDatas = new();
    [JsonProperty]
    public List<SeedData> seedDatas = new();
    public GameData Clone()
    {
        var clone = new GameData();
        clone.coins = coins;
        clone.level = level;
        clone.farmAmount = farmAmount;
        foreach (var clo in plotDatas)
        {
            clone.plotDatas.Add(clo.Clone());
        }
        foreach (var clo in farmProductDatas)
        {
            clone.farmProductDatas.Add(clo.Clone());
        }
        foreach (var clo in seedDatas)
        {
            clone.seedDatas.Add(clo.Clone());
        }
        
        return clone;
    }

}