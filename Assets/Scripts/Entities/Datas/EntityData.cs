using Newtonsoft.Json;


[System.Serializable]
public class EntityData
{
    [JsonProperty] 
    public string name;
    [JsonProperty]
    public int lifeCycles;
    [JsonProperty]
    public float growthTime;
    [JsonProperty]
    public float timeToHarvest;
    [JsonProperty]
    public float witherDelay;
    [JsonProperty]
    public int yieldAmount;
    [JsonProperty]
    public bool isMature;
    public EntityData Clone()
    {
        return (EntityData)this.MemberwiseClone();
    }
    public EntityData(string name, int lifeCycles)
    {
        this.name = name;
        this.lifeCycles = lifeCycles;
    }
    public EntityData(){}
}