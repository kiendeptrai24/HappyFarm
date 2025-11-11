using System;
using Newtonsoft.Json;

[Serializable]
public class DirtData
{
    [JsonProperty] public string nameOfEntiy;
    [JsonProperty] public bool hasEntity;

    // Constructor rỗng để deserialize
    public DirtData()
    {
        nameOfEntiy = null;
        hasEntity = false;
    }

    // Constructor tiện lợi
    public DirtData(string nameOfEntiy, bool hasEntity)
    {
        this.nameOfEntiy = nameOfEntiy;
        this.hasEntity = hasEntity;
    }
    public DirtData Clone()
    {
        return (DirtData)this.MemberwiseClone();
    }
}