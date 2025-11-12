using System.Collections.Generic;
using UnityEngine;
public enum ProductBuyAble
{
    Cow,
    Tomato,
    Bluebarry,
    Dirt

}
public enum ProductSaleAble
{
    CowSMilk,
    TomatoSeed,
    Bluebarry,
    Strawberry

}
public class Shop : Singleton<Shop>
{
    private Inventory inventory;
    [System.Serializable]
    public class ShopItemData
    {
        public ProductBuyAble product;
        public string name;
        public int price;
        public string input = "Input in here >>>";
        public string description = "";
        public string validCharacters = "0123456789";
        public int characterLimit = 3;
    }
    [System.Serializable]
    public class ShopItemSaleData
    {
        public ProductSaleAble product;
        public string name;
        public int price;
        public int amount;
    }
    
    private void Awake() {
        inventory = FindAnyObjectByType<Inventory>();
    }
    public List<ShopItemData> buyableItems;
    public List<ShopItemSaleData> saleableItems;
    public void Buy(ShopItemData itemProduct, int quality)
    {
        var item = buyableItems.Find(x => x.product == itemProduct.product);
        if (item == null) return;

        if (inventory.coins >= item.price)
        {
            SeedData seedData = new SeedData();
            seedData.name = item.name;
            seedData.price = item.price;
            seedData.yieldAmount = quality;
            seedData.type = item.product;
            inventory.AddSeed(seedData);
            Debug.Log($"💰 Mua {item.product} thành công, còn {inventory.coins} vàng");
        }
        else
        {
            Debug.Log("❌ Không đủ tiền để mua!");
        }
    }

    // Bán item
    public void Sell(ProductSaleAble product)
    {
        // if (!inventory.HasItem(product.ToString(), 1))
        // {
        //     Debug.Log("❌ Không có item để bán!");
        //     return;
        // }

        // var item = saleableItems.Find(x => x.product.ToString() == product.ToString());
        // if (item == null) return;

        // inventory.RemoveItem(product.ToString(), 1);
        // playerCoins += item.price;
        // Debug.Log($"💰 Bán {item.name} thành công, hiện có {playerCoins} vàng");
    }


}