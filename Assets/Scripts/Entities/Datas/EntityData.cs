using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;


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
    public float baseTimeToHarvest;
    [JsonProperty]
    public float witherDelay;
    [JsonProperty]
    public int yieldAmount = 1;
    [JsonProperty]
    public int baseYieldAmount = 1;
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
    public void ApplyUpgrade(FarmUpgradeData upgradeData)
    {
        timeToHarvest = baseTimeToHarvest;
        timeToHarvest *= upgradeData.harvestSpeedMultiplier;

        yieldAmount = baseYieldAmount;
        yieldAmount = Mathf.RoundToInt(yieldAmount * upgradeData.yieldAmountMutiplier);
    }
    public EntityData() { }
}