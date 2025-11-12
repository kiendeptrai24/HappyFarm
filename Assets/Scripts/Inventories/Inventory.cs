
using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour, ISaveLoadData
{
    [Header("Inventory Info")]
    public long coins = 0;
    public FarmUpgradeData farmUpgradeData = new();

    [Header("Farm Product Info")]
    public List<FarmProductData> farmProductDatas = new();
    public List<SeedData> seedDatas = new();
    private Dictionary<string, SeedData> seedDataBase = new();
    private Dictionary<string, FarmProductData> farmProductDataBase = new();

    public Action<long,int> OnCoinsAndLevelUpdate;
    public Action<List<FarmProductData>> OnProductDataUpdate;
    public Action<List<SeedData>> OnSeedDataUpdate;


    private void Awake()
    {
        SaveLoadManager.Instance.RegisterSaveLoadData(this);
    }
    private void Start()
    {
        //call back ui
        OnCoinsAndLevelUpdate?.Invoke(coins, farmUpgradeData.level);
        OnProductDataUpdate?.Invoke(farmProductDatas);
        OnSeedDataUpdate?.Invoke(seedDatas);
    }
    public void AddFarmProduct(EntityData entityData)
    {
        if (farmProductDataBase.TryGetValue(entityData.name, out var productData))
        {
            productData.yieldAmount += entityData.yieldAmount;
            OnProductDataUpdate?.Invoke(farmProductDatas);
        }
    }
    public void AddSeed(SeedData seedData)
    {
        if (seedDataBase.ContainsKey(seedData.type.ToString()) == false)
        {
            //logic create seed when not have
            seedDatas.Add(seedData);
            seedDataBase.Add(seedData.type.ToString(), seedData);
            coins -= seedData.price * seedData.yieldAmount;

            //call back UI
            OnSeedDataUpdate?.Invoke(seedDatas);
            OnCoinsAndLevelUpdate?.Invoke(coins, farmUpgradeData.level);
            return;
        }
        if (seedDataBase.TryGetValue(seedData.type.ToString(), out var seed))
        {
            //logic add seed
            coins -= seedData.price * seedData.yieldAmount;
            seed.yieldAmount += seedData.yieldAmount;

            //call back UI
            OnSeedDataUpdate?.Invoke(seedDatas);
            OnCoinsAndLevelUpdate?.Invoke(coins, farmUpgradeData.level);
        }
    }
    public void RemoveSeed(FarmProductData farmProduct)
    {
        
    }
    public void Load(GameData gameData)
    {
        try
        {
            //load UI
            coins = gameData.coins;
            farmUpgradeData.level = gameData.level;
            foreach (var productData in gameData.farmProductDatas)
            {
                farmProductDataBase.Add(productData.name, productData);
                farmProductDatas.Add(productData);
            }
            foreach (var seedData in gameData.seedDatas)
            {
                seedDatas.Add(seedData);
                seedDataBase.Add(seedData.type.ToString(), seedData);
            }

            // call back ui
            OnProductDataUpdate?.Invoke(farmProductDatas);
            OnSeedDataUpdate?.Invoke(seedDatas);
        }
        catch (Exception ex)
        {
            Debug.Log("Inventory error Load: " + ex.Message);
        }
    }
    public void Save(GameData gameData)
    {
        try
        {
            gameData.farmProductDatas.Clear();
            gameData.seedDatas.Clear();
            gameData.level = farmUpgradeData.level;
            gameData.coins = coins;
            gameData.farmProductDatas = farmProductDatas;
            gameData.seedDatas = seedDatas;
        }
        catch (Exception ex)
        {
            Debug.Log("Inventory error Save: " + ex.Message);
        }
    }
}