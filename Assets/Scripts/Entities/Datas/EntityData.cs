using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;


[System.Serializable]
public class EntityData
{
    [JsonProperty] 
    public string name;
    [JsonIgnore]
    [JsonProperty]
    public int lifeCycles;
    public float timeToHarvest;
    [JsonIgnore]
    public float witherDelay;
    [JsonIgnore]
    public int yieldAmount;
    [JsonIgnore] 
    public int price;
    public EntityData Clone()
    {
        return (EntityData)this.MemberwiseClone();
    }
}