using Newtonsoft.Json;
using System;
using UnityEngine;

[Serializable]
public class FarmUpgradeData
{
    FarmManager farmManager;
    private const int MAX_LEVEL = 20; // tùy bạn giới hạn bao nhiêu cấp
    private const float BASE_MULTIPLIER = 0.9f; // giảm 10% mỗi cấp
    private const float BASE_YIELD_MULTIPLIER = 1.1f; // mỗi cấp tăng 10%
    [JsonProperty] public int level = 1;
    public float harvestSpeedMultiplier => Mathf.Pow(BASE_MULTIPLIER, level-1);
    public float yieldAmountMutiplier => Mathf.Pow(BASE_YIELD_MULTIPLIER, level);
    public int nextUpgradeCost => 500;
    public FarmUpgradeData(FarmManager farmManagerq)
    {
        this.farmManager = farmManagerq;
    }

    public bool Upgrade(ref long coins, int countlevel = 1)
    {
        if (level >= MAX_LEVEL)
        {
            Debug.Log("⚠️ Đã đạt cấp tối đa!");
            return false;
        }

        int cost = nextUpgradeCost;
        if (coins >= cost)
        {
            coins -= cost * countlevel;
            level += countlevel;
            Debug.Log($"✅ Nâng cấp thành công! Level {level} - Tốc độ tăng {Math.Round((1 - harvestSpeedMultiplier) * 100, 1)}%");
            Debug.Log($"✅ Nâng cấp thành công! Level {level} - tiền bán tăng {Math.Round((yieldAmountMutiplier) * 100, 1)}%");
            foreach (var plant in farmManager.plantedEntities)
            {
                plant.data.ApplyUpgrade(this);
            }
            return true;
        }
        else
        {
            Debug.Log("❌ Không đủ vàng để nâng cấp!");
            return false;
        }
    }
}
