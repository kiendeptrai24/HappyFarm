using System;
using Newtonsoft.Json;

[Serializable]
public class SeedData
{
    public string name;
    public int yieldAmount;
    public int price;
    public SeedData Clone()
    {
        return (SeedData)this.MemberwiseClone();
    }
}
