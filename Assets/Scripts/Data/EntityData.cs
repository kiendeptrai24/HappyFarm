using Newtonsoft.Json;
using UnityEngine;


[System.Serializable]
public class EntityData
{
    [JsonProperty]
    public string name;
    [JsonProperty]
    public ProductSaleAble type;
    [JsonProperty]
    public ProductBuyAble seed;
    [JsonProperty]
    public int lifeCycles;
    [JsonProperty]
    public float growthTime;
    public float timeToHarvest;
    [JsonProperty]
    public float baseGrowthTime;
    [JsonProperty]
    public float baseTimeToHarvest;
    [JsonProperty]
    public float witherDelay;
    [JsonProperty]
    public int yieldAmount = 1;
    [JsonProperty]
    public int baseYieldAmount = 1;
    [JsonProperty]
    public int quantity;
    [JsonProperty]
    public int price;
    [JsonProperty]
    public bool isMature;
    [JsonProperty]
    public bool immortal = false;
    public EntityData Clone()
    {
        var clone = (EntityData)this.MemberwiseClone();
        clone.timeToHarvest = baseTimeToHarvest * 60;
        return clone;
    }
    public EntityData(string name, int lifeCycles)
    {
        this.name = name;
        this.lifeCycles = lifeCycles;
    }
    public void ApplyUpgrade(FarmUpgradeData upgradeData)
    {
        timeToHarvest = baseTimeToHarvest * 60;
        timeToHarvest *= upgradeData.harvestSpeedMultiplier;

        yieldAmount = baseYieldAmount;
        yieldAmount = Mathf.RoundToInt(yieldAmount * upgradeData.yieldAmountMutiplier);
    }
    public EntityData() { }
}