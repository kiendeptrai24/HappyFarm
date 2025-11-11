using System;
using Newtonsoft.Json;

[Serializable]
public class FarmProductData
{
    public string name;
    public int yieldAmount;
    public int price;
    public FarmProductData Clone()
    {
        return (FarmProductData)this.MemberwiseClone();
    }
}