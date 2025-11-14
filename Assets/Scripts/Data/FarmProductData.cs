using System;
using Newtonsoft.Json;

[Serializable]
public class FarmProductData
{
    public ProductSaleAble type;
    public int yieldAmount;
    public int price;
    public int quantity;
    public FarmProductData Clone()
    {
        return (FarmProductData)this.MemberwiseClone();
    }
}