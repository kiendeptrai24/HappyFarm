using System;
using Newtonsoft.Json;

[Serializable]
public class SeedData
{
    public ProductBuyAble type;
    public string name;
    public int yieldAmount;
    public int quantity;
    public int price;
    public SeedData Clone()
    {
        return (SeedData)this.MemberwiseClone();
    }
}
