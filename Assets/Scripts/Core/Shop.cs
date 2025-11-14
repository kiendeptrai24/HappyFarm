using System.Collections.Generic;
using UnityEngine;
public enum ProductBuyAble
{
    Cow,
    Tomato,
    Bluebarry,
    Dirt,
    Farmer,
    None


}
public enum ProductSaleAble
{
    CowMilk,
    Tomato,
    Bluebarry,
    Strawberry,
    None

}
public class Shop : Singleton<Shop>
{
    private Inventory inventory;
    [System.Serializable]
    public class ShopItemData
    {
        public ProductBuyAble product;
        public string name;
        public int quality;
        public int price;
        public int yieldAmount;
        public string input = "Input in here >>>";
        public string description = "";
        public string validCharacters = "0123456789";
        public int characterLimit = 3;
    }
    [System.Serializable]
    public class ShopItemSaleData
    {
        public ProductSaleAble type;
        public int price;
        public int quality;
    }

    private void Awake()
    {
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
            seedData.yieldAmount = item.yieldAmount;
            seedData.quantity = quality;
            seedData.type = item.product;
            inventory.AddSeed(seedData);
            Debug.Log($"💰 Mua {item.product} thành công với số lượng {seedData.quantity}, còn {inventory.coins} vàng");
        }
        else
        {
            Debug.Log("❌ Không đủ tiền để mua!");
        }
    }

    // Bán item
    public void Sell(ShopItemSaleData sale, int quality)
    {
        var item = saleableItems.Find(x => x.type == sale.type);
        if (item == null)
        {
            Debug.Log("❌ Không tìm thấy sản phẩm để bán!");
            return;
        }

        FarmProductData farmProductData = new FarmProductData();
        farmProductData.type = item.type;
        farmProductData.price = item.price;
        farmProductData.quantity = quality;
        inventory.RemoveFarmProduct(farmProductData);
    }


}