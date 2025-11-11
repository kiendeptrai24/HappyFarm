
using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour, ISaveLoadData
{
    public List<FarmProductData> farmProductDatas;
    public List<SeedData> seedDatas;
    private Dictionary<string, FarmProductData> farmProductDataBase = new();
    public Action<List<FarmProductData>> OnProductDataUpdate;
    public Action<List<SeedData>> OnSeedDataUpdate;


    private void Awake()
    {
        SaveLoadManager.Instance.RegisterSaveLoadData(this);
    }
    private void Start()
    {
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
        seedDatas.Add(seedData);
        OnSeedDataUpdate?.Invoke(seedDatas);
    }
    public void Load(GameData gameData)
    {
        Debug.Log(gameData.farmProductDatas.Count);
        foreach (var productData in gameData.farmProductDatas)
        {
            farmProductDataBase.Add(productData.name, productData);
            farmProductDatas.Add(productData);
        }
        foreach (var seedData in gameData.seedDatas)
        {
            seedDatas.Add(seedData);
        }
        OnProductDataUpdate?.Invoke(farmProductDatas);
        OnSeedDataUpdate?.Invoke(seedDatas);
    }
    public void Save(GameData gameData)
    {
        gameData.farmProductDatas.Clear();
        gameData.seedDatas.Clear();

        gameData.farmProductDatas = farmProductDatas;
        gameData.seedDatas = seedDatas;
    }
}