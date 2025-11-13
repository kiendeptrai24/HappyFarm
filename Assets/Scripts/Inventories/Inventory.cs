
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour, ISaveLoadData
{
    [Header("Inventory Info")]
    public long coins = 0;
    public FarmUpgradeData farmUpgradeData;

    [Header("Farm Product Info")]
    public List<FarmProductData> farmProductDatas = new();
    public List<SeedData> seedDatas = new();
    private Dictionary<string, SeedData> seedDataBase = new();
    private Dictionary<string, FarmProductData> farmProductDataBase = new();

    public System.Action<long, int> OnCoinsAndLevelUpdate;
    public System.Action<List<FarmProductData>> OnProductDataUpdate;
    public System.Action<List<SeedData>> OnSeedDataUpdate;
    [Header("Seed data prefabs")]
    public List<GameObject> entities = new();
    public Dictionary<string, FarmEntity> farmEntities = new();


    private void Awake()
    {
        SaveLoadManager.Instance.RegisterSaveLoadData(this);
        FarmManager farmManager = FindAnyObjectByType<FarmManager>();
        farmUpgradeData = new FarmUpgradeData(farmManager);
        foreach (var entity in entities)
        {
            var obj = entity.GetComponent<FarmEntity>();

            farmEntities.Add(obj.data.seed.ToString(), obj);
        }

    }
    private void Start()
    {
        //call back ui
        OnCoinsAndLevelUpdate?.Invoke(coins, farmUpgradeData.level);
        OnProductDataUpdate?.Invoke(farmProductDatas);
        OnSeedDataUpdate?.Invoke(seedDatas);
    }
    public GameObject GetRandomSeed()
    {
        if (seedDatas == null || seedDatas.Count == 0)
            return null;

        int randomIndex = Random.Range(0, seedDatas.Count);
        SeedData seedData = seedDatas[randomIndex];

        if (seedData == null)
            return null;

        if (!farmEntities.TryGetValue(seedData.type.ToString(), out var entity) || entity == null)
            return null;

        if (seedData.quantity > 0)
        {
            seedData.quantity--;
        }
        else
        {
            seedDatas.RemoveAt(randomIndex);
            seedDataBase.Remove(seedData.type.ToString());
        }

        OnSeedDataUpdate?.Invoke(seedDatas);
        return entity.gameObject;
    }

    public void AddSeed(SeedData seedData)
    {
        if (seedDataBase.TryGetValue(seedData.type.ToString(), out var seed))
        {
            //logic add seed
            seed.quantity += seedData.quantity;
            coins -= seedData.price * seedData.quantity;

            //call back UI
            OnSeedDataUpdate?.Invoke(seedDatas);
            OnCoinsAndLevelUpdate?.Invoke(coins, farmUpgradeData.level);
        }
        else
        {
            //logic create seed when not have
            seedDatas.Add(seedData);
            seedDataBase.Add(seedData.type.ToString(), seedData);
            coins -= seedData.price * seedData.quantity;

            //call back UI
            OnSeedDataUpdate?.Invoke(seedDatas);
            OnCoinsAndLevelUpdate?.Invoke(coins, farmUpgradeData.level);
        }
    }
    public void RemoveSeed(SeedData seedData)
    {
        if (farmProductDataBase.TryGetValue(seedData.type.ToString(), out var seed))
        {
            if (seedData.quantity > seed.quantity)
            {
                Debug.Log("Not enough seeds");
                return;
            }
            seed.quantity -= seedData.quantity;
        }
    }
    public void AddFarmProduct(EntityData entityData)
    {
        if (farmProductDataBase.TryGetValue(entityData.type.ToString(), out var productData))
        {
            productData.quantity += entityData.yieldAmount;
            productData.price = entityData.price;
            productData.yieldAmount = entityData.yieldAmount;
            OnProductDataUpdate?.Invoke(farmProductDatas);
        }
        else
        {
            FarmProductData newProductData = new FarmProductData();
            newProductData.type = entityData.type;
            newProductData.quantity = 1;
            newProductData.price = entityData.price;
            newProductData.yieldAmount = entityData.yieldAmount;
            farmProductDatas.Add(newProductData);
            farmProductDataBase.Add(newProductData.type.ToString(), newProductData);
        }
    }
    public void RemoveFarmProduct(FarmProductData farmProduct)
    {
        if (farmProductDataBase.TryGetValue(farmProduct.type.ToString(), out var productData))
        {
            int amountToRemove = productData.quantity - farmProduct.quantity;

            if (amountToRemove <= 0)
            {
                farmProductDataBase.Remove(farmProduct.type.ToString());
                farmProductDatas.Remove(productData);
                coins += farmProduct.price * farmProduct.quantity;
            }
            else
            {
                productData.quantity = amountToRemove;
                coins += farmProduct.price * farmProduct.quantity;
            }
            OnProductDataUpdate?.Invoke(farmProductDatas);
            OnCoinsAndLevelUpdate?.Invoke(coins, farmUpgradeData.level);
        }
    }
    public void UpgradeFarm(int countLevel = 1)
    {
        if (farmUpgradeData.Upgrade(ref coins, countLevel))
        {
            OnCoinsAndLevelUpdate?.Invoke(coins, farmUpgradeData.level);
        }
    }
    public bool GetObj(ShopItemType shopItemType)
    {
        int price = 0;

        switch (shopItemType)
        {
            case ShopItemType.Seed:
                price = 15;
                break;
            case ShopItemType.Farmer:
                price = 15;
                break;
            case ShopItemType.Dirt:
                price = 500;
                break;
            case ShopItemType.Animal:
                price = 100;
                break;
            default:
                return false;
        }

        // kiểm tra đủ tiền trước khi trừ
        if (!CheckEnoughCoin(price))
        {
            Debug.Log("Không đủ tiền để mua " + shopItemType);
            return false;
        }
        return true;
    }

    public bool CheckEnoughCoin(int monney)
    {
        if (coins - monney < 0)
        {
            return false;
        }
        coins -= monney;
        OnCoinsAndLevelUpdate.Invoke(coins, farmUpgradeData.level);
        return true;
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
                farmProductDataBase.Add(productData.type.ToString(), productData);
                farmProductDatas.Add(productData);
            }
            foreach (var seedData in gameData.seedDatas)
            {
                seedDatas.Add(seedData);
                seedDataBase.Add(seedData.type.ToString(), seedData);
            }

            // call back ui
            OnSeedDataUpdate?.Invoke(seedDatas);
            OnProductDataUpdate?.Invoke(farmProductDatas);
            OnCoinsAndLevelUpdate?.Invoke(coins, farmUpgradeData.level);
        }
        catch (System.Exception ex)
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
        catch (System.Exception ex)
        {
            Debug.Log("Inventory error Save: " + ex.Message);
        }
    }
}